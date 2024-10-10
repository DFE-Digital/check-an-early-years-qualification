using System.Text.Json;
using Dfe.EarlyYearsQualification.Content.Constants;
using Dfe.EarlyYearsQualification.Web.Constants;
using Dfe.EarlyYearsQualification.Web.Services.Cookies;
using Dfe.EarlyYearsQualification.Web.Services.UserJourneyCookieService;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;

namespace Dfe.EarlyYearsQualification.UnitTests.Services;

[TestClass]
public class UserJourneyCookieServiceTests
{
    [TestMethod]
    public void SetWhereWasQualificationAwarded_StringProvided_SetsCookieCorrectly()
    {
        var modelInCookie = new UserJourneyCookieService.UserJourneyModel();
        var mockHttpContextAccessor = SetCookieManagerWithExistingCookie(modelInCookie);
        var mockLogger = new Mock<ILogger<UserJourneyCookieService>>();

        var service = new UserJourneyCookieService(mockLogger.Object, mockHttpContextAccessor.cookieManager.Object);

        service.SetWhereWasQualificationAwarded("some test string");

        var model = new UserJourneyCookieService.UserJourneyModel
                    {
                        WhereWasQualificationAwarded = "some test string"
                    };

        CheckSerializedModelWasSet(mockHttpContextAccessor, model);
    }

    [TestMethod]
    public void SetWhenWasQualificationStarted_StringProvided_SetsCookieCorrectly()
    {
        var modelInCookie = new UserJourneyCookieService.UserJourneyModel();
        var mockHttpContextAccessor = SetCookieManagerWithExistingCookie(modelInCookie);
        var mockLogger = new Mock<ILogger<UserJourneyCookieService>>();

        var service = new UserJourneyCookieService(mockLogger.Object, mockHttpContextAccessor.cookieManager.Object);

        service.SetWhenWasQualificationStarted("some test string");

        var model = new UserJourneyCookieService.UserJourneyModel
                    {
                        WhenWasQualificationStarted = "some test string"
                    };

        CheckSerializedModelWasSet(mockHttpContextAccessor, model);
    }

    [TestMethod]
    public void SetLevelOfQualification_StringProvided_SetsCookieCorrectly()
    {
        var modelInCookie = new UserJourneyCookieService.UserJourneyModel();
        var mockHttpContextAccessor = SetCookieManagerWithExistingCookie(modelInCookie);
        var mockLogger = new Mock<ILogger<UserJourneyCookieService>>();

        var service = new UserJourneyCookieService(mockLogger.Object, mockHttpContextAccessor.cookieManager.Object);

        service.SetLevelOfQualification("some test string");

        var model = new UserJourneyCookieService.UserJourneyModel
                    {
                        LevelOfQualification = "some test string"
                    };

        CheckSerializedModelWasSet(mockHttpContextAccessor, model);
    }

    [TestMethod]
    public void SetAwardingOrganisation_StringProvided_SetsCookieCorrectly()
    {
        var modelInCookie = new UserJourneyCookieService.UserJourneyModel();
        var mockHttpContextAccessor = SetCookieManagerWithExistingCookie(modelInCookie);
        var mockLogger = new Mock<ILogger<UserJourneyCookieService>>();

        var service = new UserJourneyCookieService(mockLogger.Object, mockHttpContextAccessor.cookieManager.Object);

        service.SetAwardingOrganisation("some test string");

        var model = new UserJourneyCookieService.UserJourneyModel
                    {
                        WhatIsTheAwardingOrganisation = "some test string"
                    };

        CheckSerializedModelWasSet(mockHttpContextAccessor, model);
    }

    [TestMethod]
    public void GetUserJourneyModelFromCookie_NoCookiePresent_ReturnsDefaultValues()
    {
        var mockHttpContextAccessor = SetCookieManagerWithExistingCookie(null);
        var mockLogger = new Mock<ILogger<UserJourneyCookieService>>();

        var service = new UserJourneyCookieService(mockLogger.Object, mockHttpContextAccessor.cookieManager.Object);

        service.GetAwardingOrganisation().Should().BeNull();
        service.GetWhenWasQualificationStarted().Should().Be((null, null));
        service.GetLevelOfQualification().Should().BeNull();
        service.GetWhereWasQualificationAwarded().Should().BeNull();
        service.GetQualificationWasSelectedFromList().Should().Be(YesOrNo.No);
        service.GetSearchCriteria().Should().BeNull();
        service.GetAdditionalQuestionsAnswers().Should().BeEmpty();
        service.GetAwardingOrganisationIsNotOnList().Should().BeFalse();
    }

