using Newtonsoft.Json.Serialization;

namespace Dfe.EarlyYearsQualification.Web.ContractResolvers;

public class ObfuscatorValueProvider : IValueProvider
{
    public void SetValue(object target, object? value)
    {
        // We don't use this
    }

    public object GetValue(object target)
    {
        return "****";
    }
}