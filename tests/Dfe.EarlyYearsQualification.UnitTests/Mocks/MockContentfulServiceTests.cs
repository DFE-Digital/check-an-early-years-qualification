using Contentful.Core.Models;
using Dfe.EarlyYearsQualification.Content.Constants;
using Dfe.EarlyYearsQualification.Content.Entities;
using Dfe.EarlyYearsQualification.Mock.Content;

namespace Dfe.EarlyYearsQualification.UnitTests.Mocks;

[TestClass]
public class MockContentfulServiceTests
{
    [TestMethod]
    public async Task GetAccessibilityStatementPage_ReturnsExpectedDetails()
    {
        var contentfulService = new MockContentfulService();

        var result = await contentfulService.GetAccessibilityStatementPage();
        result.Should().NotBeNull();
        result.Should().BeAssignableTo<AccessibilityStatementPage>();
        result!.Heading.Should().NotBeNullOrEmpty();
    }

    [TestMethod]
    public async Task GetAdvicePage_QualificationsAchievedOutsideTheUk_ReturnsExpectedDetails()
    {
        var contentfulService = new MockContentfulService();

        var result = await contentfulService.GetAdvicePage(AdvicePages.QualificationsAchievedOutsideTheUk);
        result.Should().NotBeNull();
        result.Should().BeAssignableTo<AdvicePage>();
        result!.Heading.Should().NotBeNullOrEmpty();
        result.Body!.Content[0].Should().BeAssignableTo<Paragraph>()
              .Which.Content.Should().ContainSingle(x => ((Text)x).Value == "Test Advice Page Body");
    }

    [TestMethod]
    public async Task GetAdvicePage_Level2SeptAndAug_ReturnsExpectedDetails()
    {
        var contentfulService = new MockContentfulService();

        var result = await contentfulService.GetAdvicePage(AdvicePages.QualificationsStartedBetweenSept2014AndAug2019);
        result.Should().NotBeNull();
        result.Should().BeAssignableTo<AdvicePage>();
        result!.Heading.Should().NotBeNullOrEmpty();
        result.Body!.Content[0].Should().BeAssignableTo<Paragraph>()
              .Which.Content.Should().ContainSingle(x => ((Text)x).Value == "Test Advice Page Body");
    }

    [TestMethod]
    public async Task GetAdvicePage_QualificationsAchievedInScotland_ReturnsExpectedDetails()
    {
        var contentfulService = new MockContentfulService();

        var result = await contentfulService.GetAdvicePage(AdvicePages.QualificationsAchievedInScotland);
        result.Should().NotBeNull();
        result.Should().BeAssignableTo<AdvicePage>();
        result!.Heading.Should().Be("Qualifications achieved in Scotland");
        result.Body!.Content[0].Should().BeAssignableTo<Paragraph>()
              .Which.Content.Should().ContainSingle(x => ((Text)x).Value == "Test Advice Page Body");
    }

    [TestMethod]
    public async Task GetAdvicePage_QualificationsAchievedInWales_ReturnsExpectedDetails()
    {
        var contentfulService = new MockContentfulService();

        var result = await contentfulService.GetAdvicePage(AdvicePages.QualificationsAchievedInWales);
        result.Should().NotBeNull();
        result.Should().BeAssignableTo<AdvicePage>();
        result!.Heading.Should().Be("Qualifications achieved in Wales");
        result.Body!.Content[0].Should().BeAssignableTo<Paragraph>()
              .Which.Content.Should().ContainSingle(x => ((Text)x).Value == "Test Advice Page Body");
    }

    [TestMethod]
    public async Task GetAdvicePage_QualificationsAchievedInNorthernIreland_ReturnsExpectedDetails()
    {
        var contentfulService = new MockContentfulService();

        var result = await contentfulService.GetAdvicePage(AdvicePages.QualificationsAchievedInNorthernIreland);
        result.Should().NotBeNull();
        result.Should().BeAssignableTo<AdvicePage>();
        result!.Heading.Should().Be("Qualifications achieved in Northern Ireland");
        result.Body!.Content[0].Should().BeAssignableTo<Paragraph>()
              .Which.Content.Should().ContainSingle(x => ((Text)x).Value == "Test Advice Page Body");
    }

