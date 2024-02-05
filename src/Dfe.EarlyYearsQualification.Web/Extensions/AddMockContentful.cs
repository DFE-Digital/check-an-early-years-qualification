using Dfe.EarlyYearsQualification.Content.Services;
using Moq;

namespace Dfe.EarlyYearsQualification.Web.Extensions
{
  public static class IServiceCollectionExtensions
  {
    public static IServiceCollection AddMockContentful(this IServiceCollection services)
    {

      var mockContentfulService = new Mock<IContentService>();

      mockContentfulService.Setup(x => x.GetLandingPage()).ReturnsAsync(new Content.Entities.LandingPage()
      {
        Header = "Test Header",
        ServiceIntroduction = new Contentful.Core.Models.Document(),
        StartButtonText = "Test Start Button Text"
      });

      services.AddSingleton(mockContentfulService.Object);
      return services;
    }
  }
}