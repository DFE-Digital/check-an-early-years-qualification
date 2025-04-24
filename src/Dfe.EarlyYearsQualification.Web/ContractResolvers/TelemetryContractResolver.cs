using Dfe.EarlyYearsQualification.Web.Attributes;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Dfe.EarlyYearsQualification.Web.ContractResolvers;

public class TelemetryContractResolver : DefaultContractResolver
{
    protected override IList<JsonProperty> CreateProperties(Type type, MemberSerialization memberSerialization)
    {
        var baseProperties = base.CreateProperties(type, memberSerialization);
        var processedProperties = new List<JsonProperty>();
        
        foreach (var property in baseProperties)
        {
            var checkedProperty = CheckPropertyForCustomAttributes(property, memberSerialization);
            if (checkedProperty.IncludeInTelemetry)
            {
                checkedProperty.JsonProperty.ValueProvider = checkedProperty.IsSensitiveValue ? new MaskedValueProvider() : checkedProperty.JsonProperty.ValueProvider;
                processedProperties.Add(checkedProperty.JsonProperty);
            }
        }
        
        return processedProperties;
    }

    private TelemetryProperty CheckPropertyForCustomAttributes(JsonProperty jsonProperty, MemberSerialization memberSerialization)
    {
        var customAttributes = jsonProperty.AttributeProvider!.GetAttributes(true);
            
        var includeInTelemetryAttribute = customAttributes
                                          .OfType<IncludeInTelemetryAttribute>()
                                          .FirstOrDefault();
            
        var sensitiveAttribute = customAttributes
                                 .OfType<SensitiveAttribute>()
                                 .FirstOrDefault();

        var hasSubPropertiesWithCustomAttributes =
            CheckSubPropertiesForCustomAttributes(jsonProperty, memberSerialization);

        var includeInTelemetry = includeInTelemetryAttribute is not null;

        return new TelemetryProperty
                     {
                         IncludeInTelemetry = includeInTelemetry || hasSubPropertiesWithCustomAttributes,
                         IsSensitiveValue = includeInTelemetry && sensitiveAttribute is not null,
                         JsonProperty = jsonProperty
                     };
    }
    
    private bool CheckSubPropertiesForCustomAttributes(JsonProperty property, MemberSerialization memberSerialization)
    {
        var subProperties = base.CreateProperties(property.PropertyType!, memberSerialization);
        foreach (var subProperty in subProperties)
        {
            var checkedSubProperty = CheckPropertyForCustomAttributes(subProperty, memberSerialization);
            if (checkedSubProperty.IncludeInTelemetry)
            {
                return true;
            }
        }

        return false;
    }
}