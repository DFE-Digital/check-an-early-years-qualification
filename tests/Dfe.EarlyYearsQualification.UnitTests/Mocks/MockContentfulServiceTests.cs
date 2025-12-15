using Contentful.Core.Models;
using Dfe.EarlyYearsQualification.Content.Constants;
using Dfe.EarlyYearsQualification.Content.Entities;
using Dfe.EarlyYearsQualification.Content.Entities.Help;
using Dfe.EarlyYearsQualification.Mock.Content;
using Dfe.EarlyYearsQualification.Web.Constants;

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
        result.Heading.Should().NotBeNullOrEmpty();
    }

    [TestMethod]
    public async Task GetAdvicePage_QualificationsAchievedOutsideTheUk_ReturnsExpectedDetails()
    {
        var contentfulService = new MockContentfulService();

        var result = await contentfulService.GetAdvicePage(AdvicePages.QualificationsAchievedOutsideTheUk);
        result.Should().NotBeNull();
        result.Should().BeAssignableTo<AdvicePage>();
        result.Heading.Should().NotBeNullOrEmpty();
        result.Body!.Content[0].Should().BeAssignableTo<Paragraph>()
              .Which.Content.Should().ContainSingle(x => ((Text)x).Value == "Test Advice Page Body");
        result.UpDownFeedback.Should().NotBeNull();
        result.RightHandSideContent.Should().NotBeNull();
    }

    [TestMethod]
    public async Task GetAdvicePage_Level2SeptAndAug_ReturnsExpectedDetails()
    {
        var contentfulService = new MockContentfulService();

        var result = await contentfulService.GetAdvicePage(AdvicePages.QualificationsStartedBetweenSept2014AndAug2019);
        result.Should().NotBeNull();
        result.Should().BeAssignableTo<AdvicePage>();
        result.Heading.Should().NotBeNullOrEmpty();
        result.Body!.Content[0].Should().BeAssignableTo<Paragraph>()
              .Which.Content.Should().ContainSingle(x => ((Text)x).Value == "Test Advice Page Body");
        result.UpDownFeedback.Should().BeNull();
        result.RightHandSideContent.Should().NotBeNull();
    }

    [TestMethod]
    public async Task GetAdvicePage_QualificationsAchievedInScotland_ReturnsExpectedDetails()
    {
        var contentfulService = new MockContentfulService();

        var result = await contentfulService.GetAdvicePage(AdvicePages.QualificationsAchievedInScotland);
        result.Should().NotBeNull();
        result.Should().BeAssignableTo<AdvicePage>();
        result.Heading.Should().Be("Qualifications achieved in Scotland");
        result.Body!.Content[0].Should().BeAssignableTo<Paragraph>()
              .Which.Content.Should().ContainSingle(x => ((Text)x).Value == "Test Advice Page Body");
        result.UpDownFeedback.Should().NotBeNull();
        result.RightHandSideContent.Should().NotBeNull();
    }

    [TestMethod]
    public async Task GetAdvicePage_QualificationsAchievedInWales_ReturnsExpectedDetails()
    {
        var contentfulService = new MockContentfulService();

        var result = await contentfulService.GetAdvicePage(AdvicePages.QualificationsAchievedInWales);
        result.Should().NotBeNull();
        result.Should().BeAssignableTo<AdvicePage>();
        result.Heading.Should().Be("Qualifications achieved in Wales");
        result.Body!.Content[0].Should().BeAssignableTo<Paragraph>()
              .Which.Content.Should().ContainSingle(x => ((Text)x).Value == "Test Advice Page Body");
        result.UpDownFeedback.Should().NotBeNull();
        result.RightHandSideContent.Should().NotBeNull();
    }

    [TestMethod]
    public async Task GetAdvicePage_QualificationsAchievedInNorthernIreland_ReturnsExpectedDetails()
    {
        var contentfulService = new MockContentfulService();

        var result = await contentfulService.GetAdvicePage(AdvicePages.QualificationsAchievedInNorthernIreland);
        result.Should().NotBeNull();
        result.Should().BeAssignableTo<AdvicePage>();
        result.Heading.Should().Be("Qualifications achieved in Northern Ireland");
        result.Body!.Content[0].Should().BeAssignableTo<Paragraph>()
              .Which.Content.Should().ContainSingle(x => ((Text)x).Value == "Test Advice Page Body");
        result.UpDownFeedback.Should().NotBeNull();
        result.RightHandSideContent.Should().NotBeNull();
    }

    [TestMethod]
    public async Task GetAdvicePage_QualificationNotOnTheList_ReturnsExpectedDetails()
    {
        var contentfulService = new MockContentfulService();

        var result = await contentfulService.GetAdvicePage(AdvicePages.QualificationNotOnTheList);
        result.Should().NotBeNull();
        result.Should().BeAssignableTo<AdvicePage>();
        result.Heading.Should().Be("Qualification not on the list");
        result.Body!.Content[0].Should().BeAssignableTo<Paragraph>()
              .Which.Content.Should().ContainSingle(x => ((Text)x).Value == "Test Advice Page Body");
        result.UpDownFeedback.Should().NotBeNull();
        result.RightHandSideContent.Should().NotBeNull();
    }

    [TestMethod]
    public async Task GetAdvicePage_Level7QualificationStartedBetweenSept2014AndAug2019_ReturnsExpectedDetails()
    {
        var contentfulService = new MockContentfulService();

        var result =
            await contentfulService.GetAdvicePage(AdvicePages.Level7QualificationStartedBetweenSept2014AndAug2019);
        result.Should().NotBeNull();
        result.Should().BeAssignableTo<AdvicePage>();
        result.Heading.Should().NotBeNullOrEmpty();
        result.Body!.Content[0].Should().BeAssignableTo<Paragraph>()
              .Which.Content.Should().ContainSingle(x => ((Text)x).Value == "Test Advice Page Body");
        result.UpDownFeedback.Should().BeNull();
        result.RightHandSideContent.Should().NotBeNull();
    }

    [TestMethod]
    public async Task GetAdvicePage_Level7QualificationAfterAug2019_ReturnsExpectedDetails()
    {
        var contentfulService = new MockContentfulService();

        var result = await contentfulService.GetAdvicePage(AdvicePages.Level7QualificationAfterAug2019);
        result.Should().NotBeNull();
        result.Should().BeAssignableTo<AdvicePage>();
        result.Heading.Should().NotBeNullOrEmpty();
        result.Body!.Content[0].Should().BeAssignableTo<Paragraph>()
              .Which.Content.Should().ContainSingle(x => ((Text)x).Value == "Test Advice Page Body");
        result.UpDownFeedback.Should().BeNull();
        result.RightHandSideContent.Should().NotBeNull();
    }

    [TestMethod]
    public async Task GetAdvicePage_Help_ReturnsExpectedDetails()
    {
        var contentfulService = new MockContentfulService();

        var result = await contentfulService.GetAdvicePage(AdvicePages.Help);
        result.Should().NotBeNull();
        result.Should().BeAssignableTo<AdvicePage>();
        result.Heading.Should().NotBeNullOrEmpty();
        result.Heading.Should().Be("Help");
        result.Body!.Content[0].Should().BeAssignableTo<Paragraph>()
              .Which.Content.Should().ContainSingle(x => ((Text)x).Value == "Test Advice Page Body");
        result.UpDownFeedback.Should().BeNull();
        result.RightHandSideContent.Should().NotBeNull();
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
        result.Heading.Should().NotBeNullOrEmpty();
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

        var result = await contentfulService.GetQualificationDetailsPage(false, false, 3, 6, 2001, false, false);
        result.Should().NotBeNull();
        result.Should().BeAssignableTo<QualificationDetailsPage>();
        result.Labels.AwardingOrgLabel.Should().NotBeNullOrEmpty();
        result.Labels.DateOfCheckLabel.Should().NotBeNullOrEmpty();
        result.Labels.LevelLabel.Should().NotBeNullOrEmpty();
        result.Labels.MainHeader.Should().NotBeNullOrEmpty();
        result.Labels.QualificationDetailsSummaryHeader.Should().NotBeNullOrEmpty();
        result.Labels.QualificationNameLabel.Should().NotBeNullOrEmpty();
        result.Labels.QualificationStartDateLabel.Should().NotBeNullOrEmpty();
        result.Labels.QualificationAwardedDateLabel.Should().NotBeNullOrEmpty();
        result.Labels.RatiosTextNotFullAndRelevant!.Content[0].Should().BeAssignableTo<Paragraph>()
              .Which.Content.Should().ContainSingle(x => ((Text)x).Value == "This is not F&R");
        result.Labels.RatiosTextL3Ebr!.Content[0].Should().BeAssignableTo<Paragraph>()
              .Which.Content.Should().ContainSingle(x => ((Text)x).Value == "This is the ratio text L3 EBR");
        result.Labels.QualificationResultHeading.Should().Be("Qualification result heading");
        result.Labels.QualificationResultFrMessageHeading.Should().Be("Full and relevant");
        result.Labels.QualificationResultFrMessageBody.Should().Be("Full and relevant body");
        result.Labels.QualificationResultNotFrMessageHeading.Should().Be("Not full and relevant");
        result.Labels.QualificationResultNotFrMessageBody.Should().Be("Not full and relevant body");
        result.Labels.QualificationResultNotFrL3MessageHeading.Should().Be("Not full and relevant L3");
        result.Labels.QualificationResultNotFrL3MessageBody.Should().Be("Not full and relevant L3 body");
        result.Labels.QualificationResultNotFrL3OrL6MessageHeading.Should().Be("Not full and relevant L3 or L6");
        result.Labels.QualificationResultNotFrL3OrL6MessageBody.Should().Be("Not full and relevant L3 or L6 body");
        result.Labels.QualificationNumberLabel.Should().Be("Qualification Number (QN)");
        result.IsDegreeSpecificPage.Should().BeFalse();
        result.IsAutomaticallyApprovedAtLevel6.Should().BeFalse();
    }

    [TestMethod]
    public async Task GetDetailsPage_UserIsCheckingOwnQualification_ReturnsExpectedDetails()
    {
        var contentfulService = new MockContentfulService();

        var result = await contentfulService.GetQualificationDetailsPage(true, false, 3, 1, 2024, false, false);
        result.Should().NotBeNull();
        result.Should().BeAssignableTo<QualificationDetailsPage>();
        result.Labels.AwardingOrgLabel.Should().NotBeNullOrEmpty();
        result.Labels.DateOfCheckLabel.Should().NotBeNullOrEmpty();
        result.Labels.LevelLabel.Should().NotBeNullOrEmpty();
        result.Labels.MainHeader.Should().NotBeNullOrEmpty();
        result.Labels.QualificationDetailsSummaryHeader.Should().NotBeNullOrEmpty();
        result.Labels.QualificationNameLabel.Should().NotBeNullOrEmpty();
        result.Labels.QualificationStartDateLabel.Should().NotBeNullOrEmpty();
        result.Labels.QualificationAwardedDateLabel.Should().NotBeNullOrEmpty();
        result.Labels.RatiosTextNotFullAndRelevant!.Content[0].Should().BeAssignableTo<Paragraph>()
              .Which.Content.Should().ContainSingle(x => ((Text)x).Value == "This is not F&R");
        result.Labels.RatiosTextL3Ebr!.Content[0].Should().BeAssignableTo<Paragraph>()
              .Which.Content.Should().ContainSingle(x => ((Text)x).Value == "This is the ratio text L3 EBR");
        result.Labels.QualificationResultHeading.Should().Be("Qualification result heading");
        result.Labels.QualificationResultFrMessageHeading.Should().Be("Full and relevant");
        result.Labels.QualificationResultFrMessageBody.Should().Be("Full and relevant body");
        result.Labels.QualificationResultNotFrMessageHeading.Should().Be("Not full and relevant");
        result.Labels.QualificationResultNotFrMessageBody.Should().Be("Not full and relevant body");
        result.Labels.QualificationResultNotFrL3MessageHeading.Should().Be("Not full and relevant L3");
        result.Labels.QualificationResultNotFrL3MessageBody.Should().Be("Not full and relevant L3 body");
        result.Labels.QualificationResultNotFrL3OrL6MessageHeading.Should().Be("Not full and relevant L3 or L6");
        result.Labels.QualificationResultNotFrL3OrL6MessageBody.Should().Be("Not full and relevant L3 or L6 body");
        result.Labels.QualificationNumberLabel.Should().Be("Qualification Number (QN)");
        result.IsDegreeSpecificPage.Should().BeFalse();
        result.IsAutomaticallyApprovedAtLevel6.Should().BeFalse();
    }

    [TestMethod]
    public async Task GetRadioQuestionPage_PassInWhereWasTheQualificationAwarded_ReturnsExpectedDetails()
    {
        var contentfulService = new MockContentfulService();

        var result = await contentfulService.GetRadioQuestionPage(QuestionPages.WhereWasTheQualificationAwarded);
        result.Should().NotBeNull();
        result.Should().BeAssignableTo<RadioQuestionPage>();
        result.Question.Should().NotBeNullOrEmpty();
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
    public async Task GetRadioQuestionPage_PassInAreYouCheckingYourOwnQualification_ReturnsExpectedDetails()
    {
        var contentfulService = new MockContentfulService();

        var result = await contentfulService.GetRadioQuestionPage(QuestionPages.AreYouCheckingYourOwnQualification);
        result.Should().NotBeNull();
        result.Should().BeAssignableTo<RadioQuestionPage>();
        result.Question.Should().NotBeNullOrEmpty();
        result.CtaButtonText.Should().NotBeNullOrEmpty();
        result.ErrorMessage.Should().NotBeNullOrEmpty();
        result.ErrorBannerHeading.Should().NotBeNull();
        result.ErrorBannerLinkText.Should().NotBeNull();
        result.Options.Should().NotBeNullOrEmpty();
        result.Options.Count.Should().Be(2);
        (result.Options[0] as Option)!.Label.Should().Be("Yes, I am checking my own qualification");
        (result.Options[0] as Option)!.Value.Should().Be("yes");
        (result.Options[1] as Option)!.Label.Should().Be("No, I am checking someone else's qualification");
        (result.Options[1] as Option)!.Value.Should().Be("no");
    }

    [TestMethod]
    public async Task GetRadioQuestionPage_PassInWhatLevelIsTheQualification_ReturnsExpectedDetails()
    {
        var contentfulService = new MockContentfulService();

        var result = await contentfulService.GetRadioQuestionPage(QuestionPages.WhatLevelIsTheQualification);
        result.Should().NotBeNull();
        result.Should().BeAssignableTo<RadioQuestionPage>();
        result.Question.Should().NotBeNullOrEmpty();
        result.CtaButtonText.Should().NotBeNullOrEmpty();
        result.ErrorMessage.Should().NotBeNullOrEmpty();
        result.ErrorBannerHeading.Should().NotBeNull();
        result.ErrorBannerLinkText.Should().NotBeNull();
        result.AdditionalInformationHeader.Should().Be("This is the additional information header");
        result.AdditionalInformationBody!.Content[0].Should().BeAssignableTo<Paragraph>()
              .Which.Content.Should().ContainSingle(x => ((Text)x).Value == "This is the additional information body");
        result.Options.Should().NotBeNullOrEmpty();
        result.Options.Count.Should().Be(7);
        (result.Options[0] as Option)!.Label.Should().Be("Level 2");
        (result.Options[0] as Option)!.Value.Should().Be("2");
        (result.Options[1] as Option)!.Label.Should().Be("Level 3");
        (result.Options[1] as Option)!.Value.Should().Be("3");
        (result.Options[2] as Option)!.Label.Should().Be("Level 4");
        (result.Options[2] as Option)!.Value.Should().Be("4");
        (result.Options[3] as Option)!.Label.Should().Be("Level 5");
        (result.Options[3] as Option)!.Value.Should().Be("5");
        (result.Options[4] as Option)!.Label.Should().Be("Level 6");
        (result.Options[4] as Option)!.Value.Should().Be("6");
        (result.Options[5] as Option)!.Label.Should().Be("Level 7");
        (result.Options[5] as Option)!.Value.Should().Be("7");
        (result.Options[6] as Option)!.Label.Should().Be("Not Sure");
        (result.Options[6] as Option)!.Value.Should().Be("0");
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
                                          FutureDateErrorBannerLinkText =
                                              "started- Future date error message banner link",
                                          FutureDateErrorMessage = "started- Future date error message",
                                          MissingMonthErrorMessage = "started- Missing Month Error Message",
                                          MissingYearErrorMessage = "started- Missing Year Error Message",
                                          MissingMonthBannerLinkText = "started- Missing Month Banner Link Text",
                                          MissingYearBannerLinkText = "started- Missing Year Banner Link Text",
                                          MonthOutOfBoundsErrorLinkText =
                                              "started- Month Out Of Bounds Error Link Text",
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
                                          FutureDateErrorBannerLinkText =
                                              "awarded- Future date error message banner link",
                                          FutureDateErrorMessage = "awarded- Future date error message",
                                          MissingMonthErrorMessage = "awarded- Missing Month Error Message",
                                          MissingYearErrorMessage = "awarded- Missing Year Error Message",
                                          MissingMonthBannerLinkText = "awarded- Missing Month Banner Link Text",
                                          MissingYearBannerLinkText = "awarded- Missing Year Banner Link Text",
                                          MonthOutOfBoundsErrorLinkText =
                                              "awarded- Month Out Of Bounds Error Link Text",
                                          MonthOutOfBoundsErrorMessage = "awarded- Month Out Of Bounds Error Message",
                                          YearOutOfBoundsErrorLinkText = "awarded- Year Out Of Bounds Error Link Text",
                                          YearOutOfBoundsErrorMessage = "awarded- Year Out Of Bounds Error Message"
                                      };

        var contentfulService = new MockContentfulService();

        var result =
            await contentfulService.GetDatesQuestionPage(QuestionPages.WhenWasTheQualificationStartedAndAwarded);

        result.Should().NotBeNull();
        result.Question.Should().Be("Test Dates Questions");
        result.CtaButtonText.Should().Be("Continue");
        result.ErrorBannerHeading.Should().Be("There is a problem");
        result.AwardedDateIsAfterStartedDateErrorText.Should().Be("Error- AwardedDateIsAfterStartedDateErrorText");
        result.BackButton.Should().BeEquivalentTo(new NavigationLink
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
        result.CtaButtonText.Should().Be("Test Button Text");
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
        result.Header.Should().Be("Test Header");
        result.BackButton.Should().BeEquivalentTo(new NavigationLink
                                                  {
                                                      DisplayText = "TEST",
                                                      Href = "/questions/check-your-answers",
                                                      OpenInNewTab = false
                                                  });
        result.QualificationFoundPrefix.Should().Be("We found");
        result.SearchButtonText.Should().Be("Refine");
        result.SearchCriteriaHeading.Should().Be("Your search");
        result.MultipleQualificationsFoundText.Should().Be("matching qualifications");
        result.SingleQualificationFoundText.Should().Be("matching qualification");
        result.PreSearchBoxContent!.Content[0].Should().BeAssignableTo<Paragraph>()
              .Which.Content.Should().ContainSingle(x => ((Text)x).Value == "Pre search box content");
        result.Pre2014L6OrNotSureContentHeading.Should().Be("Pre 2014 L6 or not sure heading");
        result.Pre2014L6OrNotSureContent!.Content[0].Should().BeAssignableTo<Paragraph>()
              .Which.Content.Should().ContainSingle(x => ((Text)x).Value == "Pre 2014 L6 or not sure content");
        result.Post2014L6OrNotSureContentHeading.Should().Be("Post 2014 L6 or not sure heading");
        result.Post2014L6OrNotSureContent!.Content[0].Should().BeAssignableTo<Paragraph>()
              .Which.Content.Should().ContainSingle(x => ((Text)x).Value == "Post 2014 L6 or not sure content");
        result.PostQualificationListContentHeading.Should().Be("Post qualification list header");
        result.PostQualificationListContent!.Content[0].Should().BeAssignableTo<Hyperlink>()
              .Which.Content.Should().Contain(x => ((Text)x).Value == "Link to not on list advice page");
        result.AnyLevelHeading.Should().Be("any level");
        result.AnyAwardingOrganisationHeading.Should().Be("various awarding organisations");
        result.NoResultsText!.Content[0].Should().BeAssignableTo<Paragraph>()
              .Which.Content.Should().ContainSingle(x => ((Text)x).Value == "Test no qualifications text");
        result.ClearSearchText.Should().Be("Clear search");
        result.AwardedLocationPrefixText.Should().Be("awarded in");
        result.StartDatePrefixText.Should().Be("started in");
        result.AwardedDatePrefixText.Should().Be("awarded in");
        result.LevelPrefixText.Should().Be("level");
        result.AwardedByPrefixText.Should().Be("awarded by");
        result.QualificationNumberLabel.Should().Be("Qualification Number (QN)");
    }

    [TestMethod]
    public async Task GetConfirmQualificationPage_ReturnsCorrectMockData()
    {
        var contentfulService = new MockContentfulService();

        var result = await contentfulService.GetConfirmQualificationPage();

        result.Should().NotBeNull();
        result.Options.Should().BeEquivalentTo([
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
                                                      Href = "/select-a-qualification-to-check"
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
        result.QualificationNumberLabel.Should().Be("Qualification Number (QN)");
    }

    [TestMethod]
    public async Task GetCheckAdditionalRequirementsAnswerPage_ReturnsExpectedDetails()
    {
        var contentfulService = new MockContentfulService();

        var result = await contentfulService.GetCheckAdditionalRequirementsAnswerPage();

        result.Should().NotBeNull();
        result.Should().BeAssignableTo<CheckAdditionalRequirementsAnswerPage>();

        result.BackButton!.Href.Should().Be("/qualifications/check-additional-questions");
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
        result.CtaButtonText.Should().NotBeNullOrEmpty();
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
        result.Content.Should().NotBeNull();
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
        result.AcceptButtonText.Should().NotBeNull();
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
        result.BackButton.Should().BeEquivalentTo(new NavigationLink
                                                  {
                                                      DisplayText = "Back",
                                                      OpenInNewTab = false,
                                                      Href = "/select-a-qualification-to-check"
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
    public async Task GetCannotFindQualificationPage_PassInLevel3AndPractitioner_ReturnsExpectedContent()
    {
        var contentfulService = new MockContentfulService();
        var result = await contentfulService.GetCannotFindQualificationPage(3, 7, 2015, true);

        result.Should().NotBeNull();
        result.Should().BeAssignableTo<CannotFindQualificationPage>();
        result.BackButton.Should().BeEquivalentTo(new NavigationLink
                                                  {
                                                      DisplayText = "TEST",
                                                      OpenInNewTab = false,
                                                      Href = "/select-a-qualification-to-check"
                                                  });

        result.Heading.Should().Be("This is the practitioner level 3 page");
        result.Body!.Content[0].Should().BeAssignableTo<Paragraph>()
              .Which.Content.Should().ContainSingle(x => ((Text)x).Value == "This is the practitioner body text");
        result.FromWhichYear.Should().Be("Sep-14");
        result.ToWhichYear.Should().Be("Aug-19");
        result.IsPractitionerSpecificPage.Should().BeTrue();
    }
    
    [TestMethod]
    public async Task GetCannotFindQualificationPage_PassInLevel3_ReturnsExpectedContent()
    {
        var contentfulService = new MockContentfulService();
        var result = await contentfulService.GetCannotFindQualificationPage(3, 7, 2015, false);

        result.Should().NotBeNull();
        result.Should().BeAssignableTo<CannotFindQualificationPage>();
        result.BackButton.Should().BeEquivalentTo(new NavigationLink
                                                  {
                                                      DisplayText = "TEST",
                                                      OpenInNewTab = false,
                                                      Href = "/select-a-qualification-to-check"
                                                  });

        result.Heading.Should().Be("This is the level 3 page");
        result.Body.Should().NotBeNull();
        result.FromWhichYear.Should().Be("Sep-14");
        result.ToWhichYear.Should().Be("Aug-19");
    }

    [TestMethod]
    public async Task GetCannotFindQualificationPage_PassInLevel4_ReturnsExpectedContent()
    {
        var contentfulService = new MockContentfulService();
        var result = await contentfulService.GetCannotFindQualificationPage(4, 7, 2015, false);

        result.Should().NotBeNull();
        result.Should().BeAssignableTo<CannotFindQualificationPage>();
        result.BackButton.Should().BeEquivalentTo(new NavigationLink
                                                  {
                                                      DisplayText = "TEST",
                                                      OpenInNewTab = false,
                                                      Href = "/select-a-qualification-to-check"
                                                  });

        result.Heading.Should().Be("This is the level 4 page");
        result.Body.Should().NotBeNull();
        result.FromWhichYear.Should().Be("Sep-19");
        result.ToWhichYear.Should().BeEmpty();
    }

    [TestMethod]
    public async Task GetCannotFindQualificationPage_PassInLevel5_ReturnsNull()
    {
        var contentfulService = new MockContentfulService();
        var result = await contentfulService.GetCannotFindQualificationPage(5, 7, 2015, false);

        result.Should().BeNull();
    }

    [TestMethod]
    public async Task GetOpenGraphData_ReturnsExpectedDetails()
    {
        var contentfulService = new MockContentfulService();

        var result = await contentfulService.GetOpenGraphData();

        result.Should().NotBeNull();
        result.Should().BeAssignableTo<OpenGraphData>();
        result.Title.Should().Be("OG Title");
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
        result.PageHeading.Should().Be("Check your answers");
        result.BackButton.Should().BeEquivalentTo(new NavigationLink
                                                  {
                                                      DisplayText = "TEST",
                                                      OpenInNewTab = false,
                                                      Href = "/questions/what-is-the-awarding-organisation"
                                                  });
        result.CtaButtonText.Should().Be("Continue");
        result.ChangeAnswerText.Should().Be("Change");
        result.QualificationAwardedText.Should().Be("Awarded in");
        result.QualificationStartedText.Should().Be("Started in");
        result.AnyAwardingOrganisationText.Should().Be("Various awarding organisations");
        result.AnyLevelText.Should().Be("Any level");
    }


    [TestMethod]
    public async Task GetGetHelpPage_ReturnsExpectedDetails()
    {
        var contentfulService = new MockContentfulService();

        var result = await contentfulService.GetGetHelpPage();

        var enquiryReasons = new List<Option>
                             {
                                 new() { Label = "I have a question about a qualification", Value = nameof(HelpFormEnquiryReasons.QuestionAboutAQualification) },
                                 new() { Label = "I am experiencing an issue with the service", Value = nameof(HelpFormEnquiryReasons.IssueWithTheService) },
                             };

        result.Should().NotBeNull();
        result.Should().BeAssignableTo<GetHelpPage>();
        result.Heading.Should().Be("Get help with the Check an early years qualification service");
        result.PostHeadingContent.Content[0].Should().BeAssignableTo<Paragraph>()
              .Which.Content.Should().ContainSingle(x => ((Text)x).Value == "Use this form to ask a question about a qualification or report a problem with the service or the information it provides.\r\nWe aim to respond to all queries within 5 working days. Complex cases may take longer.\r\n");

        result.ReasonForEnquiryHeading.Should().Be("Why are you contacting us?");

        result.EnquiryReasons.Should().NotBeNull();
        result.EnquiryReasons.Count.Should().Be(2);
        result.EnquiryReasons.First().Label.Should().Be(enquiryReasons.First().Label);
        result.EnquiryReasons.Last().Value.Should().Be(enquiryReasons.Last().Value);
        result.EnquiryReasons.First().Label.Should().Be(enquiryReasons.First().Label);
        result.EnquiryReasons.Last().Value.Should().Be(enquiryReasons.Last().Value);

        result.CtaButtonText.Should().Be("Continue");
        result.BackButton.Should().BeEquivalentTo(new NavigationLink
        {
            DisplayText = "Home",
            OpenInNewTab = false,
            Href = "/"
        });
        result.ErrorBannerHeading.Should().Be("There is a problem");
        result.NoEnquiryOptionSelectedErrorMessage.Should().Be("Select one option");
    }

    [TestMethod]
    public async Task GetHelpQualificationDetailsPage_ReturnsExpectedDetails()
    {
        var contentfulService = new MockContentfulService();

        var result = await contentfulService.GetHelpQualificationDetailsPage();

        result.Should().NotBeNull();
        result.Should().BeAssignableTo<HelpQualificationDetailsPage>();

        result.Heading.Should().Be("What are the qualification details?");
        result.PostHeadingContent.Should().Be("We need to know the following qualification details to quickly and accurately respond to any questions you may have.");
        result.CtaButtonText.Should().Be("Continue");
        result.BackButton.Should().BeEquivalentTo(
            new NavigationLink
            {
                DisplayText = "Back to get help with the Check an early years qualification service",
                Href = "/help/get-help",
                OpenInNewTab = false
            }
        );

        result.QualificationNameHeading.Should().Be("Qualification name");
        result.QualificationNameErrorMessage.Should().Be("Enter the qualification name");
        result.AwardingOrganisationHeading.Should().Be("Awarding organisation");
        result.AwardingOrganisationErrorMessage.Should().Be("Enter the awarding organisation");
        result.ErrorBannerHeading.Should().Be("There is a problem");
        result.AwardedDateIsAfterStartedDateErrorText.Should().Be("The awarded date must be after the started date");

        result.StartDateQuestion.MonthLabel.Should().Be("Month");
        result.StartDateQuestion.YearLabel.Should().Be("Year");
        result.StartDateQuestion.QuestionHeader.Should().Be("Start date (optional)");
        result.StartDateQuestion.QuestionHint.Should().Be("Enter the start date so we can check if the qualification is approved as full and relevant. For example 9 2013.");
        result.StartDateQuestion.ErrorBannerLinkText.Should().Be("Enter the month and year that the qualification was started");
        result.StartDateQuestion.ErrorMessage.Should().Be("Enter the month and year that the qualification was started");
        result.StartDateQuestion.FutureDateErrorBannerLinkText.Should().Be("The date the qualification was started must be in the past");
        result.StartDateQuestion.FutureDateErrorMessage.Should().Be("The date the qualification was started must be in the past");
        result.StartDateQuestion.MissingMonthErrorMessage.Should().Be("Enter the month that the qualification was started");
        result.StartDateQuestion.MissingYearErrorMessage.Should().Be("Enter the year that the qualification was started");
        result.StartDateQuestion.MissingMonthBannerLinkText.Should().Be("Enter the month that the qualification was started");
        result.StartDateQuestion.MissingYearBannerLinkText.Should().Be("Enter the year that the qualification was started");
        result.StartDateQuestion.MonthOutOfBoundsErrorLinkText.Should().Be("The month the qualification was started must be between 1 and 12");
        result.StartDateQuestion.MonthOutOfBoundsErrorMessage.Should().Be("The month the qualification was started must be between 1 and 12");
        result.StartDateQuestion.YearOutOfBoundsErrorLinkText.Should().Be("The year the qualification was started must be between 1900 and $[actual-year]$");
        result.StartDateQuestion.YearOutOfBoundsErrorMessage.Should().Be("The year the qualification was started must be between 1900 and $[actual-year]$");

        result.AwardedDateQuestion.MonthLabel.Should().Be("Month");
        result.AwardedDateQuestion.YearLabel.Should().Be("Year");
        result.AwardedDateQuestion.QuestionHeader.Should().Be("Award date");
        result.AwardedDateQuestion.QuestionHint.Should().Be("Enter the date the qualification was awarded so we can tell you if other requirements apply. For example 6 2015.");
        result.AwardedDateQuestion.ErrorBannerLinkText.Should().Be("Enter the month and year that the qualification was awarded");
        result.AwardedDateQuestion.ErrorMessage.Should().Be("Enter the date the qualification was awarded");
        result.AwardedDateQuestion.FutureDateErrorBannerLinkText.Should().Be("The date the qualification was awarded must be in the past");
        result.AwardedDateQuestion.FutureDateErrorMessage.Should().Be("The date the qualification was awarded must be in the past");
        result.AwardedDateQuestion.MissingMonthErrorMessage.Should().Be("Enter the month that the qualification was awarded");
        result.AwardedDateQuestion.MissingYearErrorMessage.Should().Be("Enter the year that the qualification was awarded");
        result.AwardedDateQuestion.MissingMonthBannerLinkText.Should().Be("Enter the month that the qualification was awarded");
        result.AwardedDateQuestion.MissingYearBannerLinkText.Should().Be("Enter the year that the qualification was awarded");
        result.AwardedDateQuestion.MonthOutOfBoundsErrorLinkText.Should().Be("The month the qualification was awarded must be between 1 and 12");
        result.AwardedDateQuestion.MonthOutOfBoundsErrorMessage.Should().Be("The month the qualification was awarded must be between 1 and 12");
        result.AwardedDateQuestion.YearOutOfBoundsErrorLinkText.Should().Be("The year the qualification was awarded must be between 1900 and $[actual-year]$");
        result.AwardedDateQuestion.YearOutOfBoundsErrorMessage.Should().Be("The year the qualification was awarded must be between 1900 and $[actual-year]$");
    }

    [TestMethod]
    public async Task GetHelpProvideDetailsPage_ReturnsExpectedDetails()
    {
        var contentfulService = new MockContentfulService();

        var result = await contentfulService.GetHelpProvideDetailsPage();

        result.Should().NotBeNull();
        result.Should().BeAssignableTo<HelpProvideDetailsPage>();

        result.Heading.Should().Be("How can we help you?");
        result.PostHeadingContent.Should().Be("Give as much detail as you can. This helps us give you the right support.");
        result.CtaButtonText.Should().Be("Continue");
        result.BackButtonToGetHelpPage.Should().BeEquivalentTo(
            new NavigationLink
            {
                DisplayText = "Back to get help with the Check an early years qualification service",
                Href = "/help/get-help",
                OpenInNewTab = false
            }
        );
        result.BackButtonToQualificationDetailsPage.Should().BeEquivalentTo(
            new NavigationLink
            {
                DisplayText = "Back to what are the qualification details",
                Href = "/help/qualification-details",
                OpenInNewTab = false
            }
        );
        result.AdditionalInformationWarningText.Should().Be("Do not include any personal information");
        result.AdditionalInformationErrorMessage.Should().Be("Provide information about how we can help you");
        result.ErrorBannerHeading.Should().Be("There is a problem");
    }

    [TestMethod]
    public async Task GetHelpEmailAddressPage_ReturnsExpectedDetails()
    {
        var contentfulService = new MockContentfulService();

        var result = await contentfulService.GetHelpEmailAddressPage();

        result.Should().NotBeNull();
        result.Should().BeAssignableTo<HelpEmailAddressPage>();

        result.BackButton.Should().BeEquivalentTo(
            new NavigationLink
            {
                DisplayText = "Back to how can we help you",
                Href = "/help/provide-details",
                OpenInNewTab = false
            }
        );
        result.Heading.Should().Be("What is your email address?");
        result.PostHeadingContent.Should().Be("We will only use this email address to reply to your message");
        result.CtaButtonText.Should().Be("Send message");
        result.ErrorBannerHeading.Should().Be("There is a problem");
        result.NoEmailAddressEnteredErrorMessage.Should().Be("Enter an email address");
        result.InvalidEmailAddressErrorMessage.Should().Be("Enter an email address in the correct format, for example name@example.com");
    }

    [TestMethod]
    public async Task GetHelpConfirmationPage_ReturnsExpectedDetails()
    {
        var contentfulService = new MockContentfulService();

        var result = await contentfulService.GetHelpConfirmationPage();
        result.Should().NotBeNull();
        result.Should().BeAssignableTo<HelpConfirmationPage>();
        result.SuccessMessage.Should().Be("Message sent");
        result.BodyHeading.Should().Be("What happens next");
        result.Body.Should().NotBeNull();
        result.Body.Content[0].Should().BeAssignableTo<Paragraph>()
              .Which.Content.Should().ContainSingle(x => ((Text)x).Value == "The Check an early years qualification team will reply to your message within 5 working days. Complex cases may take longer.\r\nWe may need to contact you for more information before we can respond.\r\n");
    }
    
    [TestMethod]
    public async Task GetPreCheckPage_ReturnsExpectedDetails()
    {
        var contentfulService = new MockContentfulService();

        var result = await contentfulService.GetPreCheckPage();
        result.Should().NotBeNull();
        result.Should().BeAssignableTo<PreCheckPage>();
        result.Header.Should().Be("Get ready to start the qualification check");
        result.BackButton.Should().BeEquivalentTo(new NavigationLink
                                                  {
                                                      DisplayText = "Back",
                                                      OpenInNewTab = false,
                                                      Href = "/"
                                                  });
        result.PostHeaderContent!.Content[0].Should().BeAssignableTo<Paragraph>()
              .Which.Content.Should().ContainSingle(x => ((Text)x).Value == "This is the post header content");
        result.Question.Should().Be("Do you have all the information you need to complete the check?");
        result.Options.Should().NotBeNullOrEmpty();
        result.Options.Count.Should().Be(2);
        (result.Options[0] as Option)!.Label.Should().Be("Yes");
        (result.Options[0] as Option)!.Value.Should().Be("yes");
        (result.Options[1] as Option)!.Label.Should().Be("No");
        (result.Options[1] as Option)!.Value.Should().Be("no");
        result.InformationMessage.Should().Be("You need all the information listed above to get a result. If you do not have it, you will not be able to complete this check.");
        result.CtaButtonText.Should().Be("Continue");
        result.ErrorBannerHeading.Should().Be("There is a problem");
        result.ErrorMessage.Should().Be("Confirm if you have all the information you need to complete the check");
    }

    [TestMethod]
    public async Task GetFooter_ReturnsExpectedDetails()
    {
        var contentfulService = new MockContentfulService();

        var result = await contentfulService.GetFooter();

        result.Should().NotBeNull();
        result.NavigationLinks.Should().NotBeNull();
        result.NavigationLinks.Count.Should().Be(2);
        result.NavigationLinks[0].DisplayText.Should().Be("Privacy notice");
        result.NavigationLinks[1].DisplayText.Should().Be("Accessibility statement");
        result.LeftHandSideFooterSection.Should().NotBeNull();
        result.LeftHandSideFooterSection.Heading.Should().Be("Left section");
        result.LeftHandSideFooterSection.Body.Content[0].Should().BeAssignableTo<Paragraph>()
              .Which.Content.Should().ContainSingle(x => ((Text)x).Value == "This is the left hand side footer content");
        result.RightHandSideFooterSection.Should().NotBeNull();
        result.RightHandSideFooterSection.Heading.Should().Be("Right section");
        result.RightHandSideFooterSection.Body.Content[0].Should().BeAssignableTo<Paragraph>()
              .Which.Content.Should().ContainSingle(x => ((Text)x).Value == "This is the right hand side footer content");
    }
    
    [TestMethod]
    public async Task GetFeedbackFormPage_ReturnsExpectedDetails()
    {
        var contentfulService = new MockContentfulService();
        
        var result = await contentfulService.GetFeedbackFormPage();
        
        result.Should().NotBeNull();
        result.Heading.Should().Be("Give feedback");
        result.PostHeadingContent.Should().NotBeNull();
        result.PostHeadingContent.Content[0].Should().BeAssignableTo<Paragraph>()
              .Which.Content.Should().ContainSingle(x => ((Text)x).Value == "This is the post heading content");
        result.BackButton.Should().BeEquivalentTo(new NavigationLink
                                                  {
                                                      DisplayText = "Home",
                                                      OpenInNewTab = false,
                                                      Href = "/"
                                                  });
        result.CtaButtonText.Should().Be("Submit feedback");
        result.ErrorBannerHeading.Should().Be("There is a problem");
        result.Questions.Should().NotBeNullOrEmpty();
        result.Questions.Count.Should().Be(3);
        result.Questions[0].Should().BeAssignableTo<FeedbackFormQuestionRadio>();
        
        var question0 = (FeedbackFormQuestionRadio)result.Questions[0];
        question0.Should().NotBeNull();
        question0.Sys.Should().NotBeNull();
        question0.Sys.Id.Should().Be(FeedbackFormQuestions.WouldYouLikeToBeContactedAboutResearch);
        question0.Question.Should().Be("Did you get everything you needed today?");
        question0.Options.Should().NotBeNullOrEmpty();
        question0.Options.Count.Should().Be(2);
        (question0.Options[0] as Option)!.Label.Should().Be("Yes");
        (question0.Options[0] as Option)!.Value.Should().Be("yes");
        (question0.Options[1] as Option)!.Label.Should().Be("No");
        (question0.Options[1] as Option)!.Value.Should().Be("no");
        question0.IsTheQuestionMandatory.Should().BeTrue();
        question0.ErrorMessage.Should().Be("Select whether you got everything you needed today");
        
        var question1 = (FeedbackFormQuestionTextArea)result.Questions[1];
        question1.Should().NotBeNull();
        question1.Question.Should().Be("Tell us about your experience (optional)");
        question1.HintText.Should().Be("Do not include personal information, for example the name of the qualification holder");
        
        var question2 = (FeedbackFormQuestionRadioAndInput)result.Questions[2];
        question2.Should().NotBeNull();
        question2.Question.Should().Be("Would you like us to contact you about future user research?");
        question2.Options.Should().NotBeNullOrEmpty();
        question2.Options.Count.Should().Be(2);
        (question2.Options[0] as Option)!.Label.Should().Be("Yes");
        (question2.Options[0] as Option)!.Value.Should().Be("yes");
        (question2.Options[1] as Option)!.Label.Should().Be("No");
        (question2.Options[1] as Option)!.Value.Should().Be("no");
        question2.IsTheQuestionMandatory.Should().BeTrue();
        question2.InputHeading.Should().Be("Your email address");
        question2.InputHeadingHintText.Should().Be("Input heading hint text");
        question2.ValidateInputAsAnEmailAddress.Should().BeTrue();
        question2.ErrorMessage.Should().Be("Select whether you want to be contacted about future research");
        question2.ErrorMessageForInput.Should().Be("Enter your email address");
        question2.ErrorMessageForInvalidEmailFormat.Should()
                 .Be("Enter an email address in the correct format, like name@example.com");
    }

    [TestMethod]
    public async Task GetFeedbackFormConfirmationPage_ReturnsExpectedDetails()
    {
        var contentfulService = new MockContentfulService();
        
        var result = await contentfulService.GetFeedbackFormConfirmationPage();
        
        result.Should().NotBeNull();
        result.SuccessMessage.Should().Be("Your feedback has been successfully submitted");
        result.Body.Should().NotBeNull();
        result.Body.Content[0].Should().BeAssignableTo<Paragraph>()
              .Which.Content.Should().ContainSingle(x => ((Text)x).Value == "Thank you for your feedback. We look at every piece of feedback and will use your comments to make the service better for everyone.");
        result.OptionalEmailHeading.Should().Be("What happens next");
        result.OptionalEmailBody.Should().NotBeNull();
        result.OptionalEmailBody.Content[0].Should().BeAssignableTo<Paragraph>()
              .Which.Content.Should().ContainSingle(x => ((Text)x).Value == "As you agreed to be contacted about future research, someone from our research team may contact you by email.");
        result.ReturnToHomepageLink.Should().BeEquivalentTo(new NavigationLink
                                                            {
                                                                DisplayText = "Home",
                                                                OpenInNewTab = false,
                                                                Href = "/"
                                                            });
    }
}