namespace Dfe.EarlyYearsQualification.Web.Services.Environments;

public class EnvironmentService(IConfiguration? config) : IEnvironmentService
{
    public bool IsProduction()
    {
        if (config?["UseMockContentful"]?.ToLower() == "true") return false;
        
        string environment = config?["ENVIRONMENT"] ?? "production";
        // ...safest to assume production if environment not configured

        return environment.StartsWith("prod", StringComparison.OrdinalIgnoreCase);
    }
}