    [TestMethod]
    public async Task GetAdvicePage_QualificationNotOnTheList_ReturnsExpectedDetails()
    {
        var contentfulService = new MockContentfulService();

        var result = await contentfulService.GetAdvicePage(AdvicePages.QualificationNotOnTheList);
        result.Should().NotBeNull();
        result.Should().BeAssignableTo<AdvicePage>();
        result!.Heading.Should().Be("Qualification not on the list");
        result.Body!.Content[0].Should().BeAssignableTo<Paragraph>()
              .Which.Content.Should().ContainSingle(x => ((Text)x).Value == "Test Advice Page Body");
    }

    [TestMethod]
    public async Task GetAdvicePage_Level7QualificationStartedBetweenSept2014AndAug2019_ReturnsExpectedDetails()
    {
        var contentfulService = new MockContentfulService();

        var result = await contentfulService.GetAdvicePage(AdvicePages.Level7QualificationStartedBetweenSept2014AndAug2019);
        result.Should().NotBeNull();
        result.Should().BeAssignableTo<AdvicePage>();
        result!.Heading.Should().NotBeNullOrEmpty();
        result.Body!.Content[0].Should().BeAssignableTo<Paragraph>()
              .Which.Content.Should().ContainSingle(x => ((Text)x).Value == "Test Advice Page Body");
    }

    [TestMethod]
    public async Task GetAdvicePage_Level7QualificationAfterAug2019_ReturnsExpectedDetails()
    {
        var contentfulService = new MockContentfulService();

        var result = await contentfulService.GetAdvicePage(AdvicePages.Level7QualificationAfterAug2019);
        result.Should().NotBeNull();
        result.Should().BeAssignableTo<AdvicePage>();
        result!.Heading.Should().NotBeNullOrEmpty();
        result.Body!.Content[0].Should().BeAssignableTo<Paragraph>()
              .Which.Content.Should().ContainSingle(x => ((Text)x).Value == "Test Advice Page Body");
    }

    [TestMethod]
    public async Task GetAdvicePage_UnknownEntryId_ReturnsException()
    {
        var contentfulService = new MockContentfulService();

        var page = await contentfulService.GetAdvicePage("Invalid entry Id");

        page.Should().BeNull();
    }

    [TestMethod]
    public async Task GetCookiesPage_ReturnsExpectedDetails()
    {
        var contentfulService = new MockContentfulService();

        var result = await contentfulService.GetCookiesPage();
        result.Should().NotBeNull();
        result.Should().BeAssignableTo<CookiesPage>();
        result!.Heading.Should().NotBeNullOrEmpty();
        result.Body!.Content[0].Should().BeAssignableTo<Paragraph>()
              .Which.Content.Should().ContainSingle(x => ((Text)x).Value == "Test Cookies Page Body");
        result.ButtonText.Should().NotBeNullOrEmpty();
        result.ErrorText.Should().NotBeNullOrEmpty();
        result.SuccessBannerHeading.Should().NotBeNullOrEmpty();
        result.SuccessBannerContent!.Content[0].Should().BeAssignableTo<Paragraph>()
              .Which.Content.Should().ContainSingle(x => ((Text)x).Value == "Test Success Banner Content");
        result.Options.Should().NotBeNullOrEmpty();
        result.Options.Count.Should().Be(2);
    }

