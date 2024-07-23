using System.ComponentModel.DataAnnotations;
using Dfe.EarlyYearsQualification.Web.Attributes;
using FluentAssertions;

namespace Dfe.EarlyYearsQualification.UnitTests.Attributes;

[TestClass]
public class AnswerValidationAttributeTests
{
    [TestMethod]
    public void IsValid_InvalidObject_ReturnsErrorMessage()
    {
        var model = new InvalidPropertyModel();
        List<ValidationResult> validationResults = []; 
        var result = Validator.TryValidateObject(model, new ValidationContext(model, null, null), validationResults, true );
        result.Should().BeFalse();
        validationResults.Count.Should().Be(1);
        validationResults[0].ErrorMessage.Should().Be("Object is not of type Dictionary<string, string>()");
    }
    
    [TestMethod]
    public void IsValid_DictionaryHasNoKeys_ReturnsErrorMessage()
    {
        var model = new ValidPropertyModel();
        List<ValidationResult> validationResults = []; 
        var result = Validator.TryValidateObject(model, new ValidationContext(model, null, null), validationResults, true );
        result.Should().BeFalse();
        validationResults.Count.Should().Be(1);
        validationResults[0].ErrorMessage.Should().Be("Answers are required");
    }
    
    [TestMethod]
    public void IsValid_DictionaryHasMissingValue_ReturnsErrorMessage()
    {
        var model = new ValidPropertyModel();
        model.Answers.Add("Test Key", "");
        List<ValidationResult> validationResults = []; 
        var result = Validator.TryValidateObject(model, new ValidationContext(model, null, null), validationResults, true );
        result.Should().BeFalse();
        validationResults.Count.Should().Be(1);
        validationResults[0].ErrorMessage.Should().Be("Value is required for key: Test Key");
    }
    
    [TestMethod]
    public void IsValid_NoIssues_ReturnsSuccessMessage()
    {
        var model = new ValidPropertyModel();
        model.Answers.Add("Test Key", "Test");
        List<ValidationResult> validationResults = []; 
        var result = Validator.TryValidateObject(model, new ValidationContext(model, null, null), validationResults, true );
        result.Should().BeTrue();
        validationResults.Count.Should().Be(0);
    }
}

internal class InvalidPropertyModel
{
    [AnswerValidation] public string Answers { get; set; } = string.Empty;
}

internal class ValidPropertyModel
{
    [AnswerValidation] public Dictionary<string,string> Answers { get; set; } = [];
}