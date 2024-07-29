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

        Func<KeyValuePair<string, string>, bool> isEmptyPredicate =
            keyValuePair => string.IsNullOrEmpty(keyValuePair.Value);

        // ReSharper disable once ConvertIfStatementToReturnStatement
        if (dictionary.Any(isEmptyPredicate))
        {
            var key = dictionary.First(isEmptyPredicate).Key;
            return new ValidationResult($"Value is required for key: {key}");
        }

        return ValidationResult.Success;
    }
}