using Dfe.EarlyYearsQualification.Content.Entities;
using Dfe.EarlyYearsQualification.Web.Models.Content;

namespace Dfe.EarlyYearsQualification.UnitTests.Models.Content;

[TestClass]
public class QualificationWebViewModelTests
{
    [TestMethod]
    public void Constructor_WithValidQualification_SetsPropertiesCorrectly()
    {
        // Arrange
        var qualification = new Qualification("qual-id", "Qual Name", "Awarding Org", 3)
        {
            FromWhichYear = "Sep-14",
            ToWhichYear = "Aug-19",
            AdditionalRequirements = "Some requirements",
            StaffChildRatio = 5,
            QualificationNumber = "123/456/789",
            EyqlTabs = [new() { Heading = "Post-September 2014" }]
        };

        // Act
        var model = new QualificationWebViewModel(qualification);

        // Assert
        model.FromWhichYear.Should().Be("September 2014");
        model.ToWhichYear.Should().Be("August 2019");
        model.AdditionalRequirements.Should().Be("Some requirements");
        model.StaffChildRatio.Should().Be(5);
        model.QualificationNumber.Should().Be("123 / 456 / 789");
        model.EyqlTabs.Should().NotBeNull();
        model.EyqlTabs.First().Heading.Should().Be("Post-September 2014");
    }

    [TestMethod]
    [DataRow("")]
    [DataRow(null)]
    public void Constructor_WithInvalidAdditionalRequirements_SetsToNone(string? input)
    {
        // Arrange
        var qualification = new Qualification("qual-id", "Qual Name", "Awarding Org", 3)
        {
            AdditionalRequirements = input,
            EyqlTabs = []
        };

        // Act
        var model = new QualificationWebViewModel(qualification);

        // Assert
        model.AdditionalRequirements.Should().Be("None");
    }

    [TestMethod]
    [DataRow("")]
    [DataRow(null)]
    public void Constructor_WithInvalidQualificationNumber_SetsToMissingValue(string? input)
    {
        // Arrange
        var qualification = new Qualification("qual-id", "Qual Name", "Awarding Org", 3)
        {
            QualificationNumber = input,
            EyqlTabs = []
        };

        // Act
        var model = new QualificationWebViewModel(qualification);

        // Assert
        model.QualificationNumber.Should().Be("-");
    }

    [TestMethod]
    [DataRow("")]
    [DataRow(null)]
    [DataRow("null")]
    public void Constructor_WithInvalidFromWhichYear_SetsToMissingValue(string? input)
    {
        // Arrange
        var qualification = new Qualification("qual-id", "Qual Name", "Awarding Org", 3)
        {
            FromWhichYear = input,
            EyqlTabs = []
        };

        // Act
        var model = new QualificationWebViewModel(qualification);

        // Assert
        model.FromWhichYear.Should().Be("-");
    }

    [TestMethod]
    [DataRow("")]
    [DataRow(null)]
    [DataRow("null")]
    public void Constructor_WithInvalidToWhichYear_SetsToMissingValue(string? input)
    {
        // Arrange
        var qualification = new Qualification("qual-id", "Qual Name", "Awarding Org", 3)
        {
            ToWhichYear = input,
            EyqlTabs = []
        };

        // Act
        var model = new QualificationWebViewModel(qualification);

        // Assert
        model.ToWhichYear.Should().Be("-");
    }

    [TestMethod]
    public void Constructor_WithInvalidDateFormatFromWhichYear_KeepsOriginalValue()
    {
        // Arrange
        var qualification = new Qualification("qual-id", "Qual Name", "Awarding Org", 3)
        {
            FromWhichYear = "InvalidDate",
            EyqlTabs = []
        };

        // Act
        var model = new QualificationWebViewModel(qualification);

        // Assert
        model.FromWhichYear.Should().Be("InvalidDate");
    }

    [TestMethod]
    public void Constructor_WithInvalidDateFormatToWhichYear_KeepsOriginalValue()
    {
        // Arrange
        var qualification = new Qualification("qual-id", "Qual Name", "Awarding Org", 3)
        {
            ToWhichYear = "InvalidDate",
            EyqlTabs = []
        };

        // Act
        var model = new QualificationWebViewModel(qualification);

        // Assert
        model.ToWhichYear.Should().Be("InvalidDate");
    }

    [TestMethod]
    public void Constructor_WithStaffChildRatio_SetsValueCorrectly()
    {
        // Arrange
        var qualification = new Qualification("qual-id", "Qual Name", "Awarding Org", 3)
        {
            StaffChildRatio = 2,
            EyqlTabs = []
        };

        // Act
        var model = new QualificationWebViewModel(qualification);

        // Assert
        model.StaffChildRatio.Should().Be(2);
    }

    [TestMethod]
    public void Constructor_WithEyqlTabs_SetsTabsCorrectly()
    {
        // Arrange
        var tabs = new List<Tab> { new() { Heading = "Post-September 2014" }, new() { Heading = "Post-September 2024" } };
        var qualification = new Qualification("qual-id", "Qual Name", "Awarding Org", 3)
        {
            EyqlTabs = tabs
        };

        // Act
        var model = new QualificationWebViewModel(qualification);

        // Assert
        model.EyqlTabs.Should().BeSameAs(tabs);
    }
}