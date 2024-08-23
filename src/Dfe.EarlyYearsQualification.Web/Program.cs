using System.Diagnostics.CodeAnalysis;
using Azure.Identity;
using Contentful.AspNetCore;
using Dfe.EarlyYearsQualification.Content.Extensions;
using Dfe.EarlyYearsQualification.Content.Services;
using Dfe.EarlyYearsQualification.Mock.Extensions;
using Dfe.EarlyYearsQualification.Web.Filters;
using Dfe.EarlyYearsQualification.Web.Helpers;
using Dfe.EarlyYearsQualification.Web.Models.Content.QuestionModels.Validators;
using Dfe.EarlyYearsQualification.Web.Security;
using Dfe.EarlyYearsQualification.Web.Services.Cookies;
using Dfe.EarlyYearsQualification.Web.Services.CookiesPreferenceService;
using Dfe.EarlyYearsQualification.Web.Services.DatesAndTimes;
using Dfe.EarlyYearsQualification.Web.Services.UserJourneyCookieService;
using GovUk.Frontend.AspNetCore;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using OwaspHeaders.Core.Extensions;
using RobotsTxt;

var builder = WebApplication.CreateBuilder(args);
builder.WebHost.ConfigureKestrel(serverOptions => { serverOptions.AddServerHeader = false; });

var useMockContentful = builder.Configuration.GetValue<bool>("UseMockContentful");
if (!useMockContentful)
{
    var keyVaultEndpoint = builder.Configuration.GetSection("KeyVault").GetValue<string>("Endpoint");
    builder.Configuration.AddAzureKeyVault(new Uri(keyVaultEndpoint!), new DefaultAzureCredential());

    var blobStorageConnectionString = builder.Configuration.GetSection("Storage").GetValue<string>("ConnectionString");
    builder.Services.AddDataProtection()
           .PersistKeysToAzureBlobStorage(blobStorageConnectionString, "data-protection", "data-protection")
           .ProtectKeysWithAzureKeyVault(new Uri($"{keyVaultEndpoint}keys/data-protection"),
                                         new DefaultAzureCredential());
}

// Add services to the container.
builder.Services.AddAntiforgery(options => options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest);
builder.Services.AddControllersWithViews(options =>
                                         {
                                             // Ensures that all POST actions are protected by default.
                                             options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute());
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
    builder.Services.AddTransient<IContentFilterService, ContentfulContentFilterService>();
}

builder.Services.AddModelRenderers();
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddScoped<ICookieManager, CookieManager>();
builder.Services.AddTransient<ICookiesPreferenceService, CookiesPreferenceService>();
builder.Services.AddTransient<IUserJourneyCookieService, UserJourneyCookieService>();
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
builder.Services.AddTransient<GtmConfiguration>();
builder.Services.AddSingleton<IPlaceholderUpdater, PlaceholderUpdater>();

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

builder.Services.AddStaticRobotsTxt(robotsTxtOptions => robotsTxtOptions.DenyAll());

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
app.UseRobotsTxt();
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