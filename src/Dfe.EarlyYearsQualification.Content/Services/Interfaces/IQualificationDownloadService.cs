namespace Dfe.EarlyYearsQualification.Content.Services.Interfaces;

public interface IQualificationDownloadService
{
    Task GenerateEyqlDownloadByEnvironment(string environment);

    Task<(byte[] fileContents, string fileName)> GetEyqlDownload(string environment);

    Task<byte[]?> GetEyqlDataForInternalDownload();
}