    [TestMethod]
    public void ResetUserJourneyCookie_NoCookieSet_AddsBaseModelAsCookie()
    {
        var mockHttpContextAccessor = SetCookieManagerWithExistingCookie(null);
        var mockLogger = new Mock<ILogger<UserJourneyCookieService>>();

        var service = new UserJourneyCookieService(mockLogger.Object, mockHttpContextAccessor.cookieManager.Object);

        service.ResetUserJourneyCookie();

        CheckSerializedModelWasSet(mockHttpContextAccessor, new UserJourneyCookieService.UserJourneyModel());
    }

    [TestMethod]
    public void ResetUserJourneyCookie_FullModelAsCookieExists_AddsBaseModelAsCookie()
    {
        var model = new UserJourneyCookieService.UserJourneyModel
                    {
                        LevelOfQualification = "test level of qualification",
                        WhenWasQualificationStarted = "test when was qualification started",
                        WhereWasQualificationAwarded = "test where was qualification awarded"
                    };
        var mockHttpContextAccessor = SetCookieManagerWithExistingCookie(model);
        var mockLogger = new Mock<ILogger<UserJourneyCookieService>>();

        var service = new UserJourneyCookieService(mockLogger.Object, mockHttpContextAccessor.cookieManager.Object);

        service.ResetUserJourneyCookie();

        CheckSerializedModelWasSet(mockHttpContextAccessor, new UserJourneyCookieService.UserJourneyModel());
    }

    [TestMethod]
    public void GetWhereWasQualificationAwarded_CookieValueIsEmpty_ReturnsNull()
    {
        var existingModel = new UserJourneyCookieService.UserJourneyModel
                            {
                                WhereWasQualificationAwarded = string.Empty
                            };
        var mockHttpContextAccessor = SetCookieManagerWithExistingCookie(existingModel);
        var mockLogger = new Mock<ILogger<UserJourneyCookieService>>();

        var service = new UserJourneyCookieService(mockLogger.Object, mockHttpContextAccessor.cookieManager.Object);

        var model = service.GetWhereWasQualificationAwarded();

        model.Should().BeNull();
    }

    [TestMethod]
    public void GetWhereWasQualificationAwarded_CookieHasValue_ReturnsValueWithUpperCaseFirstLetter()
    {
        var existingModel = new UserJourneyCookieService.UserJourneyModel
                            {
                                WhereWasQualificationAwarded = "england"
                            };
        var mockHttpContextAccessor = SetCookieManagerWithExistingCookie(existingModel);
        var mockLogger = new Mock<ILogger<UserJourneyCookieService>>();

        var service = new UserJourneyCookieService(mockLogger.Object, mockHttpContextAccessor.cookieManager.Object);

        var model = service.GetWhereWasQualificationAwarded();

        model.Should().Be("England");
    }

    [TestMethod]
    public void GetAwardingOrganisation_CookieValueIsEmpty_ReturnsNull()
    {
        var existingModel = new UserJourneyCookieService.UserJourneyModel
                            {
                                WhatIsTheAwardingOrganisation = string.Empty
                            };
        var mockHttpContextAccessor = SetCookieManagerWithExistingCookie(existingModel);
        var mockLogger = new Mock<ILogger<UserJourneyCookieService>>();

        var service = new UserJourneyCookieService(mockLogger.Object, mockHttpContextAccessor.cookieManager.Object);

        var model = service.GetAwardingOrganisation();

        model.Should().BeNull();
    }

    [TestMethod]
    public void GetAwardingOrganisation_CookieHasValue_ReturnsValue()
    {
        var existingModel = new UserJourneyCookieService.UserJourneyModel
                            {
                                WhatIsTheAwardingOrganisation = AwardingOrganisations.Ncfe
                            };
        var mockHttpContextAccessor = SetCookieManagerWithExistingCookie(existingModel);
        var mockLogger = new Mock<ILogger<UserJourneyCookieService>>();

        var service = new UserJourneyCookieService(mockLogger.Object, mockHttpContextAccessor.cookieManager.Object);

        var model = service.GetAwardingOrganisation();

        model.Should().Be(AwardingOrganisations.Ncfe);
    }

    [TestMethod]
    public void GetAwardingOrganisationIsNotOnList_CookieValueIsFalse_ReturnsValue()
    {
        var existingModel = new UserJourneyCookieService.UserJourneyModel();

        var mockHttpContextAccessor = SetCookieManagerWithExistingCookie(existingModel);
        var mockLogger = new Mock<ILogger<UserJourneyCookieService>>();

        var service = new UserJourneyCookieService(mockLogger.Object, mockHttpContextAccessor.cookieManager.Object);

        var model = service.GetAwardingOrganisationIsNotOnList();

        model.Should().BeFalse();
    }

