using System.Diagnostics.CodeAnalysis;
using Azure.Identity;
using Contentful.AspNetCore;
using Dfe.EarlyYearsQualification.Content.RichTextParsing;
using Dfe.EarlyYearsQualification.Content.Services;
using Dfe.EarlyYearsQualification.Content.Services.Interfaces;
using Dfe.EarlyYearsQualification.Mock.Extensions;
using Dfe.EarlyYearsQualification.Web.Filters;
using Dfe.EarlyYearsQualification.Web.Helpers;
using Dfe.EarlyYearsQualification.Web.Models.Content.QuestionModels.Validators;
using Dfe.EarlyYearsQualification.Web.Security;
using Dfe.EarlyYearsQualification.Web.Services.Cookies;
using Dfe.EarlyYearsQualification.Web.Services.CookiesPreferenceService;
using Dfe.EarlyYearsQualification.Web.Services.DatesAndTimes;
using Dfe.EarlyYearsQualification.Web.Services.Notifications;
using Dfe.EarlyYearsQualification.Web.Services.Notifications.Options;
using Dfe.EarlyYearsQualification.Web.Services.QualificationDetails;
using Dfe.EarlyYearsQualification.Web.Services.QualificationSearch;
using Dfe.EarlyYearsQualification.Web.Services.UserJourneyCookieService;
using GovUk.Frontend.AspNetCore;
using Microsoft.ApplicationInsights.AspNetCore.Extensions;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Notify.Client;
using Notify.Interfaces;
using OwaspHeaders.Core.Extensions;

var builder = WebApplication.CreateBuilder(args);

var applicationInsightsServiceOptions = new ApplicationInsightsServiceOptions
                                        {
                                            EnableAdaptiveSampling = false
                                        };

builder.Services.AddApplicationInsightsTelemetry(applicationInsightsServiceOptions);
builder.WebHost.ConfigureKestrel(serverOptions => { serverOptions.AddServerHeader = false; });

var useMockContentful = builder.Configuration.GetValue<bool>("UseMockContentful");
if (!useMockContentful)
{
    var keyVaultEndpoint = builder.Configuration.GetSection("KeyVault").GetValue<string>("Endpoint");
    builder.Configuration.AddAzureKeyVault(new Uri(keyVaultEndpoint!), new DefaultAzureCredential());

    builder.Services
           .AddDataProtection()
           .ProtectKeysWithAzureKeyVault(new Uri($"{keyVaultEndpoint}keys/data-protection"),
                                         new DefaultAzureCredential());

    if (!builder.Environment.IsDevelopment())
    {
        var blobStorageConnectionString =
            builder.Configuration
                   .GetSection("Storage")
                   .GetValue<string>("ConnectionString");

        const string containerName = "data-protection";
        const string blobName = "data-protection";

        builder.Services
               .AddDataProtection()
               .PersistKeysToAzureBlobStorage(blobStorageConnectionString,
                                              containerName,
                                              blobName);
    }
}

// Add services to the container.
builder.Services.AddAntiforgery(options =>
                                {
                                    options.Cookie = new AntiForgeryCookieBuilder
                                                     {
                                                         Name = ".AspNetCore.Antiforgery",
                                                         SameSite = SameSiteMode.Strict,
                                                         HttpOnly = true,
                                                         IsEssential = true,
                                                         SecurePolicy = CookieSecurePolicy.None
                                                     };
                                });

builder.Services.AddControllersWithViews(options =>
                                         {
                                             // Ensures that all POST actions are protected by default.
                                             options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute());
                                             options.Filters.Add(new ResponseCacheAttribute
                                                                 {
                                                                     NoStore = true,
                                                                     Location = ResponseCacheLocation.None
                                                                 });
                                         });

builder.Services.AddContentful(builder.Configuration);

builder.Services.AddGovUkFrontend();

if (useMockContentful)
{
    builder.Services.AddMockContentfulServices();
}
else
{
    builder.Services.AddTransient<IContentService, ContentfulContentService>();
    builder.Services.AddTransient<IQualificationsRepository, QualificationsRepository>();
}

builder.Services.AddTransient<IQualificationDetailsService, QualificationDetailsService>();
builder.Services.AddTransient<IQualificationSearchService, QualificationSearchService>();
builder.Services.AddModelRenderers();
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddScoped<ICookieManager, CookieManager>();
builder.Services.AddTransient<ICookiesPreferenceService, CookiesPreferenceService>();
builder.Services.AddScoped<IUserJourneyCookieService, UserJourneyCookieService>();
builder.Services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();
builder.Services.AddScoped(x =>
                           {
                               var actionContext = x.GetRequiredService<IActionContextAccessor>().ActionContext;
                               var factory = x.GetRequiredService<IUrlHelperFactory>();
                               return factory.GetUrlHelper(actionContext!);
                           });

builder.Services.AddSingleton<IFuzzyAdapter, FuzzyAdapter>();
builder.Services.AddSingleton<IDateTimeAdapter, DateTimeAdapter>();
builder.Services.AddSingleton<IDateQuestionModelValidator, DateQuestionModelValidator>();
builder.Services.AddTransient<TrackingConfiguration>();
builder.Services.AddTransient<OpenGraphDataHelper>();
builder.Services.AddSingleton<IPlaceholderUpdater, PlaceholderUpdater>();
builder.Services.AddSingleton<ICheckServiceAccessKeysHelper, CheckServiceAccessKeysHelper>();

if (useMockContentful)
{
    builder.Services.AddSingleton<INotificationService, MockNotificationService>();
}
else
{
    builder.Services.Configure<NotificationOptions>(builder.Configuration.GetSection("Notifications"));
    builder.Services.AddSingleton<INotificationClient, NotificationClient>(_ =>
                                                                           {
                                                                               var options = builder.Configuration.GetSection("Notifications").Get<NotificationOptions>();
                                                                               return new NotificationClient(options!.ApiKey);
                                                                           });
    builder.Services.AddSingleton<INotificationService, GovUkNotifyService>();
}

var accessIsChallenged = !builder.Configuration.GetValue<bool>("ServiceAccess:IsPublic");
// ...by default, challenge the user for the secret value unless that's explicitly turned off

if (accessIsChallenged)
{
    builder.Services.AddScoped<IChallengeResourceFilterAttribute, ChallengeResourceFilterAttribute>();
}
else
{
    builder.Services.AddSingleton<IChallengeResourceFilterAttribute, NoChallengeResourceFilterAttribute>();
}

var app = builder.Build();

app.UseSecureHeadersMiddleware(
                               SecureHeaderConfiguration.CustomConfiguration()
                              );

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment() || useMockContentful)
{
    app.UseExceptionHandler("/Error");
    app.UseStatusCodePagesWithReExecute("/Error/{0}");

    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
else
{
    app.UseDeveloperExceptionPage();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
                       "default",
                       "{controller=Home}/{action=Index}/{id?}");

await app.RunAsync();

[ExcludeFromCodeCoverage]
// ReSharper disable once UnusedType.Global
// ...declared partial so we can exclude it from code coverage calculations
public static partial class Program;