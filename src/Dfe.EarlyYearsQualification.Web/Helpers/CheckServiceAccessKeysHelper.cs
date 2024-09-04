namespace Dfe.EarlyYearsQualification.Web.Helpers;

public class CheckServiceAccessKeysHelper(IConfiguration configuration) : ICheckServiceAccessKeysHelper
{
    public bool AllowPublicAccess => configuration.GetValue<bool>("ServiceAccess:IsPublic");

    public IEnumerable<string> ConfiguredKeys
    {
        get
        {
            var keys = configuration
                       .GetSection("ServiceAccess")
                       .GetSection("Keys")
                       .Get<string[]>();

            return keys == null ? [] : keys.Where(k => !string.IsNullOrWhiteSpace(k));
        }
    }
}