    [TestMethod]
    public void GetAwardingOrganisationIsNotOnList_CookieValueIsTrue_ReturnsValue()
    {
        var existingModel = new UserJourneyCookieService.UserJourneyModel
                            { SelectedAwardingOrganisationNotOnTheList = true };

        var mockHttpContextAccessor = SetCookieManagerWithExistingCookie(existingModel);
        var mockLogger = new Mock<ILogger<UserJourneyCookieService>>();

        var service = new UserJourneyCookieService(mockLogger.Object, mockHttpContextAccessor.cookieManager.Object);

        var model = service.GetAwardingOrganisationIsNotOnList();

        model.Should().BeTrue();
    }

    [TestMethod]
    public void GetLevelOfQualification_CookieValueIsEmpty_ReturnsNull()
    {
        var existingModel = new UserJourneyCookieService.UserJourneyModel
                            {
                                LevelOfQualification = string.Empty
                            };
        var mockHttpContextAccessor = SetCookieManagerWithExistingCookie(existingModel);
        var mockLogger = new Mock<ILogger<UserJourneyCookieService>>();

        var service = new UserJourneyCookieService(mockLogger.Object, mockHttpContextAccessor.cookieManager.Object);

        var model = service.GetLevelOfQualification();

        model.Should().BeNull();
    }

    [TestMethod]
    public void GetLevelOfQualification_CookieHasValue_ReturnsValue()
    {
        var existingModel = new UserJourneyCookieService.UserJourneyModel
                            {
                                LevelOfQualification = "4"
                            };
        var mockHttpContextAccessor = SetCookieManagerWithExistingCookie(existingModel);
        var mockLogger = new Mock<ILogger<UserJourneyCookieService>>();

        var service = new UserJourneyCookieService(mockLogger.Object, mockHttpContextAccessor.cookieManager.Object);

        var model = service.GetLevelOfQualification();

        model.Should().Be(4);
    }

    [TestMethod]
    public void GetWhenWasQualificationStarted_CookieValueIsEmpty_ReturnsNull()
    {
        var existingModel = new UserJourneyCookieService.UserJourneyModel
                            {
                                WhenWasQualificationStarted = string.Empty
                            };
        var mockHttpContextAccessor = SetCookieManagerWithExistingCookie(existingModel);
        var mockLogger = new Mock<ILogger<UserJourneyCookieService>>();

        var service = new UserJourneyCookieService(mockLogger.Object, mockHttpContextAccessor.cookieManager.Object);

        var (startMonth, startYear) = service.GetWhenWasQualificationStarted();

        startMonth.Should().BeNull();
        startYear.Should().BeNull();
    }

    [TestMethod]
    public void GetWhenWasQualificationStarted_CookieHasInvalidValue_ReturnsNull()
    {
        var existingModel = new UserJourneyCookieService.UserJourneyModel
                            {
                                WhenWasQualificationStarted = "4"
                            };
        var mockHttpContextAccessor = SetCookieManagerWithExistingCookie(existingModel);
        var mockLogger = new Mock<ILogger<UserJourneyCookieService>>();

        var service = new UserJourneyCookieService(mockLogger.Object, mockHttpContextAccessor.cookieManager.Object);

        var (startMonth, startYear) = service.GetWhenWasQualificationStarted();

        startMonth.Should().BeNull();
        startYear.Should().BeNull();
    }

    [TestMethod]
    public void GetWhenWasQualificationStarted_CookieHasValidValue_ReturnsValue()
    {
        var existingModel = new UserJourneyCookieService.UserJourneyModel
                            {
                                WhenWasQualificationStarted = "4/2015"
                            };
        var mockHttpContextAccessor = SetCookieManagerWithExistingCookie(existingModel);
        var mockLogger = new Mock<ILogger<UserJourneyCookieService>>();

        var service = new UserJourneyCookieService(mockLogger.Object, mockHttpContextAccessor.cookieManager.Object);

        var (startMonth, startYear) = service.GetWhenWasQualificationStarted();

        startMonth.Should().Be(4);
        startYear.Should().Be(2015);
    }

    [TestMethod]
    public void GetQualificationStartedBeforeSept2019_CookieValueIsEmpty_Throws()
    {
        var existingModel = new UserJourneyCookieService.UserJourneyModel
                            {
                                WhenWasQualificationStarted = string.Empty
                            };

        var mockHttpContextAccessor = SetCookieManagerWithExistingCookie(existingModel);
        var mockLogger = new Mock<ILogger<UserJourneyCookieService>>();

        var service = new UserJourneyCookieService(mockLogger.Object, mockHttpContextAccessor.cookieManager.Object);

        var action = () => service.WasStartedBeforeSeptember2014();

        action.Should().Throw<InvalidOperationException>();
    }

