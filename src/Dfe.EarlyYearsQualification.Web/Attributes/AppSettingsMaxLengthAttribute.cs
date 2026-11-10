using System.ComponentModel.DataAnnotations;

namespace Dfe.EarlyYearsQualification.Web.Attributes;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
public class AppSettingsMaxLengthAttribute(string configKey) : ValidationAttribute
{
    private readonly string _configKey = configKey ?? throw new ArgumentNullException(nameof(configKey));

    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value == null)
        {
            return ValidationResult.Success;
        }

        var configuration = validationContext.GetService<IConfiguration>();
        if (configuration == null)
        {
            throw new InvalidOperationException("IConfiguration service could not be resolved from ValidationContext.");
        }

        var maxLengthConfig = configuration[_configKey];
        if (!int.TryParse(maxLengthConfig, out var maxLength))
        {
            throw new InvalidOperationException($"Configuration key '{_configKey}' is missing or not a valid integer.");
        }

        if (value is string stringValue && stringValue.Length > maxLength)
        {
            var errorMessage = $"The field {validationContext.DisplayName} must be a string with a maximum length of {maxLength}.";

            return new ValidationResult(errorMessage);
        }

        return ValidationResult.Success;
    }
}