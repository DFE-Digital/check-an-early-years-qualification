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

        var keyWithNoValue = dictionary
                             .Where(keyValuePair => string.IsNullOrEmpty(keyValuePair.Value))
                             .Cast<KeyValuePair<string, string>?>()
                             .FirstOrDefault();

        return keyWithNoValue.HasValue
                   ? new ValidationResult($"Value is required for key: {keyWithNoValue.Value.Key}")
                   : ValidationResult.Success;
    }
}