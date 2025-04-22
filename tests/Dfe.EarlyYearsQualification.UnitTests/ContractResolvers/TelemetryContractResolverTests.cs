using Dfe.EarlyYearsQualification.Web.Attributes;
using Dfe.EarlyYearsQualification.Web.ContractResolvers;
using Newtonsoft.Json;

namespace Dfe.EarlyYearsQualification.UnitTests.ContractResolvers;

[TestClass]
public class TelemetryContractResolverTests
{
    [TestMethod]
    public void CreateProperties_PassInObjectWithNoIncludeInTelemetryAttributes_ReturnsEmptyObject()
    {
        var result = JsonConvert.SerializeObject(new NoIncludeInTelemetryAttributes(),
                                           new JsonSerializerSettings
                                           {
                                               ContractResolver = new TelemetryContractResolver()
                                           });
        result.Should().Match("{}");
    }
    
    [TestMethod]
    public void CreateProperties_PassInObjectWithIncludeInTelemetryAttributes_ReturnsExpectedObject()
    {
        var result = JsonConvert.SerializeObject(new SingleIncludeInTelemetryAttributes(),
                                                 new JsonSerializerSettings
                                                 {
                                                     ContractResolver = new TelemetryContractResolver()
                                                 });
        result.Should().Match("{\"Include\":\"Testing\"}");
    }
    
    [TestMethod]
    public void CreateProperties_PassInObjectWithMultipleIncludeInTelemetryAttributesAndSensitiveAttribute_ReturnsExpectedObject()
    {
        var result = JsonConvert.SerializeObject(new MultipleIncludeInTelemetryAttributes(),
                                                 new JsonSerializerSettings
                                                 {
                                                     ContractResolver = new TelemetryContractResolver()
                                                 });
        result.Should().Match("{\"Include\":\"Testing\",\"Sensitive\":\"****\"}");
    }
    
    [TestMethod]
    public void CreateProperties_PassInObjectWithSubClass_ReturnsExpectedObject()
    {
        var result = JsonConvert.SerializeObject(new SingleIncludeWithinSubClass(),
                                                 new JsonSerializerSettings
                                                 {
                                                     ContractResolver = new TelemetryContractResolver()
                                                 });
        result.Should().Match("{\"Child\":{\"Include\":\"Testing\"}}");
    }
    
    [TestMethod]
    public void CreateProperties_PassInObjectWithMultipleSubClasses_ReturnsExpectedObject()
    {
        var result = JsonConvert.SerializeObject(new MultipleSubClassesWithinIncludeAndSensitive(),
                                                 new JsonSerializerSettings
                                                 {
                                                     ContractResolver = new TelemetryContractResolver()
                                                 });
        result.Should().Match("{\"ChildWithInclude\":{\"Include\":\"Testing\"},\"ChildWithSensitive\":{\"Sensitive\":\"****\"}}");
    }
    
    [TestMethod]
    public void CreateProperties_PassInObjectWithSingleIncludeInTelemetryAttributeAndSensitiveAttribute_ReturnsExpectedObject()
    {
        var result = JsonConvert.SerializeObject(new ChildWithIncludeAndSensitive(),
                                                 new JsonSerializerSettings
                                                 {
                                                     ContractResolver = new TelemetryContractResolver()
                                                 });
        result.Should().Match("{\"Child\":{\"Include\":\"Testing\",\"Sensitive\":\"****\"}}");
    }
}

internal class NoIncludeInTelemetryAttributes
{
    public string Test { get; set; } = string.Empty;
}

internal class SingleIncludeInTelemetryAttributes
{
    public string Test { get; set; } = string.Empty;
    
    [IncludeInTelemetry]
    public string Include { get; set; } = "Testing";
}

internal class MultipleIncludeInTelemetryAttributes
{
    public string Test { get; set; } = string.Empty;
    
    [IncludeInTelemetry]
    public string Include { get; set; } = "Testing";

    [IncludeInTelemetry]
    [Sensitive]
    public string Sensitive { get; set; } = "Sensitive value";
}

internal class SingleIncludeWithinSubClass
{
    public SubClass Child { get; set; } = new();
}

internal class SubClass
{
    [IncludeInTelemetry]
    public string Include { get; set; } = "Testing";
}

internal class MultipleSubClassesWithinIncludeAndSensitive
{
    public SubClass ChildWithInclude { get; set; } = new();
    
    public SubClassWithSensitiveAttribute ChildWithSensitive { get; set; } = new();
}

internal class SubClassWithSensitiveAttribute
{
    public string FieldShouldBeIgnored { get; set; } = "Testing";
    
    [IncludeInTelemetry]
    [Sensitive]
    public string Sensitive { get; set; } = "Testing";
}

internal class ChildWithIncludeAndSensitive
{
    public MultipleIncludeInTelemetryAttributes Child { get; set; } = new();
}