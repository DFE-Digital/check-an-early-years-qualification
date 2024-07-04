using System.Text.Json;
using Dfe.EarlyYearsQualification.Web.Constants;
using Dfe.EarlyYearsQualification.Web.Models;
using Dfe.EarlyYearsQualification.Web.Services.UserJourneyCookieService;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Moq;

namespace Dfe.EarlyYearsQualification.UnitTests.Services;

[TestClass]
public class UserJourneyCookieServiceTests
{
    [TestMethod]
    public void SetWhereWasQualificationAwarded_StringProvided_SetsCookieCorrectly()
    {
        var modelInCookie = new UserJourneyModel();
        var mockHttpContextAccessor = SetHttpContextWithExistingCookie(modelInCookie);
        var mockLogger = new Mock<ILogger<UserJourneyCookieService>>();

        var service = new UserJourneyCookieService(mockHttpContextAccessor.Object, mockLogger.Object);

        service.SetWhereWasQualificationAwarded("some test string");

        var serialisedModelToCheck = JsonSerializer.Serialize(new UserJourneyModel
                                                              {
                                                                  WhereWasQualificationAwarded = "some test string"
                                                              });

        CheckSerializedModelWasSet(mockHttpContextAccessor, serialisedModelToCheck);
    }

    [TestMethod]
    public void SetWhenWasQualificationAwarded_StringProvided_SetsCookieCorrectly()
    {
        var modelInCookie = new UserJourneyModel();
        var mockHttpContextAccessor = SetHttpContextWithExistingCookie(modelInCookie);
        var mockLogger = new Mock<ILogger<UserJourneyCookieService>>();

        var service = new UserJourneyCookieService(mockHttpContextAccessor.Object, mockLogger.Object);

        service.SetWhenWasQualificationAwarded("some test string");

        var serialisedModelToCheck = JsonSerializer.Serialize(new UserJourneyModel
                                                              {
                                                                  WhenWasQualificationAwarded = "some test string"
                                                              });

        CheckSerializedModelWasSet(mockHttpContextAccessor, serialisedModelToCheck);
    }

    [TestMethod]
    public void SetLevelOfQualification_StringProvided_SetsCookieCorrectly()
    {
        var modelInCookie = new UserJourneyModel();
        var mockHttpContextAccessor = SetHttpContextWithExistingCookie(modelInCookie);
        var mockLogger = new Mock<ILogger<UserJourneyCookieService>>();

        var service = new UserJourneyCookieService(mockHttpContextAccessor.Object, mockLogger.Object);

        service.SetLevelOfQualification("some test string");

        var serialisedModelToCheck = JsonSerializer.Serialize(new UserJourneyModel
                                                              {
                                                                  LevelOfQualification = "some test string"
                                                              });

        CheckSerializedModelWasSet(mockHttpContextAccessor, serialisedModelToCheck);
    }

    [TestMethod]
    public void SetAwardingOrganisation_StringProvided_SetsCookieCorrectly()
    {
        var modelInCookie = new UserJourneyModel();
        var mockHttpContextAccessor = SetHttpContextWithExistingCookie(modelInCookie);
        var mockLogger = new Mock<ILogger<UserJourneyCookieService>>();

        var service = new UserJourneyCookieService(mockHttpContextAccessor.Object, mockLogger.Object);

        service.SetAwardingOrganisation("some test string");

        var serialisedModelToCheck = JsonSerializer.Serialize(new UserJourneyModel
                                                              {
                                                                  WhatIsTheAwardingOrganisation = "some test string"
                                                              });

        CheckSerializedModelWasSet(mockHttpContextAccessor, serialisedModelToCheck);
    }

    [TestMethod]
    public void GetUserJourneyModelFromCookie_NoCookiePresent_SetsBaseModelAsCookie()
    {
        var mockHttpContextAccessor = SetHttpContextWithExistingCookie(null);
        var mockLogger = new Mock<ILogger<UserJourneyCookieService>>();

        var service = new UserJourneyCookieService(mockHttpContextAccessor.Object, mockLogger.Object);

        var model = service.GetUserJourneyModelFromCookie();

        model.LevelOfQualification.Should().BeEmpty();
        model.WhenWasQualificationAwarded.Should().BeEmpty();
        model.WhereWasQualificationAwarded.Should().BeEmpty();

        var serialisedModelToCheck = JsonSerializer.Serialize(new UserJourneyModel());
        CheckSerializedModelWasSet(mockHttpContextAccessor, serialisedModelToCheck);
    }

