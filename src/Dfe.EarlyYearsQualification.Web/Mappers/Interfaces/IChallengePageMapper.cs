using Dfe.EarlyYearsQualification.Content.Entities;
using Dfe.EarlyYearsQualification.Web.Models.Content;

namespace Dfe.EarlyYearsQualification.Web.Mappers.Interfaces;

public interface IChallengePageMapper
{
    Task<ChallengePageModel> Map(ChallengePageModel model, ChallengePage content,
                                 string sanitisedReferralAddress);
}