    [TestMethod]
    public async Task GetDetailsPage_ReturnsExpectedDetails()
    {
        var contentfulService = new MockContentfulService();

        var result = await contentfulService.GetDetailsPage();
        result.Should().NotBeNull();
        result.Should().BeAssignableTo<DetailsPage>();
        result!.AwardingOrgLabel.Should().NotBeNullOrEmpty();
        result.BookmarkHeading.Should().NotBeNullOrEmpty();
        result.BookmarkText.Should().NotBeNullOrEmpty();
        result.CheckAnotherQualificationHeading.Should().NotBeNullOrEmpty();
        result.CheckAnotherQualificationText!.Content[0].Should().BeAssignableTo<Paragraph>()
              .Which.Content.Should().ContainSingle(x => ((Text)x).Value == "Test Check Another Qualification Text");
        result.DateAddedLabel.Should().NotBeNullOrEmpty();
        result.DateOfCheckLabel.Should().NotBeNullOrEmpty();
        result.FurtherInfoText!.Content[0].Should().BeAssignableTo<Paragraph>()
              .Which.Content.Should().ContainSingle(x => ((Text)x).Value == "Test Further Info Text");
        result.LevelLabel.Should().NotBeNullOrEmpty();
        result.MainHeader.Should().NotBeNullOrEmpty();
        result.QualificationNumberLabel.Should().NotBeNullOrEmpty();
        result.QualificationDetailsSummaryHeader.Should().NotBeNullOrEmpty();
        result.QualificationNameLabel.Should().NotBeNullOrEmpty();
        result.QualificationStartDateLabel.Should().NotBeNullOrEmpty();
        result.QualificationAwardedDateLabel.Should().NotBeNullOrEmpty();
        result.RatiosText!.Content[0].Should().BeAssignableTo<Paragraph>()
              .Which.Content.Should().ContainSingle(x => ((Text)x).Value == "This is the ratio text");
        result.RatiosTextNotFullAndRelevant!.Content[0].Should().BeAssignableTo<Paragraph>()
              .Which.Content.Should().ContainSingle(x => ((Text)x).Value == "This is not F&R");
        result.QualificationResultHeading.Should().Be("Qualification result heading");
        result.QualificationResultFrMessageHeading.Should().Be("Full and relevant");
        result.QualificationResultFrMessageBody.Should().Be("Full and relevant body");
        result.QualificationResultNotFrMessageHeading.Should().Be("Not full and relevant");
        result.QualificationResultNotFrMessageBody.Should().Be("Not full and relevant body");
        result.QualificationResultNotFrL3MessageHeading.Should().Be("Not full and relevant L3");
        result.QualificationResultNotFrL3MessageBody.Should().Be("Not full and relevant L3 body");
    }

    [TestMethod]
    public async Task GetNavigationLinks_ReturnsExpectedDetails()
    {
        var contentfulService = new MockContentfulService();

        var result = await contentfulService.GetNavigationLinks();
        result.Should().NotBeNull();
        result.Should().BeAssignableTo<List<NavigationLink>>();
        result.Count.Should().Be(2);
    }

    [TestMethod]
    public async Task GetRadioQuestionPage_PassInWhereWasTheQualificationAwarded_ReturnsExpectedDetails()
    {
        var contentfulService = new MockContentfulService();

        var result = await contentfulService.GetRadioQuestionPage(QuestionPages.WhereWasTheQualificationAwarded);
        result.Should().NotBeNull();
        result.Should().BeAssignableTo<RadioQuestionPage>();
        result!.Question.Should().NotBeNullOrEmpty();
        result.CtaButtonText.Should().NotBeNullOrEmpty();
        result.ErrorMessage.Should().NotBeNullOrEmpty();
        result.ErrorBannerHeading.Should().NotBeNull();
        result.ErrorBannerLinkText.Should().NotBeNull();
        result.Options.Should().NotBeNullOrEmpty();
        result.Options.Count.Should().Be(6);
        (result.Options[0] as Option)!.Label.Should().Be("England");
        (result.Options[0] as Option)!.Value.Should().Be("england");
        (result.Options[1] as Option)!.Label.Should().Be("Scotland");
        (result.Options[1] as Option)!.Value.Should().Be("scotland");
        (result.Options[2] as Option)!.Label.Should().Be("Wales");
        (result.Options[2] as Option)!.Value.Should().Be("wales");
        (result.Options[3] as Option)!.Label.Should().Be("Northern Ireland");
        (result.Options[3] as Option)!.Value.Should().Be("northern-ireland");
        (result.Options[4] as Divider)!.Text.Should().Be("or");
        (result.Options[5] as Option)!.Label.Should().Be("Outside the United Kingdom");
        (result.Options[5] as Option)!.Value.Should().Be("outside-uk");
    }

    [TestMethod]
    public async Task GetRadioQuestionPage_PassInWhatLevelIsTheQualification_ReturnsExpectedDetails()
    {
        var contentfulService = new MockContentfulService();

        var result = await contentfulService.GetRadioQuestionPage(QuestionPages.WhatLevelIsTheQualification);
        result.Should().NotBeNull();
        result.Should().BeAssignableTo<RadioQuestionPage>();
        result!.Question.Should().NotBeNullOrEmpty();
        result.CtaButtonText.Should().NotBeNullOrEmpty();
        result.ErrorMessage.Should().NotBeNullOrEmpty();
        result.ErrorBannerHeading.Should().NotBeNull();
        result.ErrorBannerLinkText.Should().NotBeNull();
        result.AdditionalInformationHeader.Should().Be("This is the additional information header");
        result.AdditionalInformationBody!.Content[0].Should().BeAssignableTo<Paragraph>()
              .Which.Content.Should().ContainSingle(x => ((Text)x).Value == "This is the additional information body");
        result.Options.Should().NotBeNullOrEmpty();
        result.Options.Count.Should().Be(4);
        (result.Options[0] as Option)!.Label.Should().Be("Level 2");
        (result.Options[0] as Option)!.Value.Should().Be("2");
        (result.Options[1] as Option)!.Label.Should().Be("Level 3");
        (result.Options[1] as Option)!.Value.Should().Be("3");
        (result.Options[2] as Option)!.Label.Should().Be("Level 6");
        (result.Options[2] as Option)!.Value.Should().Be("6");
        (result.Options[3] as Option)!.Label.Should().Be("Level 7");
        (result.Options[3] as Option)!.Value.Should().Be("7");
    }

