using Dfe.EarlyYearsQualification.Content.Entities;
using Dfe.EarlyYearsQualification.Web.Models.Content;

namespace Dfe.EarlyYearsQualification.Web.Mappers.Interfaces;

public interface IStaticPageMapper
{
    Task<StaticPageModel> Map(StaticPage page);

    Task<QualificationNotOnListPageModel> Map(CannotFindQualificationPage cannotFindQualificationPage);
}