using Contentful.Core.Models;
using Dfe.EarlyYearsQualification.Content.Constants;
using Dfe.EarlyYearsQualification.Content.Entities;
using Dfe.EarlyYearsQualification.Mock.Content;
using FluentAssertions;

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
    public async Task GetAdvicePage_QualificationLevel7_ReturnsExpectedDetails()
    {
        var contentfulService = new MockContentfulService();

        var result = await contentfulService.GetAdvicePage(AdvicePages.QualificationLevel7);
        result.Should().NotBeNull();
        result.Should().BeAssignableTo<AdvicePage>();
        result!.Heading.Should().Be("Qualification at Level 7");
        result.Body!.Content[0].Should().BeAssignableTo<Paragraph>()
              .Which.Content.Should().ContainSingle(x => ((Text)x).Value == "Test Advice Page Body");
    }

    [TestMethod]
    public async Task GetAdvicePage_Level6QualificationPre2014_ReturnsExpectedDetails()
    {
        var contentfulService = new MockContentfulService();

        var result = await contentfulService.GetAdvicePage(AdvicePages.Level6QualificationPre2014);
        result.Should().NotBeNull();
        result.Should().BeAssignableTo<AdvicePage>();
        result!.Heading.Should().NotBeNullOrEmpty();
        result.Body!.Content[0].Should().BeAssignableTo<Paragraph>()
              .Which.Content.Should().ContainSingle(x => ((Text)x).Value == "Test Advice Page Body");
    }

    [TestMethod]
    public async Task GetAdvicePage_Level6QualificationPost2014_ReturnsExpectedDetails()
    {
        var contentfulService = new MockContentfulService();

        var result = await contentfulService.GetAdvicePage(AdvicePages.Level6QualificationPost2014);
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
    public async Task GetQualificationById_ReturnsExpectedDetails()
    {
        var contentfulService = new MockContentfulService();

        var result = await contentfulService.GetQualificationById("test_id");
        result.Should().NotBeNull();
        result.Should().BeAssignableTo<Qualification>();
        result!.AdditionalRequirements.Should().NotBeNullOrEmpty();
        result.AwardingOrganisationTitle.Should().NotBeNullOrEmpty();
        result.FromWhichYear.Should().NotBeNullOrEmpty();
        result.QualificationId.Should().NotBeNullOrEmpty();
        result.QualificationLevel.Should().BeGreaterThan(0);
        result.QualificationName.Should().NotBeNullOrEmpty();
        result.QualificationNumber.Should().NotBeNullOrEmpty();
        result.ToWhichYear.Should().NotBeNullOrEmpty();
        result.AdditionalRequirementQuestions.Should().NotBeNull();
        result.AdditionalRequirementQuestions!.Count.Should().Be(2);
        result.AdditionalRequirementQuestions[0].Question.Should().NotBeNullOrEmpty();
        result.AdditionalRequirementQuestions[0].HintText.Should().NotBeNullOrEmpty();
        result.AdditionalRequirementQuestions[0].ConfirmationStatement.Should().NotBeNullOrEmpty();
        result.AdditionalRequirementQuestions[0].DetailsHeading.Should().NotBeNullOrEmpty();
        result.AdditionalRequirementQuestions[0].DetailsHeading.Should().NotBeNullOrEmpty();
        result.AdditionalRequirementQuestions[0].Answers.Should().NotBeNull();
        result.AdditionalRequirementQuestions[0].Answers.Count.Should().Be(2);
        result.AdditionalRequirementQuestions[0].Answers[0].Label.Should().NotBeNullOrEmpty();
        result.AdditionalRequirementQuestions[0].Answers[0].Value.Should().NotBeNullOrEmpty();
        result.AdditionalRequirementQuestions[0].Answers[1].Label.Should().NotBeNullOrEmpty();
        result.AdditionalRequirementQuestions[0].Answers[1].Value.Should().NotBeNullOrEmpty();
        result.AdditionalRequirementQuestions[1].Question.Should().NotBeNullOrEmpty();
        result.AdditionalRequirementQuestions[1].HintText.Should().NotBeNullOrEmpty();
        result.AdditionalRequirementQuestions[1].ConfirmationStatement.Should().NotBeNullOrEmpty();
        result.AdditionalRequirementQuestions[1].DetailsHeading.Should().NotBeNullOrEmpty();
        result.AdditionalRequirementQuestions[1].DetailsHeading.Should().NotBeNullOrEmpty();
        result.AdditionalRequirementQuestions[1].Answers.Should().NotBeNull();
        result.AdditionalRequirementQuestions[1].Answers.Count.Should().Be(2);
        result.AdditionalRequirementQuestions[1].Answers[0].Label.Should().NotBeNullOrEmpty();
        result.AdditionalRequirementQuestions[1].Answers[0].Value.Should().NotBeNullOrEmpty();
        result.AdditionalRequirementQuestions[1].Answers[1].Label.Should().NotBeNullOrEmpty();
        result.AdditionalRequirementQuestions[1].Answers[1].Value.Should().NotBeNullOrEmpty();
        result.RatioRequirements.Should().NotBeNullOrEmpty();
        result.RatioRequirements!.Count.Should().Be(4);
        result.RatioRequirements[0].RatioRequirementName.Should().Be(RatioRequirements.Level2RatioRequirementName);
        result.RatioRequirements[0].FullAndRelevantForLevel2Before2014.Should().BeFalse();
        result.RatioRequirements[0].FullAndRelevantForLevel2After2014.Should().BeFalse();
        result.RatioRequirements[0].FullAndRelevantForLevel3Before2014.Should().BeFalse();
        result.RatioRequirements[0].FullAndRelevantForLevel3After2014.Should().BeTrue();
        result.RatioRequirements[0].FullAndRelevantForLevel4Before2014.Should().BeFalse();
        result.RatioRequirements[0].FullAndRelevantForLevel4After2014.Should().BeFalse();
        result.RatioRequirements[0].FullAndRelevantForLevel5Before2014.Should().BeFalse();
        result.RatioRequirements[0].FullAndRelevantForLevel5After2014.Should().BeFalse();
        result.RatioRequirements[0].FullAndRelevantForLevel6Before2014.Should().BeFalse();
        result.RatioRequirements[0].FullAndRelevantForLevel6After2014.Should().BeFalse();
        result.RatioRequirements[0].FullAndRelevantForLevel7Before2014.Should().BeFalse();
        result.RatioRequirements[0].FullAndRelevantForLevel7After2014.Should().BeFalse();
        result.RatioRequirements[0].FullAndRelevantForQtsEtcBefore2014.Should().BeFalse();
        result.RatioRequirements[0].FullAndRelevantForQtsEtcAfter2014.Should().BeFalse();
        result.RatioRequirements[0].RequirementForLevel2Before2014.Should().BeNull();
        result.RatioRequirements[0].RequirementForLevel2After2014.Should().BeNull();
        result.RatioRequirements[0].RequirementForLevel3Before2014.Should().BeNull();
        result.RatioRequirements[0].RequirementForLevel3After2014.Should().BeNull();
        result.RatioRequirements[0].RequirementForLevel4Before2014.Should().BeNull();
        result.RatioRequirements[0].RequirementForLevel4After2014.Should().BeNull();
        result.RatioRequirements[0].RequirementForLevel5Before2014.Should().BeNull();
        result.RatioRequirements[0].RequirementForLevel5After2014.Should().BeNull();
        result.RatioRequirements[0].RequirementForLevel6Before2014.Should().BeNull();
        result.RatioRequirements[0].RequirementForLevel6After2014.Should().BeNull();
        result.RatioRequirements[0].RequirementForLevel7Before2014.Should().BeNull();
        result.RatioRequirements[0].RequirementForLevel7After2014.Should().BeNull();
        result.RatioRequirements[0].RequirementForQtsEtcBefore2014.Should().BeNull();
        result.RatioRequirements[0].RequirementForQtsEtcAfter2014.Should().BeNull();
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
        result.Options.Count.Should().Be(5);
        result.Options[0].Label.Should().Be("England");
        result.Options[0].Value.Should().Be("england");
        result.Options[1].Label.Should().Be("Scotland");
        result.Options[1].Value.Should().Be("scotland");
        result.Options[2].Label.Should().Be("Wales");
        result.Options[2].Value.Should().Be("wales");
        result.Options[3].Label.Should().Be("Northern Ireland");
        result.Options[3].Value.Should().Be("northern-ireland");
        result.Options[4].Label.Should().Be("Outside the United Kingdom");
        result.Options[4].Value.Should().Be("outside-uk");
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
        result.Options[0].Label.Should().Be("Level 2");
        result.Options[0].Value.Should().Be("2");
        result.Options[1].Label.Should().Be("Level 3");
        result.Options[1].Value.Should().Be("3");
        result.Options[2].Label.Should().Be("Level 6");
        result.Options[2].Value.Should().Be("6");
        result.Options[3].Label.Should().Be("Level 7");
        result.Options[3].Value.Should().Be("7");
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
    public async Task GetDateQuestionPage_PassWhenWasQualificationStartedId_ReturnsExpectedDetails()
    {
        var contentfulService = new MockContentfulService();

        var result = await contentfulService.GetDateQuestionPage(QuestionPages.WhenWasTheQualificationStarted);

        result.Should().NotBeNull();
        result!.CtaButtonText.Should().Be("Continue");
        result.ErrorMessage.Should().Be("Test Error Message");
        result.ErrorBannerHeading.Should().Be("There is a problem");
        result.ErrorBannerLinkText.Should().Be("Test error banner link text");
        result.AdditionalInformationHeader.Should().Be("This is the additional information header");
        result.AdditionalInformationBody!.Content[0].Should().BeAssignableTo<Paragraph>()
              .Which.Content.Should().ContainSingle(x => ((Text)x).Value == "This is the additional information body");
        result.MonthLabel.Should().Be("Test Month Label");
        result.YearLabel.Should().Be("Test Year Label");
        result.QuestionHint.Should().Be("Test Question Hint");
    }

    [TestMethod]
    public async Task GetDateQuestionPage_PassInvalidEntryId_ReturnsException()
    {
        var contentfulService = new MockContentfulService();

        Func<Task> act = () => contentfulService.GetDateQuestionPage("Fake_entry_id");

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
    public async Task GetQualifications_ReturnsAListOfQualifications()
    {
        var contentfulService = new MockContentfulService();

        var result = await contentfulService.GetQualifications();

        result.Count.Should().Be(5);
    }

    [TestMethod]
    public async Task GetQualifications_ReturnsAQualificationWithAnAdditionalRequirementsQuestion()
    {
        var contentfulService = new MockContentfulService();

        var result = await contentfulService.GetQualifications();

        result.Count.Should().Be(5);

        var qualificationWithAdditionalRequirements = result[4];
        qualificationWithAdditionalRequirements.AdditionalRequirementQuestions.Should().HaveCount(1);

        var additionalRequirementQuestions = qualificationWithAdditionalRequirements.AdditionalRequirementQuestions;

        additionalRequirementQuestions.Should().HaveCount(1);
        additionalRequirementQuestions![0].AnswerToBeFullAndRelevant.Should().Be(true);

        var answers = additionalRequirementQuestions[0].Answers;

        answers[0].Label.Should().Be("Yes");
        answers[0].Value.Should().Be("yes");
        answers[1].Label.Should().Be("No");
        answers[1].Value.Should().Be("no");
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
                                                       Href = "/"
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
}