    [TestMethod]
    public async Task GetRadioQuestionPage_PassInvalidEntryId_ReturnsException()
    {
        var contentfulService = new MockContentfulService();

        Func<Task> act = () => contentfulService.GetRadioQuestionPage("Fake_entry_id");

        await act.Should().ThrowAsync<NotImplementedException>()
                 .WithMessage("No radio question page mock for entry Fake_entry_id");
    }

    [TestMethod]
    public async Task GetDatesQuestionPage_PassWhenWasQualificationStartedId_ReturnsExpectedDetails()
    {
        var expectedStartedQuestion = new DateQuestion
                                      {
                                          MonthLabel = "started- Test Month Label",
                                          YearLabel = "started- Test Year Label",
                                          QuestionHeader = "started- Test Question Hint Header",
                                          QuestionHint = "started- Test Question Hint",
                                          ErrorBannerLinkText = "started- Test error banner link text",
                                          ErrorMessage = "started- Test Error Message",
                                          FutureDateErrorBannerLinkText = "started- Future date error message banner link",
                                          FutureDateErrorMessage = "started- Future date error message",
                                          MissingMonthErrorMessage = "started- Missing Month Error Message",
                                          MissingYearErrorMessage = "started- Missing Year Error Message",
                                          MissingMonthBannerLinkText = "started- Missing Month Banner Link Text",
                                          MissingYearBannerLinkText = "started- Missing Year Banner Link Text",
                                          MonthOutOfBoundsErrorLinkText = "started- Month Out Of Bounds Error Link Text",
                                          MonthOutOfBoundsErrorMessage = "started- Month Out Of Bounds Error Message",
                                          YearOutOfBoundsErrorLinkText = "started- Year Out Of Bounds Error Link Text",
                                          YearOutOfBoundsErrorMessage = "started- Year Out Of Bounds Error Message"
                                      };
        var expectedAwardedQuestion = new DateQuestion
                                      {
                                          MonthLabel = "awarded- Test Month Label",
                                          YearLabel = "awarded- Test Year Label",
                                          QuestionHeader = "awarded- Test Question Hint Header",
                                          QuestionHint = "awarded- Test Question Hint",
                                          ErrorBannerLinkText = "awarded- Test error banner link text",
                                          ErrorMessage = "awarded- Test Error Message",
                                          FutureDateErrorBannerLinkText = "awarded- Future date error message banner link",
                                          FutureDateErrorMessage = "awarded- Future date error message",
                                          MissingMonthErrorMessage = "awarded- Missing Month Error Message",
                                          MissingYearErrorMessage = "awarded- Missing Year Error Message",
                                          MissingMonthBannerLinkText = "awarded- Missing Month Banner Link Text",
                                          MissingYearBannerLinkText = "awarded- Missing Year Banner Link Text",
                                          MonthOutOfBoundsErrorLinkText = "awarded- Month Out Of Bounds Error Link Text",
                                          MonthOutOfBoundsErrorMessage = "awarded- Month Out Of Bounds Error Message",
                                          YearOutOfBoundsErrorLinkText = "awarded- Year Out Of Bounds Error Link Text",
                                          YearOutOfBoundsErrorMessage = "awarded- Year Out Of Bounds Error Message"
                                      };

        var contentfulService = new MockContentfulService();

        var result = await contentfulService.GetDatesQuestionPage(QuestionPages.WhenWasTheQualificationStartedAndAwarded);

        result.Should().NotBeNull();
        result!.Question.Should().Be("Test Dates Questions");
        result!.CtaButtonText.Should().Be("Continue");
        result!.ErrorBannerHeading.Should().Be("There is a problem");
        result!.AwardedDateIsAfterStartedDateErrorText.Should().Be("Error- AwardedDateIsAfterStartedDateErrorText");
        result!.BackButton.Should().BeEquivalentTo(new NavigationLink
                                                   {
                                                       DisplayText = "TEST",
                                                       Href = "/questions/where-was-the-qualification-awarded",
                                                       OpenInNewTab = false
                                                   });
        result.StartedQuestion.Should().BeEquivalentTo(expectedStartedQuestion);
        result.AwardedQuestion.Should().BeEquivalentTo(expectedAwardedQuestion);
    }

