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