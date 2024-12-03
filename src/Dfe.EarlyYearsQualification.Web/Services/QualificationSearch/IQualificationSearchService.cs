using Dfe.EarlyYearsQualification.Content.Entities;
using Dfe.EarlyYearsQualification.Web.Models.Content;

namespace Dfe.EarlyYearsQualification.Web.Services.QualificationSearch;

public interface IQualificationSearchService
{
    void Refine(string refineSearch);
    Task<QualificationListModel?> GetQualifications();
    Task<List<Qualification>> GetFilteredQualifications();
    FilterModel GetFilterModel(QualificationListPage content);
    List<BasicQualificationModel> GetBasicQualificationsModels(List<Qualification> qualifications);
}