    [TestMethod]
    public async Task GetDatesQuestionPage_PassInvalidEntryId_ReturnsException()
    {
        var contentfulService = new MockContentfulService();

        Func<Task> act = () => contentfulService.GetDatesQuestionPage("Fake_entry_id");

        await act.Should().ThrowAsync<NotImplementedException>()
                 .WithMessage("No date question page mock for entry Fake_entry_id");
    }

    [TestMethod]
    public async Task GetDropdownQuestionPage_PassWhenWasQualificationStartedId_ReturnsExpectedDetails()
    {
        var contentfulService = new MockContentfulService();

        var result = await contentfulService.GetDropdownQuestionPage(QuestionPages.WhatIsTheAwardingOrganisation);

        result.Should().NotBeNull();
        result!.CtaButtonText.Should().Be("Test Button Text");
        result.ErrorMessage.Should().Be("Test Error Message");
        result.ErrorBannerHeading.Should().Be("There is a problem");
        result.ErrorBannerLinkText.Should().Be("Test error banner link text");
        result.AdditionalInformationHeader.Should().Be("This is the additional information header");
        result.AdditionalInformationBody!.Content[0].Should().BeAssignableTo<Paragraph>()
              .Which.Content.Should().ContainSingle(x => ((Text)x).Value == "This is the additional information body");
        result.Question.Should().Be("Test Dropdown Question");
        result.DefaultText.Should().Be("Test Default Dropdown Text");
        result.DropdownHeading.Should().Be("Test Dropdown Heading");
        result.NotInListText.Should().Be("Test Not In The List");
    }

    [TestMethod]
    public async Task GetDropdownQuestionPage_PassInvalidEntryId_ReturnsException()
    {
        var contentfulService = new MockContentfulService();

        Func<Task> act = () => contentfulService.GetDropdownQuestionPage("Fake_entry_id");

        await act.Should().ThrowAsync<NotImplementedException>()
                 .WithMessage("No dropdown question page mock for entry Fake_entry_id");
    }

    [TestMethod]
    public async Task GetQualificationListPage_ReturnsCorrectMockData()
    {
        var contentfulService = new MockContentfulService();

        var result = await contentfulService.GetQualificationListPage();

        result.Should().NotBeNull();
        result!.Header.Should().Be("Test Header");
        result.BackButton.Should().BeEquivalentTo(new NavigationLink
                                                  {
                                                      DisplayText = "TEST",
                                                      Href = "/questions/what-is-the-awarding-organisation",
                                                      OpenInNewTab = false
                                                  });
    }

    [TestMethod]
    public async Task GetConfirmQualificationPage_ReturnsCorrectMockData()
    {
        var contentfulService = new MockContentfulService();

        var result = await contentfulService.GetConfirmQualificationPage();

        result.Should().NotBeNull();
        result!.Options.Should().BeEquivalentTo([
                                                    new Option
                                                    {
                                                        Label = "yes",
                                                        Value = "yes"
                                                    },
                                                    new Option
                                                    {
                                                        Label = "no",
                                                        Value = "no"
                                                    }
                                                ]);
        result.BackButton.Should().BeEquivalentTo(new NavigationLink
                                                  {
                                                      DisplayText = "Test back button",
                                                      OpenInNewTab = false,
                                                      Href = "/qualifications"
                                                  });
        result.LevelLabel.Should().Be("Test level label");
        result.ButtonText.Should().Be("Test button text");
        result.ErrorText.Should().Be("Test error text");
        result.Heading.Should().Be("Test heading");
        result.QualificationLabel.Should().Be("Test qualification label");
        result.RadioHeading.Should().Be("Test radio heading");
        result.AwardingOrganisationLabel.Should().Be("Test awarding organisation label");
        result.DateAddedLabel.Should().Be("Test date added label");
        result.ErrorBannerHeading.Should().Be("Test error banner heading");
        result.ErrorBannerLink.Should().Be("Test error banner link");
        result.PostHeadingContent!.Content[0].Should().BeAssignableTo<Paragraph>()
              .Which.Content.Should().ContainSingle(x => ((Text)x).Value == "The post heading content");
        result.VariousAwardingOrganisationsExplanation!.Content[0].Should().BeAssignableTo<Paragraph>()
              .Which.Content.Should()
              .ContainSingle(x => ((Text)x).Value == "Various awarding organisation explanation text");
    }

