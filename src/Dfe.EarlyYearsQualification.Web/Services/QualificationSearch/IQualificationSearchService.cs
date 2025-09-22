using Dfe.EarlyYearsQualification.Content.Entities;
using Dfe.EarlyYearsQualification.Web.Models.Content;

namespace Dfe.EarlyYearsQualification.Web.Services.QualificationSearch;

public interface IQualificationSearchService
{
    void Refine(string refineSearch);
    Task<QualificationListModel?> GetQualifications();
    Task<QualificationListModel> MapList(QualificationListPage content, List<Qualification>? qualifications);
    Task<List<Qualification>> GetFilteredQualifications();
    Task<List<Qualification>> GetAllQualifications();
    FilterModel GetFilterModel(QualificationListPage content);
}