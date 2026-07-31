using Dfe.EarlyYearsQualification.Content.Entities;
using Dfe.EarlyYearsQualification.Web.Models.Content;

namespace Dfe.EarlyYearsQualification.UnitTests.Models.Content;

[TestClass]
public class QualificationWebViewModelTests
{
    [TestMethod]
    public void Constructor_WithQualification_MapsBasePropertiesAndTabs()
    {
        // Arrange
        var tabs = new List<Tab>
        {
            new() { Heading = "Pre-September 2014", Order = 1 },
            new() { Heading = "September 2014 onwards", Order = 2 }
        };

        var qualification = new Qualification("qual-id", "Qualification name", "Awarding organisation", 3)
        {
            QualificationNumber = "123/456",
            EyqlTabs = tabs
        };

        // Act
        var result = new QualificationWebViewModel(qualification);

        // Assert
        result.QualificationId.Should().Be("qual-id");
        result.QualificationName.Should().Be("Qualification name");
        result.AwardingOrganisationTitle.Should().Be("Awarding organisation");
        result.QualificationLevel.Should().Be(3);
        result.QualificationNumber.Should().Be("123 / 456");
        result.EyqlTabs.Should().BeSameAs(tabs);
    }
}