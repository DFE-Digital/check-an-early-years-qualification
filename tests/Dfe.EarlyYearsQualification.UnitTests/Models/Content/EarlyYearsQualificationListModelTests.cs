using Dfe.EarlyYearsQualification.Web.Models.Content;

namespace Dfe.EarlyYearsQualification.UnitTests.Models.Content;

[TestClass]
public class EarlyYearsQualificationListModelTests
{
    [TestMethod]
    public void HasFilters_NoFiltersSet_ReturnsFalse()
    {
        // Arrange
        var model = new EarlyYearsQualificationListModel
        {
            SearchTermFilter = string.Empty,
            QualificationStartDateFilter = string.Empty,
            QualificationLevelFilter = string.Empty
        };

        // Act
        var result = model.HasFilters;

        // Assert
        result.Should().BeFalse();
    }

    [TestMethod]
    public void HasFilters_SearchTermFilterSet_ReturnsTrue()
    {
        // Arrange
        var model = new EarlyYearsQualificationListModel
        {
            SearchTermFilter = "test search",
            QualificationStartDateFilter = string.Empty,
            QualificationLevelFilter = string.Empty
        };

        // Act
        var result = model.HasFilters;

        // Assert
        result.Should().BeTrue();
    }

    [TestMethod]
    public void HasFilters_QualificationStartDateFilterSet_ReturnsTrue()
    {
        // Arrange
        var model = new EarlyYearsQualificationListModel
        {
            SearchTermFilter = string.Empty,
            QualificationStartDateFilter = "Pre-September 2014",
            QualificationLevelFilter = string.Empty
        };

        // Act
        var result = model.HasFilters;

        // Assert
        result.Should().BeTrue();
    }

    [TestMethod]
    public void HasFilters_QualificationLevelFilterSet_ReturnsTrue()
    {
        // Arrange
        var model = new EarlyYearsQualificationListModel
        {
            SearchTermFilter = string.Empty,
            QualificationStartDateFilter = string.Empty,
            QualificationLevelFilter = "3"
        };

        // Act
        var result = model.HasFilters;

        // Assert
        result.Should().BeTrue();
    }

    [TestMethod]
    public void HasFilters_AllFiltersSet_ReturnsTrue()
    {
        // Arrange
        var model = new EarlyYearsQualificationListModel
        {
            SearchTermFilter = "test search",
            QualificationStartDateFilter = "Pre-September 2014",
            QualificationLevelFilter = "3"
        };

        // Act
        var result = model.HasFilters;

        // Assert
        result.Should().BeTrue();
    }

    [TestMethod]
    public void HasFilters_NullFilters_ReturnsFalse()
    {
        // Arrange
        var model = new EarlyYearsQualificationListModel();

        // Act
        var result = model.HasFilters;

        // Assert
        result.Should().BeFalse();
    }

    [TestMethod]
    public void HasFilters_WhitespaceSearchTermFilter_ReturnsFalse()
    {
        // Arrange
        var model = new EarlyYearsQualificationListModel
        {
            SearchTermFilter = "   ",
            QualificationStartDateFilter = string.Empty,
            QualificationLevelFilter = string.Empty
        };

        // Act
        var result = model.HasFilters;

        // Assert
        result.Should().BeFalse();
    }

    [TestMethod]
    public void HasFilters_WhitespaceQualificationStartDateFilter_ReturnsFalse()
    {
        // Arrange
        var model = new EarlyYearsQualificationListModel
        {
            SearchTermFilter = string.Empty,
            QualificationStartDateFilter = "   ",
            QualificationLevelFilter = string.Empty
        };

        // Act
        var result = model.HasFilters;

        // Assert
        result.Should().BeFalse();
    }

    [TestMethod]
    public void HasFilters_WhitespaceQualificationLevelFilter_ReturnsFalse()
    {
        // Arrange
        var model = new EarlyYearsQualificationListModel
        {
            SearchTermFilter = string.Empty,
            QualificationStartDateFilter = string.Empty,
            QualificationLevelFilter = "   "
        };

        // Act
        var result = model.HasFilters;

        // Assert
        result.Should().BeFalse();
    }
}
