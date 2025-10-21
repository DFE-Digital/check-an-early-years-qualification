using Dfe.EarlyYearsQualification.Content.Entities;
using Dfe.EarlyYearsQualification.Web.Models.Content;

namespace Dfe.EarlyYearsQualification.Web.Mappers.Interfaces;

public interface IQualificationDetailsMapper
{
    Task<QualificationDetailsModel> Map(
        Qualification qualification,
        QualificationDetailsPage content,
        NavigationLink? backNavLink,
        List<AdditionalRequirementAnswerModel>? additionalRequirementAnswers,
        string dateStarted,
        string dateAwarded,
        List<Qualification> qualifications
        );
}