    [TestMethod]
    public void GetQualificationStartedBeforeSept2019_CookieHasInvalidValue_Throws()
    {
        var existingModel = new UserJourneyCookieService.UserJourneyModel
                            {
                                WhenWasQualificationStarted = "4"
                            };

        var mockHttpContextAccessor = SetCookieManagerWithExistingCookie(existingModel);
        var mockLogger = new Mock<ILogger<UserJourneyCookieService>>();

        var service = new UserJourneyCookieService(mockLogger.Object, mockHttpContextAccessor.cookieManager.Object);

        var action = () => service.WasStartedBeforeSeptember2014();

        action.Should().Throw<InvalidOperationException>();
    }

    [TestMethod]
    public void GetQualificationStartedBeforeSept2019_CookieHasValidValueIn2013_ReturnsTrue()
    {
        var existingModel = new UserJourneyCookieService.UserJourneyModel
                            {
                                WhenWasQualificationStarted = "12/2013"
                            };

        var mockHttpContextAccessor = SetCookieManagerWithExistingCookie(existingModel);
        var mockLogger = new Mock<ILogger<UserJourneyCookieService>>();

        var service = new UserJourneyCookieService(mockLogger.Object, mockHttpContextAccessor.cookieManager.Object);

        service.WasStartedBeforeSeptember2014().Should().BeTrue();
    }

    [TestMethod]
    public void GetQualificationStartedBeforeSept2019_CookieHasValidValueInAugust2014_ReturnsTrue()
    {
        var existingModel = new UserJourneyCookieService.UserJourneyModel
                            {
                                WhenWasQualificationStarted = "8/2014"
                            };

        var mockHttpContextAccessor = SetCookieManagerWithExistingCookie(existingModel);
        var mockLogger = new Mock<ILogger<UserJourneyCookieService>>();

        var service = new UserJourneyCookieService(mockLogger.Object, mockHttpContextAccessor.cookieManager.Object);

        service.WasStartedBeforeSeptember2014().Should().BeTrue();
    }

    [TestMethod]
    public void GetQualificationStartedBeforeSept2019_CookieHasValidValueInSeptember2014_ReturnsFalse()
    {
        var existingModel = new UserJourneyCookieService.UserJourneyModel
                            {
                                WhenWasQualificationStarted = "9/2014"
                            };

        var mockHttpContextAccessor = SetCookieManagerWithExistingCookie(existingModel);
        var mockLogger = new Mock<ILogger<UserJourneyCookieService>>();

        var service = new UserJourneyCookieService(mockLogger.Object, mockHttpContextAccessor.cookieManager.Object);

        service.WasStartedBeforeSeptember2014().Should().BeFalse();
    }

    [TestMethod]
    public void GetQualificationStartedBeforeSept2019_CookieHasValidValueIn2015_ReturnsFalse()
    {
        var existingModel = new UserJourneyCookieService.UserJourneyModel
                            {
                                WhenWasQualificationStarted = "1/2015"
                            };

        var mockHttpContextAccessor = SetCookieManagerWithExistingCookie(existingModel);
        var mockLogger = new Mock<ILogger<UserJourneyCookieService>>();

        var service = new UserJourneyCookieService(mockLogger.Object, mockHttpContextAccessor.cookieManager.Object);

        service.WasStartedBeforeSeptember2014().Should().BeFalse();
    }

    [TestMethod]
    public void GetQualificationStartedBetween2014And2019_CookieValueIsEmpty_Throws()
    {
        var existingModel = new UserJourneyCookieService.UserJourneyModel
                            {
                                WhenWasQualificationStarted = string.Empty
                            };

        var mockHttpContextAccessor = SetCookieManagerWithExistingCookie(existingModel);
        var mockLogger = new Mock<ILogger<UserJourneyCookieService>>();

        var service = new UserJourneyCookieService(mockLogger.Object, mockHttpContextAccessor.cookieManager.Object);

        var action = () => service.WasStartedBetweenSeptember2014AndAugust2019();

        action.Should().Throw<InvalidOperationException>();
    }

    [TestMethod]
    public void GetQualificationStartedBetween2014And2019_CookieHasInvalidValue_Throws()
    {
        var existingModel = new UserJourneyCookieService.UserJourneyModel
                            {
                                WhenWasQualificationStarted = "4"
                            };

        var mockHttpContextAccessor = SetCookieManagerWithExistingCookie(existingModel);
        var mockLogger = new Mock<ILogger<UserJourneyCookieService>>();

        var service = new UserJourneyCookieService(mockLogger.Object, mockHttpContextAccessor.cookieManager.Object);

        var action = () => service.WasStartedBetweenSeptember2014AndAugust2019();

        action.Should().Throw<InvalidOperationException>();
    }

    [TestMethod]
    public void GetQualificationStartedBetween2014And2019_CookieHasValidValueIn2013_ReturnsTrue()
    {
        var existingModel = new UserJourneyCookieService.UserJourneyModel
                            {
                                WhenWasQualificationStarted = "12/2013"
                            };

        var mockHttpContextAccessor = SetCookieManagerWithExistingCookie(existingModel);
        var mockLogger = new Mock<ILogger<UserJourneyCookieService>>();

        var service = new UserJourneyCookieService(mockLogger.Object, mockHttpContextAccessor.cookieManager.Object);

        service.WasStartedBetweenSeptember2014AndAugust2019().Should().BeFalse();
    }