    [TestMethod]
    public async Task GetCheckAdditionalRequirementsAnswerPage_ReturnsExpectedDetails()
    {
        var contentfulService = new MockContentfulService();

        var result = await contentfulService.GetCheckAdditionalRequirementsAnswerPage();

        result.Should().NotBeNull();
        result.Should().BeAssignableTo<CheckAdditionalRequirementsAnswerPage>();

        result!.BackButton!.Href.Should().Be("/qualifications/check-additional-questions");
        result.BackButton.OpenInNewTab.Should().BeFalse();
        result.BackButton.DisplayText.Should().Be("Test display text");
        result.ButtonText.Should().Be("Test button text");
        result.PageHeading.Should().Be("Test page heading");
        result.AnswerDisclaimerText.Should().Be("Test answer disclaimer text");
        result.ChangeAnswerText.Should().Be("Test change answer text");
    }

    [TestMethod]
    public async Task GetStartPage_ReturnsExpectedDetails()
    {
        var contentfulService = new MockContentfulService();

        var result = await contentfulService.GetStartPage();

        result.Should().NotBeNull();
        result.Should().BeAssignableTo<StartPage>();
        result!.CtaButtonText.Should().NotBeNullOrEmpty();
        result.Header.Should().NotBeNullOrEmpty();

        result.PostCtaButtonContent!.Content[0].Should().BeAssignableTo<Paragraph>()
              .Which.Content.Should().ContainSingle(x => ((Text)x).Value == "This is the post cta content");

        result.PreCtaButtonContent!.Content[0].Should().BeAssignableTo<Paragraph>()
              .Which.Content.Should().ContainSingle(x => ((Text)x).Value == "This is the pre cta content");

        result.RightHandSideContent!.Content[0].Should().BeAssignableTo<Paragraph>()
              .Which.Content.Should().ContainSingle(x => ((Text)x).Value == "This is the right hand content");

        result.RightHandSideContentHeader.Should().NotBeNullOrEmpty();
    }

    [TestMethod]
    public async Task GetPhaseBannerContent_ReturnsExpectedDetails()
    {
        var contentfulService = new MockContentfulService();

        var result = await contentfulService.GetPhaseBannerContent();
        result.Should().NotBeNull();
        result.Should().BeAssignableTo<PhaseBanner>();
        result!.Content.Should().NotBeNull();
        result.PhaseName.Should().NotBeNullOrEmpty();
        result.Show.Should().BeTrue();
    }

    [TestMethod]
    public async Task GetCookiesBannerContent_ReturnsExpectedDetails()
    {
        var contentfulService = new MockContentfulService();

        var result = await contentfulService.GetCookiesBannerContent();
        result.Should().NotBeNull();
        result.Should().BeAssignableTo<CookiesBanner>();
        result!.AcceptButtonText.Should().NotBeNull();
        result.AcceptedCookiesContent.Should().NotBeNull();
        result.CookiesBannerContent.Should().NotBeNull();
        result.CookiesBannerTitle.Should().NotBeNullOrEmpty();
        result.CookiesBannerLinkText.Should().NotBeNullOrEmpty();
        result.HideCookieBannerButtonText.Should().NotBeNullOrEmpty();
        result.RejectButtonText.Should().NotBeNullOrEmpty();
        result.RejectedCookiesContent.Should().NotBeNull();
    }

