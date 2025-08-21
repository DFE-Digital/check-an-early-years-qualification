namespace Dfe.EarlyYearsQualification.Web.Models.Content.HelpViewModels;

public class ConfirmationViewModel
{
    public string Heading { get; init; } = "What is your email address?"; // todo

    public string BannerSuccessMessage { get; init; } = "Message sent";

    public string AdditionalSuccessText { get; init; } = "Your message was successfully sent to the Check an early years qualification team.";

    public string BodyHeading { get; init; } = "What happens next";

    // todo this may need to be split into two
    public string Body { get; init; } = "The Check an early years qualification team will reply to your message within 5 working days. Complex cases may take longer. We may need to contact you for more information before we can respond.";

    public FeedbackComponentModel? FeedbackComponent { get; init; } = new()
    {
        Header = "Give feedback",
        Body = "Your feedback matters and will help us improve this service. Fill in our short feedback form (opens in a new tab)."
    };

    public NavigationLinkModel? ReturnToTheHomepageLink { get; init; } = 
        new()
        {
            DisplayText = "Return to the homepage",
            Href = "/"
        };
}