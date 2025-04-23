using Newtonsoft.Json.Serialization;

namespace Dfe.EarlyYearsQualification.Web.ContractResolvers;

public class TelemetryProperty
{
    public bool IncludeInTelemetry { get; init; }

    public bool IsSensitiveValue { get; init; }

    public required JsonProperty JsonProperty { get; init; }
}