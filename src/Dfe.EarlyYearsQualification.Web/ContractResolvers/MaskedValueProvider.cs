using System.Diagnostics.CodeAnalysis;
using Newtonsoft.Json.Serialization;

namespace Dfe.EarlyYearsQualification.Web.ContractResolvers;

public class MaskedValueProvider : IValueProvider
{
    [ExcludeFromCodeCoverage(Justification = "This is not used")]
    public void SetValue(object target, object? value)
    {
        // We don't use this
    }

    public object GetValue(object target)
    {
        return "****";
    }
}