    [TestMethod]
    public void GetQualificationStartedBetween2014And2019_CookieHasValidValueInAugust2014_ReturnsFalse()
    {
        var existingModel = new UserJourneyCookieService.UserJourneyModel
                            {
                                WhenWasQualificationStarted = "8/2014"
                            };

        var mockHttpContextAccessor = SetCookieManagerWithExistingCookie(existingModel);
        var mockLogger = new Mock<ILogger<UserJourneyCookieService>>();

        var service = new UserJourneyCookieService(mockLogger.Object, mockHttpContextAccessor.cookieManager.Object);

        service.WasStartedBetweenSeptember2014AndAugust2019().Should().BeFalse();
    }

    [TestMethod]
    public void GetQualificationStartedBetween2014And2019_CookieHasValidValueInSeptember2014_ReturnsTrue()
    {
        var existingModel = new UserJourneyCookieService.UserJourneyModel
                            {
                                WhenWasQualificationStarted = "9/2014"
                            };

        var mockHttpContextAccessor = SetCookieManagerWithExistingCookie(existingModel);
        var mockLogger = new Mock<ILogger<UserJourneyCookieService>>();

        var service = new UserJourneyCookieService(mockLogger.Object, mockHttpContextAccessor.cookieManager.Object);

        service.WasStartedBetweenSeptember2014AndAugust2019().Should().BeTrue();
    }

    [TestMethod]
    public void GetQualificationStartedBetween2014And2019_CookieHasValidValueIn2015_ReturnsTrue()
    {
        var existingModel = new UserJourneyCookieService.UserJourneyModel
                            {
                                WhenWasQualificationStarted = "1/2015"
                            };

        var mockHttpContextAccessor = SetCookieManagerWithExistingCookie(existingModel);
        var mockLogger = new Mock<ILogger<UserJourneyCookieService>>();

        var service = new UserJourneyCookieService(mockLogger.Object, mockHttpContextAccessor.cookieManager.Object);

        service.WasStartedBetweenSeptember2014AndAugust2019().Should().BeTrue();
    }

    [TestMethod]
    public void GetQualificationStartedBetween2014And2019_CookieHasValidValueIn2018_ReturnsTrue()
    {
        var existingModel = new UserJourneyCookieService.UserJourneyModel
                            {
                                WhenWasQualificationStarted = "12/2018"
                            };

        var mockHttpContextAccessor = SetCookieManagerWithExistingCookie(existingModel);
        var mockLogger = new Mock<ILogger<UserJourneyCookieService>>();

        var service = new UserJourneyCookieService(mockLogger.Object, mockHttpContextAccessor.cookieManager.Object);

        service.WasStartedBetweenSeptember2014AndAugust2019().Should().BeTrue();
    }

    [TestMethod]
    public void GetQualificationStartedBetween2014And2019_CookieHasValidValueInAugust2019_ReturnsTrue()
    {
        var existingModel = new UserJourneyCookieService.UserJourneyModel
                            {
                                WhenWasQualificationStarted = "8/2019"
                            };

        var mockHttpContextAccessor = SetCookieManagerWithExistingCookie(existingModel);
        var mockLogger = new Mock<ILogger<UserJourneyCookieService>>();

        var service = new UserJourneyCookieService(mockLogger.Object, mockHttpContextAccessor.cookieManager.Object);

        service.WasStartedBetweenSeptember2014AndAugust2019().Should().BeTrue();
    }

    [TestMethod]
    public void GetQualificationStartedBetween2014And2019_CookieHasValidValueInSept2019_ReturnsFalse()
    {
        var existingModel = new UserJourneyCookieService.UserJourneyModel
                            {
                                WhenWasQualificationStarted = "9/2019"
                            };

        var mockHttpContextAccessor = SetCookieManagerWithExistingCookie(existingModel);
        var mockLogger = new Mock<ILogger<UserJourneyCookieService>>();

        var service = new UserJourneyCookieService(mockLogger.Object, mockHttpContextAccessor.cookieManager.Object);

        service.WasStartedBetweenSeptember2014AndAugust2019().Should().BeFalse();
    }

    [TestMethod]
    public void GetQualificationStartedBetween2014And2019_CookieHasValidValueIn2020_ReturnsFalse()
    {
        var existingModel = new UserJourneyCookieService.UserJourneyModel
                            {
                                WhenWasQualificationStarted = "1/2020"
                            };

        var mockHttpContextAccessor = SetCookieManagerWithExistingCookie(existingModel);
        var mockLogger = new Mock<ILogger<UserJourneyCookieService>>();

        var service = new UserJourneyCookieService(mockLogger.Object, mockHttpContextAccessor.cookieManager.Object);

        service.WasStartedBetweenSeptember2014AndAugust2019().Should().BeFalse();
    }

