using Dfe.EarlyYearsQualification.Content.Entities;
using Dfe.EarlyYearsQualification.Web.Models.Content;

namespace Dfe.EarlyYearsQualification.Web.Mappers.Interfaces;

public interface IFeedbackFormPageMapper
{
    Task<FeedbackFormPageModel> Map(FeedbackFormPage feedbackFormPage);
}