namespace Dfe.EarlyYearsQualification.Content.Services.Interfaces;

public interface IQualificationDownloadService
{
    // ReSharper disable once IdentifierTypo
    Task GenerateEyqlDownload();
    
    // ReSharper disable once IdentifierTypo
    Task<string> GetEyqlDownloadLink();
}