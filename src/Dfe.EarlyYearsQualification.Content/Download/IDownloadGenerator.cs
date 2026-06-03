using Dfe.EarlyYearsQualification.Content.Entities;

namespace Dfe.EarlyYearsQualification.Content.Download;

public interface IDownloadGenerator
{
    string GenerateQualificationListContent(List<Qualification> qualifications);
}