    [TestMethod]
    public async Task GetCheckAdditionalRequirementsPageContent_ReturnsExpectedDetails()
    {
        var contentfulService = new MockContentfulService();
        var result = await contentfulService.GetCheckAdditionalRequirementsPage();

        result.Should().NotBeNull();
        result.Should().BeAssignableTo<CheckAdditionalRequirementsPage>();
        result!.BackButton.Should().BeEquivalentTo(new NavigationLink
                                                   {
                                                       DisplayText = "Back",
                                                       OpenInNewTab = false,
                                                       Href = "/qualifications"
                                                   });
        result.ErrorMessage.Should().NotBeNullOrEmpty();
        result.ErrorSummaryHeading.Should().NotBeNullOrEmpty();
        result.Heading.Should().NotBeNullOrEmpty();
        result.InformationMessage.Should().NotBeNullOrEmpty();
        result.QualificationLabel.Should().NotBeNullOrEmpty();
        result.AwardingOrganisationLabel.Should().NotBeNullOrEmpty();
        result.CtaButtonText.Should().NotBeNullOrEmpty();
        result.QualificationLevelLabel.Should().NotBeNullOrEmpty();
        result.QuestionSectionHeading.Should().NotBeNullOrEmpty();
    }

    [TestMethod]
    public async Task GetCannotFindQualificationPage_PassInLevel3_ReturnsExpectedContent()
    {
        var contentfulService = new MockContentfulService();
        var result = await contentfulService.GetCannotFindQualificationPage(3, 7, 2015);

        result.Should().NotBeNull();
        result.Should().BeAssignableTo<CannotFindQualificationPage>();
        result!.BackButton.Should().BeEquivalentTo(new NavigationLink
                                                   {
                                                       DisplayText = "TEST",
                                                       OpenInNewTab = false,
                                                       Href = "/qualifications"
                                                   });

        result.Heading.Should().Be("This is the level 3 page");
        result.Body.Should().NotBeNull();
        result.FromWhichYear.Should().Be("Sep-14");
        result.ToWhichYear.Should().Be("Aug-19");
        result.FeedbackBanner.Should().NotBeNull();
    }

    [TestMethod]
    public async Task GetCannotFindQualificationPage_PassInLevel4_ReturnsExpectedContent()
    {
        var contentfulService = new MockContentfulService();
        var result = await contentfulService.GetCannotFindQualificationPage(4, 7, 2015);

        result.Should().NotBeNull();
        result.Should().BeAssignableTo<CannotFindQualificationPage>();
        result!.BackButton.Should().BeEquivalentTo(new NavigationLink
                                                   {
                                                       DisplayText = "TEST",
                                                       OpenInNewTab = false,
                                                       Href = "/qualifications"
                                                   });

        result.Heading.Should().Be("This is the level 4 page");
        result.Body.Should().NotBeNull();
        result.FromWhichYear.Should().Be("Sep-19");
        result.ToWhichYear.Should().BeEmpty();
        result.FeedbackBanner.Should().NotBeNull();
    }

    [TestMethod]
    public async Task GetCannotFindQualificationPage_PassInLevel5_ReturnsNull()
    {
        var contentfulService = new MockContentfulService();
        var result = await contentfulService.GetCannotFindQualificationPage(5, 7, 2015);

        result.Should().BeNull();
    }

    [TestMethod]
    public async Task GetOpenGraphData_ReturnsExpectedDetails()
    {
        var contentfulService = new MockContentfulService();

        var result = await contentfulService.GetOpenGraphData();

        result.Should().NotBeNull();
        result.Should().BeAssignableTo<OpenGraphData>();
        result!.Title.Should().Be("OG Title");
        result.Description.Should().Be("OG Description");
        result.Domain.Should().Be("OG Domain");
        result.Image.Should().NotBeNull();
        result.Image!.File.Should().NotBeNull();
        result.Image.File.Url.Should().Be("test/url/og-image.png");
    }
    
    [TestMethod]
    public async Task GetCheckYourAnswersPage_ReturnsExpectedDetails()
    {
        var contentfulService = new MockContentfulService();

        var result = await contentfulService.GetCheckYourAnswersPage();

        result.Should().NotBeNull();
        result.Should().BeAssignableTo<CheckYourAnswersPage>();
        result!.PageHeading.Should().Be("Check your answers");
        result.BackButton.Should().BeEquivalentTo(new NavigationLink
                                                   {
                                                       DisplayText = "TEST",
                                                       OpenInNewTab = false,
                                                       Href = "/questions/where-was-the-qualification-awarded"
                                                   });
        result.CtaButtonText.Should().Be("Continue");
        result.ChangeAnswerText.Should().Be("Change");
        result.QualificationAwardedText.Should().Be("Awarded in");
        result.QualificationStartedText.Should().Be("Started in");
        result.AnyAwardingOrganisationText.Should().Be("Various awarding organisations");
        result.AnyLevelText.Should().Be("Any level");
    }
}