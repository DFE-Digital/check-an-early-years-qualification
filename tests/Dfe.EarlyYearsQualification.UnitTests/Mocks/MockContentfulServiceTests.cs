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
    public async Task GetAdvicePage_ReturnsExpectedDetails()
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
    public async Task GetAdvicePage_UnknownEntryId_ReturnsException()
    {
        var contentfulService = new MockContentfulService();

        Func<Task> act = async () => await contentfulService.GetAdvicePage("Invalid entry Id");

        await act.Should().ThrowAsync<NotImplementedException>()
                 .WithMessage("No advice page mock for entry Invalid entry Id");
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
        result!.Count.Should().Be(2);
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
        result.Notes.Should().NotBeNullOrEmpty();
        result.QualificationId.Should().NotBeNullOrEmpty();
        result.QualificationLevel.Should().BeGreaterThan(0);
        result.QualificationName.Should().NotBeNullOrEmpty();
        result.QualificationNumber.Should().NotBeNullOrEmpty();
        result.ToWhichYear.Should().NotBeNullOrEmpty();
    }

    [TestMethod]
    public async Task GetQuestionPage_PassInWhereWasTheQualificationAwarded_ReturnsExpectedDetails()
    {
        var contentfulService = new MockContentfulService();

        var result = await contentfulService.GetQuestionPage(QuestionPages.WhereWasTheQualificationAwarded);
        result.Should().NotBeNull();
        result.Should().BeAssignableTo<QuestionPage>();
        result!.Question.Should().NotBeNullOrEmpty();
        result.CtaButtonText.Should().NotBeNullOrEmpty();
        result.ErrorMessage.Should().NotBeNullOrEmpty();
        result.Options.Should().NotBeNullOrEmpty();
        result.Options.Count.Should().Be(2);
        result.Options[0].Label.Should().Be("England");
        result.Options[0].Value.Should().Be("england");
        result.Options[1].Label.Should().Be("Outside the United Kingdom");
        result.Options[1].Value.Should().Be("outside-uk");
    }

    [TestMethod]
    public async Task GetQuestionPage_PassInWhatLevelIsTheQualification_ReturnsExpectedDetails()
    {
        var contentfulService = new MockContentfulService();

        var result = await contentfulService.GetQuestionPage(QuestionPages.WhatLevelIsTheQualification);
        result.Should().NotBeNull();
        result.Should().BeAssignableTo<QuestionPage>();
        result!.Question.Should().NotBeNullOrEmpty();
        result.CtaButtonText.Should().NotBeNullOrEmpty();
        result.ErrorMessage.Should().NotBeNullOrEmpty();
        result.Options.Should().NotBeNullOrEmpty();
        result.Options.Count.Should().Be(2);
        result.Options[0].Label.Should().Be("Level 2");
        result.Options[0].Value.Should().Be("2");
        result.Options[1].Label.Should().Be("Level 3");
        result.Options[1].Value.Should().Be("3");
    }

    [TestMethod]
    public async Task GetQuestionPage_PassInvalidEntryId_ReturnsException()
    {
        var contentfulService = new MockContentfulService();

        Func<Task> act = () => contentfulService.GetQuestionPage("Fake_entry_id");

        await act.Should().ThrowAsync<NotImplementedException>()
            .WithMessage("No question page mock for entry Fake_entry_id");
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
}