namespace Dfe.EarlyYearsQualification.Web.Services.Environments;

public class EnvironmentService(IConfiguration? configuration) : IEnvironmentService
{
    private readonly IConfiguration? _configuration = configuration;

    public string GetEnvironment()
    {
        return _configuration?["ENVIRONMENT"] ?? "production";
    }

    public bool IsProduction()
    {
        if (_configuration?["UseMockContentful"]?.ToLower() == "true") return false;

        string environment = GetEnvironment();
        // ...safest to assume production if environment not configured

        return environment.StartsWith("prod", StringComparison.OrdinalIgnoreCase);
    }
}