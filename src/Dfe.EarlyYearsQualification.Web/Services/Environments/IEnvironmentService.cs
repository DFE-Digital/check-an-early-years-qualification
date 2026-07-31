namespace Dfe.EarlyYearsQualification.Web.Services.Environments;

public interface IEnvironmentService
{
    string GetEnvironment();

    bool IsProduction();
}