    [TestMethod]
    public void GetUserJourneyModelFromCookie_CookieModelSerializesOK_SetsCookieAsProvidedModel()
    {
        var existingModel = new UserJourneyModel
                            {
                                LevelOfQualification = "test level of qualification",
                                WhenWasQualificationAwarded = "test when was qualification awarded",
                                WhereWasQualificationAwarded = "test where was qualification awarded"
                            };
        var mockHttpContextAccessor = SetHttpContextWithExistingCookie(existingModel);
        var mockLogger = new Mock<ILogger<UserJourneyCookieService>>();

        var service = new UserJourneyCookieService(mockHttpContextAccessor.Object, mockLogger.Object);

        var model = service.GetUserJourneyModelFromCookie();

        model.LevelOfQualification.Should().Be(existingModel.LevelOfQualification);
        model.WhenWasQualificationAwarded.Should().Be(existingModel.WhenWasQualificationAwarded);
        model.WhereWasQualificationAwarded.Should().Be(existingModel.WhereWasQualificationAwarded);
    }

    [TestMethod]
    public void GetUserJourneyModelFromCookie_CookieModelDoesntSerialize_SetsBaseModelAsCookie()
    {
        var mockHttpContextAccessor = SetHttpContextWithExistingCookie("test failure");
        var mockLogger = new Mock<ILogger<UserJourneyCookieService>>();

        var service = new UserJourneyCookieService(mockHttpContextAccessor.Object, mockLogger.Object);

        var model = service.GetUserJourneyModelFromCookie();

        model.LevelOfQualification.Should().BeEmpty();
        model.WhenWasQualificationAwarded.Should().BeEmpty();
        model.WhereWasQualificationAwarded.Should().BeEmpty();

        var serialisedModelToCheck = JsonSerializer.Serialize(new UserJourneyModel());
        CheckSerializedModelWasSet(mockHttpContextAccessor, serialisedModelToCheck);
    }

    [TestMethod]
    public void ResetUserJourneyCookie_NoCookieSet_AddsBaseModelAsCookie()
    {
        var mockHttpContextAccessor = SetHttpContextWithExistingCookie(null);
        var mockLogger = new Mock<ILogger<UserJourneyCookieService>>();

        var service = new UserJourneyCookieService(mockHttpContextAccessor.Object, mockLogger.Object);

        service.ResetUserJourneyCookie();

        var serialisedModelToCheck = JsonSerializer.Serialize(new UserJourneyModel());
        CheckSerializedModelWasSet(mockHttpContextAccessor, serialisedModelToCheck);
    }

    [TestMethod]
    public void ResetUserJourneyCookie_FullModelAsCookieExists_AddsBaseModelAsCookie()
    {
        var model = new UserJourneyModel
                    {
                        LevelOfQualification = "test level of qualification",
                        WhenWasQualificationAwarded = "test when was qualification awarded",
                        WhereWasQualificationAwarded = "test where was qualification awarded"
                    };
        var mockHttpContextAccessor = SetHttpContextWithExistingCookie(model);
        var mockLogger = new Mock<ILogger<UserJourneyCookieService>>();

        var service = new UserJourneyCookieService(mockHttpContextAccessor.Object, mockLogger.Object);

        service.ResetUserJourneyCookie();

        var serialisedModelToCheck = JsonSerializer.Serialize(new UserJourneyModel());
        CheckSerializedModelWasSet(mockHttpContextAccessor, serialisedModelToCheck);
    }

    private static Mock<IHttpContextAccessor> SetHttpContextWithExistingCookie(object? model)
    {
        var serializedModel = JsonSerializer.Serialize(model);

        var requestCookiesMock = new Mock<IRequestCookieCollection>();
        var responseCookiesMock = new Mock<IResponseCookies>();

        if (model != null)
        {
            requestCookiesMock.Setup(cookiesCollection => cookiesCollection[CookieKeyNames.UserJourneyKey])
                              .Returns(serializedModel);
            responseCookiesMock.Setup(x => x.Delete(It.IsAny<string>())).Verifiable();
        }

        var httpContextMock = new Mock<IHttpContextAccessor>();
        httpContextMock.Setup(contextAccessor => contextAccessor.HttpContext!.Request.Cookies)
                       .Returns(requestCookiesMock.Object);
        httpContextMock.Setup(contextAccessor => contextAccessor.HttpContext!.Response.Cookies)
                       .Returns(responseCookiesMock.Object);
        return httpContextMock;
    }

    private static void CheckSerializedModelWasSet(Mock<IHttpContextAccessor> mockContext,
                                                   string serializedModelToCheck)
    {
        var in364Days = new DateTimeOffset(DateTime.Now.AddDays(364));
        var inOneYear = new DateTimeOffset(DateTime.Now.AddYears(1));

        mockContext
            .Verify(http =>
                        http.HttpContext!.Response.Cookies.Append(CookieKeyNames.UserJourneyKey,
                                                                  serializedModelToCheck,
                                                                  It.Is<CookieOptions>(
                                                                       options =>
                                                                           options.Secure
                                                                           && options.HttpOnly
                                                                           && options.Expires > in364Days
                                                                           && options.Expires < inOneYear)
                                                                 ),
                    Times.Once);
    }
}