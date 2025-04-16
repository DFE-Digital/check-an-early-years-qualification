using Dfe.EarlyYearsQualification.Web.Attributes;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Dfe.EarlyYearsQualification.Web.ContractResolvers;

public class TelemetryContractResolver : DefaultContractResolver
{
    protected override IList<JsonProperty> CreateProperties(Type type, MemberSerialization memberSerialization)
    {
        var baseProperties = base.CreateProperties(type, memberSerialization);
        var propertiesToIncludeInTelemetry = new Dictionary<string, bool>();

        foreach (var property in type.GetProperties())
        {
            var customAttributes = property.GetCustomAttributes(true);
            
            var includeInTelemetryAttribute = customAttributes
                                              .OfType<IncludeInTelemetryAttribute>()
                                              .FirstOrDefault();
            
            var sensitiveAttribute = customAttributes
                                     .OfType<SensitiveAttribute>()
                                     .FirstOrDefault();
            
            if (includeInTelemetryAttribute is not null)
            {
                propertiesToIncludeInTelemetry.Add(property.Name.ToUpperInvariant(), sensitiveAttribute != null);
            }
        }

        var processedProperties = new List<JsonProperty>();

        foreach (var baseProperty in baseProperties)
        {
            if (baseProperty.PropertyName is null ||
                !propertiesToIncludeInTelemetry.ContainsKey(baseProperty.PropertyName.ToUpperInvariant()))
            {
                continue;
            }
            baseProperty.PropertyType = typeof(string);
            baseProperty.ValueProvider = propertiesToIncludeInTelemetry[baseProperty.PropertyName.ToUpperInvariant()] ? new ObfuscatorValueProvider() : baseProperty.ValueProvider;
            processedProperties.Add(baseProperty);
        }

        return processedProperties;
    }
}