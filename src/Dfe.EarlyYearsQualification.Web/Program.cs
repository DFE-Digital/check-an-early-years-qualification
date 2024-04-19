using Contentful.AspNetCore;
using Dfe.EarlyYearsQualification.Content.Services;
using Dfe.EarlyYearsQualification.Web.Extensions;
using Azure.Identity;
using OwaspHeaders.Core.Extensions;
using Dfe.EarlyYearsQualification.Web.Security;

var builder = WebApplication.CreateBuilder(args);
builder.WebHost.ConfigureKestrel(serverOptions => {
  serverOptions.AddServerHeader = false;
});

if (!builder.Configuration.GetValue<bool>("UseMockContentful"))
{
  var keyVaultEndpoint = builder.Configuration.GetSection("KeyVault").GetValue<string>("Endpoint");
  builder.Configuration.AddAzureKeyVault(new Uri(keyVaultEndpoint!), new DefaultAzureCredential());
}

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddContentful(builder.Configuration);

if (builder.Configuration.GetValue<bool>("UseMockContentful"))
{
  builder.Services.AddMockContentful();
} else
{
  builder.Services.AddTransient<IContentService, ContentfulContentService>();
}

var app = builder.Build();
app.UseSecureHeadersMiddleware(
  SecureHeaderConfiguration.CustomConfiguration()
);

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
  app.UseExceptionHandler("/Home/Error");
  // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
  app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
