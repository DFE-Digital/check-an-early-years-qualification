using Dfe.EarlyYearsQualification.Content.Entities;
using Dfe.EarlyYearsQualification.Web.Models.Content;

namespace Dfe.EarlyYearsQualification.Web.Mappers.Interfaces;

public interface IConfirmQualificationPageMapper
{
    Task<ConfirmQualificationPageModel> Map(ConfirmQualificationPage content,
                                            Qualification qualification);
}