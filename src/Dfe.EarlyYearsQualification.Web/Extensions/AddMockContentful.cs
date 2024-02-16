using Dfe.EarlyYearsQualification.Content.Services;
using Dfe.EarlyYearsQualification.Content.Entities;
using Moq;

namespace Dfe.EarlyYearsQualification.Web.Extensions
{
  public static class IServiceCollectionExtensions
  {
    public static IServiceCollection AddMockContentful(this IServiceCollection services)
    {

      var mockContentfulService = new Mock<IContentService>();

      mockContentfulService.Setup(x => x.GetLandingPage()).ReturnsAsync(new LandingPage()
      {
        Header = "Test Header",
        ServiceIntroduction = new Contentful.Core.Models.Document(),
        StartButtonText = "Test Start Button Text"
      });

      mockContentfulService.Setup(x => x.GetNavigationLinks()).ReturnsAsync(new List<NavigationLink>()
      {
        new NavigationLink() { DisplayText = "Privacy notice", Href="#"}
      });

      services.AddSingleton(mockContentfulService.Object);
      return services;
    }
  }
}