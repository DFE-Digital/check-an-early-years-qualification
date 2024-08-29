namespace Dfe.EarlyYearsQualification.Web.Helpers;

public interface ICheckServiceAccessKeysHelper
{
    public bool AllowPublicAccess { get; }
    
    public IEnumerable<string> ConfiguredKeys { get; }
}