    [TestMethod]
    public void SetNameSearchCriteria_StringProvided_SetsCookieCorrectly()
    {
        var modelInCookie = new UserJourneyCookieService.UserJourneyModel();
        var mockHttpContextAccessor = SetCookieManagerWithExistingCookie(modelInCookie);
        var mockLogger = new Mock<ILogger<UserJourneyCookieService>>();

        var service = new UserJourneyCookieService(mockLogger.Object, mockHttpContextAccessor.cookieManager.Object);

        const string searchCriteria = "This is a test";
        service.SetQualificationNameSearchCriteria(searchCriteria);

        var model = new UserJourneyCookieService.UserJourneyModel
                    {
                        SearchCriteria = searchCriteria
                    };

        CheckSerializedModelWasSet(mockHttpContextAccessor, model);
    }

    [TestMethod]
    public void GetSearchCriteria_CookieHasInvalidValue_ReturnsNull()
    {
        var existingModel = new UserJourneyCookieService.UserJourneyModel
                            {
                                SearchCriteria = ""
                            };
        var mockHttpContextAccessor = SetCookieManagerWithExistingCookie(existingModel);
        var mockLogger = new Mock<ILogger<UserJourneyCookieService>>();

        var service = new UserJourneyCookieService(mockLogger.Object, mockHttpContextAccessor.cookieManager.Object);

        var searchCriteria = service.GetSearchCriteria();

        searchCriteria.Should().BeNull();
    }

    [TestMethod]
    public void GetSearchCriteria_CookieHasValidValue_ReturnsValue()
    {
        var existingModel = new UserJourneyCookieService.UserJourneyModel
                            {
                                SearchCriteria = "Test"
                            };
        var mockHttpContextAccessor = SetCookieManagerWithExistingCookie(existingModel);
        var mockLogger = new Mock<ILogger<UserJourneyCookieService>>();

        var service = new UserJourneyCookieService(mockLogger.Object, mockHttpContextAccessor.cookieManager.Object);

        var searchCriteria = service.GetSearchCriteria();

        searchCriteria.Should().Be("Test");
    }

    [TestMethod]
    public void SetAdditionalQuestionsAnswers_DictionaryProvided_SetsCookie()
    {
        var mockHttpContextAccessor =
            SetCookieManagerWithExistingCookie(new UserJourneyCookieService.UserJourneyModel());
        var mockLogger = new Mock<ILogger<UserJourneyCookieService>>();

        var service = new UserJourneyCookieService(mockLogger.Object, mockHttpContextAccessor.cookieManager.Object);

        Dictionary<string, string> dictionary = new() { { "This is a test question", "Answer" } };
        service.SetAdditionalQuestionsAnswers(dictionary);

        var model = new UserJourneyCookieService.UserJourneyModel
                    {
                        AdditionalQuestionsAnswers = dictionary
                    };

        CheckSerializedModelWasSet(mockHttpContextAccessor, model);
    }

    [TestMethod]
    public void SetAdditionalQuestionsAnswers_DictionaryProvided_ReplacesExistingAnswers()
    {
        var mockHttpContextAccessor =
            SetCookieManagerWithExistingCookie(new UserJourneyCookieService.UserJourneyModel());
        var mockLogger = new Mock<ILogger<UserJourneyCookieService>>();

        var service = new UserJourneyCookieService(mockLogger.Object, mockHttpContextAccessor.cookieManager.Object);

        Dictionary<string, string> dictionary1 = new() { { "This is a test question", "Answer" } };
        service.SetAdditionalQuestionsAnswers(dictionary1);

        service.GetAdditionalQuestionsAnswers().Should().Contain(dictionary1);

        Dictionary<string, string> dictionary2 = new() { { "This is another test question", "Another answer" } };
        service.SetAdditionalQuestionsAnswers(dictionary2);

        service.GetAdditionalQuestionsAnswers().Should().Contain(dictionary2)
               .And.NotContain(dictionary1);
    }

    [TestMethod]
    public void SetAdditionalQuestionsAnswers_CookieHasValidValue_ReturnsValue()
    {
        Dictionary<string, string> dictionary = new() { { "This is a test question", "Answer" } };
        var existingModel = new UserJourneyCookieService.UserJourneyModel
                            {
                                AdditionalQuestionsAnswers = dictionary
                            };
        var mockHttpContextAccessor = SetCookieManagerWithExistingCookie(existingModel);
        var mockLogger = new Mock<ILogger<UserJourneyCookieService>>();

        var service = new UserJourneyCookieService(mockLogger.Object, mockHttpContextAccessor.cookieManager.Object);

        var additionalQuestionsAnswers = service.GetAdditionalQuestionsAnswers();

        additionalQuestionsAnswers.Should().Equal(dictionary);
    }

