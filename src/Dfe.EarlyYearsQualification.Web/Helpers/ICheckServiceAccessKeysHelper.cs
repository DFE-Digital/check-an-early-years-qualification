namespace Dfe.EarlyYearsQualification.Web.Helpers;

public interface ICheckServiceAccessKeysHelper
{
    bool AllowPublicAccess { get; }

    IEnumerable<string> ConfiguredKeys { get; }
}