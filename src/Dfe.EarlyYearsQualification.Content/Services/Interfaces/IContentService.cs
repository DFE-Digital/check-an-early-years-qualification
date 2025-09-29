using Dfe.EarlyYearsQualification.Content.Entities;
using Dfe.EarlyYearsQualification.Content.Entities.Help;

namespace Dfe.EarlyYearsQualification.Content.Services.Interfaces;

public interface IContentService
{
    Task<StartPage?> GetStartPage();

    Task<List<NavigationLink>> GetNavigationLinks();

    Task<DetailsPage?> GetDetailsPage();

    Task<AdvicePage?> GetAdvicePage(string entryId);

    Task<RadioQuestionPage?> GetRadioQuestionPage(string entryId);

    Task<AccessibilityStatementPage?> GetAccessibilityStatementPage();

    Task<CookiesPage?> GetCookiesPage();

    Task<PhaseBanner?> GetPhaseBannerContent();

    Task<CookiesBanner?> GetCookiesBannerContent();

    Task<DatesQuestionPage?> GetDatesQuestionPage(string entryId);

    Task<DropdownQuestionPage?> GetDropdownQuestionPage(string entryId);

    Task<QualificationListPage?> GetQualificationListPage();

    Task<ConfirmQualificationPage?> GetConfirmQualificationPage();

    Task<CheckAdditionalRequirementsPage?> GetCheckAdditionalRequirementsPage();

    Task<ChallengePage?> GetChallengePage();

    Task<CannotFindQualificationPage?> GetCannotFindQualificationPage(int level, int startMonth, int startYear, bool isPractitionerSpecificPage);

    Task<CheckAdditionalRequirementsAnswerPage?> GetCheckAdditionalRequirementsAnswerPage();

    Task<OpenGraphData?> GetOpenGraphData();

    Task<CheckYourAnswersPage?> GetCheckYourAnswersPage();

    Task<PreCheckPage?> GetPreCheckPage();

    Task<Footer?> GetFooter();
    
    Task<FeedbackFormPage?> GetFeedbackFormPage();
    
    Task<FeedbackFormConfirmationPage?> GetFeedbackFormConfirmationPage();

    Task<GetHelpPage?> GetGetHelpPage();

    Task<HelpQualificationDetailsPage?> GetHelpQualificationDetailsPage();

    Task<HelpProvideDetailsPage?> GetHelpProvideDetailsPage();

    Task<HelpEmailAddressPage?> GetHelpEmailAddressPage();

    Task<HelpConfirmationPage?> GetHelpConfirmationPage();
}