    [TestMethod]
    public void NoAdditionalQuestions_HasAdditionalQuestionsAnswers_ReturnsFalse()
    {
        var model = new UserJourneyCookieService.UserJourneyModel();

        var mockCookieManager = SetCookieManagerWithExistingCookie(model);

        var service = new UserJourneyCookieService(NullLogger<UserJourneyCookieService>.Instance, mockCookieManager.cookieManager.Object);

        service.UserHasAnsweredAdditionalQuestions().Should().BeFalse();
    }

    [TestMethod]
    public void AdditionalQuestions_HasAdditionalQuestionsAnswers_ReturnsTrue()
    {
        Dictionary<string, string> dictionary = new() { { "This is a test question", "Answer" } };
        var model = new UserJourneyCookieService.UserJourneyModel { AdditionalQuestionsAnswers = dictionary };

        var mockCookieManager = SetCookieManagerWithExistingCookie(model);

        var service = new UserJourneyCookieService(NullLogger<UserJourneyCookieService>.Instance, mockCookieManager.cookieManager.Object);

        service.UserHasAnsweredAdditionalQuestions().Should().BeTrue();
    }

    [TestMethod]
    public void SetAwardingOrganisationNotOnList_True_SetsCookieToTrue()
    {
        var mockCookieManager = SetCookieManagerWithExistingCookie(new UserJourneyCookieService.UserJourneyModel());

        var service = new UserJourneyCookieService(NullLogger<UserJourneyCookieService>.Instance, mockCookieManager.cookieManager.Object);

        service.SetAwardingOrganisationNotOnList(true);

        CheckSerializedModelWasSet(mockCookieManager, new UserJourneyCookieService.UserJourneyModel
                                                      {
                                                          SelectedAwardingOrganisationNotOnTheList = true
                                                      });
    }

    [TestMethod]
    public void SetAwardingOrganisationNotOnList_False_SetsCookieToFalse()
    {
        var mockCookieManager = SetCookieManagerWithExistingCookie(new UserJourneyCookieService.UserJourneyModel
                                                                   { SelectedAwardingOrganisationNotOnTheList = true });

        var service = new UserJourneyCookieService(NullLogger<UserJourneyCookieService>.Instance, mockCookieManager.cookieManager.Object);

        service.SetAwardingOrganisationNotOnList(false);

        CheckSerializedModelWasSet(mockCookieManager, new UserJourneyCookieService.UserJourneyModel
                                                      {
                                                          SelectedAwardingOrganisationNotOnTheList = false
                                                      });
    }

    [TestMethod]
    public void SetSelectedFromList_Yes_SetsCookieToYes()
    {
        var mockCookieManager = SetCookieManagerWithExistingCookie(new UserJourneyCookieService.UserJourneyModel());

        var service = new UserJourneyCookieService(NullLogger<UserJourneyCookieService>.Instance, mockCookieManager.cookieManager.Object);

        service.SetUserSelectedQualificationFromList(YesOrNo.Yes);

        CheckSerializedModelWasSet(mockCookieManager, new UserJourneyCookieService.UserJourneyModel
                                                      {
                                                          QualificationWasSelectedFromList = YesOrNo.Yes
                                                      });
    }

    [TestMethod]
    public void SetSelectedFromList_No_SetsCookieToNo()
    {
        var mockCookieManager = SetCookieManagerWithExistingCookie(new UserJourneyCookieService.UserJourneyModel
                                                                   {
                                                                       QualificationWasSelectedFromList = YesOrNo.Yes
                                                                   });

        var service = new UserJourneyCookieService(NullLogger<UserJourneyCookieService>.Instance, mockCookieManager.cookieManager.Object);

        service.SetUserSelectedQualificationFromList(YesOrNo.No);

        CheckSerializedModelWasSet(mockCookieManager, new UserJourneyCookieService.UserJourneyModel
                                                      {
                                                          QualificationWasSelectedFromList = YesOrNo.No
                                                      });
    }

    [TestMethod]
    public void GetSelectedFromList_GetsValueInCookie()
    {
        var mockCookieManager = SetCookieManagerWithExistingCookie(new UserJourneyCookieService.UserJourneyModel
                                                                   {
                                                                       QualificationWasSelectedFromList = YesOrNo.Yes
                                                                   });

        var service = new UserJourneyCookieService(NullLogger<UserJourneyCookieService>.Instance, mockCookieManager.cookieManager.Object);

        var result = service.GetQualificationWasSelectedFromList();
        result.Should().Be(YesOrNo.Yes);
    }

