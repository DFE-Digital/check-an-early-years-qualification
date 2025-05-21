using Dfe.EarlyYearsQualification.Content.Constants;
using Dfe.EarlyYearsQualification.Content.Entities;
using Dfe.EarlyYearsQualification.Content.Validators;

namespace Dfe.EarlyYearsQualification.UnitTests.Validators;

[TestClass]
public class DateValidatorTests
{
    [TestMethod]
    public void GetDay_ReturnsExpectedValue()
    {
        var dateValidator = new DateValidator(new Mock<ILogger<DateValidator>>().Object);
        var result = dateValidator.GetDay();

        result.Should().Be(28);
    }
    
    [TestMethod]
    public void GetDate_ValueIsNull_ReturnsExpectedValue()
    {
        var dateValidator = new DateValidator(new Mock<ILogger<DateValidator>>().Object);
        var result = dateValidator.GetDate(string.Empty);

        result.Should().BeNull();
    }
    
    [TestMethod]
    public void GetDate_ValueIsStringNull_ReturnsExpectedValue()
    {
        var dateValidator = new DateValidator(new Mock<ILogger<DateValidator>>().Object);
        var result = dateValidator.GetDate("null");

        result.Should().BeNull();
    }
    
    [TestMethod]
    public void GetDate_DataContainsInvalidDateFormat_LogsError()
    {
        var mockLogger = new Mock<ILogger<DateValidator>>();
        var dateValidator = new DateValidator(mockLogger.Object);

        dateValidator.GetDate("Sep15");

        mockLogger.VerifyError("dateString Sep15 has unexpected format");
    }
    
    [TestMethod]
    public void GetDate_DataContainsInvalidMonth_LogsError()
    {
        var mockLogger = new Mock<ILogger<DateValidator>>();
        var dateValidator = new DateValidator(mockLogger.Object);

        dateValidator.GetDate("Sept-15");

        mockLogger.VerifyError("dateString Sept-15 contains unexpected month value");
    }
    
    [TestMethod]
    public void GetDate_DataContainsInvalidYear_LogsError()
    {
        var mockLogger = new Mock<ILogger<DateValidator>>();
        var dateValidator = new DateValidator(mockLogger.Object);

        dateValidator.GetDate("Aug-1a");

        mockLogger.VerifyError("dateString Aug-1a contains unexpected year value");
    }
    
    [TestMethod]
    public void GetDate_PassInDate_ReturnsDateOnly()
    {
        var mockLogger = new Mock<ILogger<DateValidator>>();
        var dateValidator = new DateValidator(mockLogger.Object);

        var result = dateValidator.GetDate("Aug-15");

        result.Should().NotBeNull();
        result.Should().Be(new DateOnly(2015, 08, 28));
    }
    
    [TestMethod]
    public void GetDate_DateIsCaseInsensitive_ReturnsDateOnly()
    {
        var mockLogger = new Mock<ILogger<DateValidator>>();
        var dateValidator = new DateValidator(mockLogger.Object);

        var result = dateValidator.GetDate("aUg-15");

        result.Should().NotBeNull();
        result.Should().Be(new DateOnly(2015, 08, 28));
    }
    
    [TestMethod]
    public void ValidateDateEntry_StartDateAndEndDateNotNull_ReturnsQualification()
    {
        var mockLogger = new Mock<ILogger<DateValidator>>();
        var dateValidator = new DateValidator(mockLogger.Object);

        var qualificationStartDate = new DateOnly(2015, 9, 1);
        var qualificationEndDate = new DateOnly(2019, 8, 31);
        var enteredStartDate = new DateOnly(2016, 7, 31);
        var qualification = new Qualification("EYQ-123", "test", AwardingOrganisations.Ncfe, 3);

        var result = dateValidator.ValidateDateEntry(qualificationStartDate, qualificationEndDate, enteredStartDate, qualification);

        result.Should().NotBeNull();
        result.Should().Be(qualification);
    }
    
    [TestMethod]
    public void ValidateDateEntry_StartDateIsNullAndEndDateNotNull_ReturnsQualification()
    {
        var mockLogger = new Mock<ILogger<DateValidator>>();
        var dateValidator = new DateValidator(mockLogger.Object);

        var qualificationEndDate = new DateOnly(2019, 8, 31);
        var enteredStartDate = new DateOnly(2016, 7, 31);
        var qualification = new Qualification("EYQ-123", "test", AwardingOrganisations.Ncfe, 3);

        var result = dateValidator.ValidateDateEntry(null, qualificationEndDate, enteredStartDate, qualification);

        result.Should().NotBeNull();
        result.Should().Be(qualification);
    }
    
    [TestMethod]
    public void ValidateDateEntry_StartDateIsNotNullAndEndDateIsNull_ReturnsQualification()
    {
        var mockLogger = new Mock<ILogger<DateValidator>>();
        var dateValidator = new DateValidator(mockLogger.Object);
        
        var qualificationStartDate = new DateOnly(2015, 9, 1);
        var enteredStartDate = new DateOnly(2016, 7, 31);
        var qualification = new Qualification("EYQ-123", "test", AwardingOrganisations.Ncfe, 3);

        var result = dateValidator.ValidateDateEntry(qualificationStartDate, null, enteredStartDate, qualification);

        result.Should().NotBeNull();
        result.Should().Be(qualification);
    }
    
    [TestMethod]
    public void ValidateDateEntry_BothStartDateAndEndDateAreNull_ReturnsNull()
    {
        var mockLogger = new Mock<ILogger<DateValidator>>();
        var dateValidator = new DateValidator(mockLogger.Object);
        
        var enteredStartDate = new DateOnly(2016, 7, 31);
        var qualification = new Qualification("EYQ-123", "test", AwardingOrganisations.Ncfe, 3);

        var result = dateValidator.ValidateDateEntry(null, null, enteredStartDate, qualification);

        result.Should().BeNull();
    }
}