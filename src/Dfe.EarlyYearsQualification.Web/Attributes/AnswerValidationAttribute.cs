using System.ComponentModel.DataAnnotations;

namespace Dfe.EarlyYearsQualification.Web.Attributes;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
public class AnswerValidationAttribute : ValidationAttribute
{
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value is not Dictionary<string, string> dictionary)
        {
            return new ValidationResult("Object is not of type Dictionary<string, string>()");
        }

        if (dictionary.Count == 0)
        {
            return new ValidationResult("Answers are required");
        }

        foreach (var keyValuePair in dictionary.Where(keyValuePair => string.IsNullOrEmpty(keyValuePair.Value)))
        {
            return new ValidationResult($"Value is required for key: {keyValuePair.Key}");
        }
        
        return ValidationResult.Success;
    }
}