    [TestMethod]
    public static void SetMultipleValues_AllSetInOutboundCookie()
    {
        var mockCookieManager =
            SetCookieManagerWithExistingCookie(new UserJourneyCookieService.UserJourneyModel());

        var service = new UserJourneyCookieService(NullLogger<UserJourneyCookieService>.Instance, mockCookieManager.cookieManager.Object);

        service.SetAwardingOrganisation("Awarding Organisation");
        service.SetUserSelectedQualificationFromList(YesOrNo.Yes);
        service.SetAdditionalQuestionsAnswers(new Dictionary<string, string> { { "1", "Answer 1" } });
        service.SetQualificationNameSearchCriteria("Search Criteria");
        service.SetLevelOfQualification("6");
        service.SetWhenWasQualificationStarted("12/2012");
        service.SetWhereWasQualificationAwarded("York");

        CheckSerializedModelWasSet(mockCookieManager,
                                   new UserJourneyCookieService.UserJourneyModel());
    }

    [TestMethod]
    public void SupportingMethod_ReturnsExpectedTestObjects()
    {
        var model = new UserJourneyCookieService.UserJourneyModel
                    {
                        AdditionalQuestionsAnswers = [],
                        SearchCriteria = "Search criteria",
                        WhenWasQualificationStarted = "12/2012",
                        WhereWasQualificationAwarded = "York",
                        LevelOfQualification = "6",
                        QualificationWasSelectedFromList = YesOrNo.Yes,
                        WhatIsTheAwardingOrganisation = "Awarding Organisation"
                    };

        var (manager, cookies) = SetCookieManagerWithExistingCookie(model);

        // Manager mock should be set up to return a dictionary containing the cookie with the cookie value passed in
        manager.Object.ReadInboundCookies().Should().BeOfType<Dictionary<string, string>>()
               .Which.Should().ContainKey(CookieKeyNames.UserJourneyKey);

        // Manager mock should be set up to accept any cookie and set it on the cookies dictionary
        manager.Object.SetOutboundCookie("X", "Y", new CookieOptions());

        cookies["X"].Should().Be("Y");
    }

    [TestMethod]
    public void ClearAdditionalQuestionsAnswersInternal_ClearValuesFromModel()
    {
        var mockCookieManager =
            SetCookieManagerWithExistingCookie(new UserJourneyCookieService.UserJourneyModel
                                               {
                                                   AdditionalQuestionsAnswers =
                                                       new Dictionary<string, string> { { "key", "value" } }
                                               });

        var mockLogger = new Mock<ILogger<UserJourneyCookieService>>();

        var service = new UserJourneyCookieService(mockLogger.Object, mockCookieManager.cookieManager.Object);

        service.ClearAdditionalQuestionsAnswers();

        CheckSerializedModelWasSet(mockCookieManager,
                                   new UserJourneyCookieService.UserJourneyModel());
    }

    private static (Mock<ICookieManager> cookieManager, Dictionary<string, string> cookies)
        SetCookieManagerWithExistingCookie(
            UserJourneyCookieService.UserJourneyModel? model)
    {
        var serializedModel = JsonSerializer.Serialize(model);

        var mockManager = new Mock<ICookieManager>();

        var cookiesReceived = new Dictionary<string, string>();

        if (model != null)
        {
            cookiesReceived.Add(CookieKeyNames.UserJourneyKey, serializedModel);
        }

        mockManager.Setup(m => m.ReadInboundCookies())
                   .Returns(cookiesReceived);

        var cookiesReturned = new Dictionary<string, string>();

        mockManager.Setup(m => m.SetOutboundCookie(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CookieOptions>()))
                   .Callback((string key, string value, CookieOptions _) => cookiesReturned[key] = value)
                   .Verifiable();

        return (mockManager, cookiesReturned);
    }

    private static void CheckSerializedModelWasSet(
        (Mock<ICookieManager> mockContext, Dictionary<string, string> cookies) cookies,
        UserJourneyCookieService.UserJourneyModel expectedModel)
    {
        var expectedCookieValue = JsonSerializer.Serialize(expectedModel);

        cookies.cookies.Should().ContainKey(CookieKeyNames.UserJourneyKey);
        cookies.cookies[CookieKeyNames.UserJourneyKey].Should().Be(expectedCookieValue);

        var in29Minutes = new DateTimeOffset(DateTime.Now.AddMinutes(29));
        var in30Minutes = new DateTimeOffset(DateTime.Now.AddMinutes(30));

        cookies.mockContext
               .Verify(m =>
                           m.SetOutboundCookie(CookieKeyNames.UserJourneyKey,
                                               expectedCookieValue,
                                               It.Is<CookieOptions>(
                                                                    options =>
                                                                        options.Secure
                                                                        && options.HttpOnly
                                                                        && options.Expires > in29Minutes
                                                                        && options.Expires < in30Minutes)
                                              ),
                       Times.Once);
    }
}