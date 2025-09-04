using Dfe.EarlyYearsQualification.Content.Entities;
using Dfe.EarlyYearsQualification.Web.Models.Content;

namespace Dfe.EarlyYearsQualification.Web.Mappers.Interfaces;

public interface IAdvicePageMapper
{
    Task<AdvicePageModel> Map(AdvicePage advicePage);
    Task<QualificationNotOnListPageModel> Map(CannotFindQualificationPage cannotFindQualificationPage);
}