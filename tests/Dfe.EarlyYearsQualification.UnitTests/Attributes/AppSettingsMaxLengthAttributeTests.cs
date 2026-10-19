using System.ComponentModel.DataAnnotations;
using Dfe.EarlyYearsQualification.Web.Attributes;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Dfe.EarlyYearsQualification.UnitTests.Attributes;

[TestClass]
public class AppSettingsMaxLengthAttributeTests
{

    [TestMethod]
    public void IsValid_ValueIsNull_ReturnsSuccess()
    {
        var attribute = new AppSettingsMaxLengthAttribute("ValidationRules:MaxCharacterLength");
        var context = CreateValidationContext(new object());
        
        var result = attribute.GetValidationResult(null, context);
        
        Assert.AreEqual(ValidationResult.Success, result);
    }

    [TestMethod]
    public void IsValid_StringLengthIsLessThanMax_ReturnsSuccess()
    {
        var attribute = new AppSettingsMaxLengthAttribute("ValidationRules:MaxCharacterLength");
        var context = CreateValidationContext(new object());
        string validString = "12345"; // Length = 5, Max = 10
        
        var result = attribute.GetValidationResult(validString, context);
        
        Assert.AreEqual(ValidationResult.Success, result);
    }

    [TestMethod]
    public void IsValid_StringLengthEqualsMax_ReturnsSuccess()
    {
        var attribute = new AppSettingsMaxLengthAttribute("ValidationRules:MaxCharacterLength");
        var context = CreateValidationContext(new object());
        string validString = "1234567890"; // Length = 10, Max = 10
        
        var result = attribute.GetValidationResult(validString, context);
        
        Assert.AreEqual(ValidationResult.Success, result);
    }

    [TestMethod]
    public void IsValid_StringLengthExceedsMax_ReturnsValidationError()
    {
        var attribute = new AppSettingsMaxLengthAttribute("ValidationRules:MaxCharacterLength");
        var context = CreateValidationContext(new object());
        string invalidString = "12345678901";
        
        var result = attribute.GetValidationResult(invalidString, context);
        
        Assert.IsNotNull(result);
        Assert.AreNotEqual(ValidationResult.Success, result);
        Assert.AreEqual("The field Object must be a string with a maximum length of 10.", result.ErrorMessage);
    }

    [TestMethod]
    public void IsValid_MissingConfigurationKey_ThrowsInvalidOperationException()
    {
        var attribute = new AppSettingsMaxLengthAttribute("NonExistent:Key");
        var context = CreateValidationContext(new object());

        Assert.Throws<InvalidOperationException>(() => attribute.GetValidationResult("TestString", context));
    }

    [TestMethod]
    public void IsValid_NonIntegerConfigurationValue_ThrowsInvalidOperationException()
    {
        var attribute = new AppSettingsMaxLengthAttribute("ValidationRules:InvalidConfigValue");
        var context = CreateValidationContext(new object());

        Assert.Throws<InvalidOperationException>(() => attribute.GetValidationResult("TestString", context));
    }

    [TestMethod]
    public void IsValid_MissingConfigurationService_ThrowsInvalidOperationException()
    {
        var attribute = new AppSettingsMaxLengthAttribute("ValidationRules:MaxCharacterLength");
        var emptyContext = new ValidationContext(new object());

        Assert.Throws<InvalidOperationException>(() => attribute.GetValidationResult("TestString", emptyContext));
    }
    
    private static ValidationContext CreateValidationContext(object instance)
    {
        var inMemorySettings = new Dictionary<string, string?>
                               {
                                   { "ValidationRules:MaxCharacterLength", "10" },
                                   { "ValidationRules:InvalidConfigValue", "NotAnInteger" }
                               };

        IConfiguration configuration = new ConfigurationBuilder()
                                       .AddInMemoryCollection(inMemorySettings)
                                       .Build();

        var services = new ServiceCollection();
        services.AddSingleton(configuration);
        var serviceProvider = services.BuildServiceProvider();
        return new ValidationContext(instance, serviceProvider, items: null);
    }
}