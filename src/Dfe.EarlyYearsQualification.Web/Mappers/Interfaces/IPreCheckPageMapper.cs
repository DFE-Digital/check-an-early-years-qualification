using Dfe.EarlyYearsQualification.Content.Entities;
using Dfe.EarlyYearsQualification.Web.Models.Content.QuestionModels;

namespace Dfe.EarlyYearsQualification.Web.Mappers.Interfaces;

public interface IPreCheckPageMapper
{
    Task<PreCheckPageModel> Map(PreCheckPage preCheckPage);
}