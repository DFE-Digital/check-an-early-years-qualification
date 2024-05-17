using Microsoft.Extensions.Logging;
using Moq;

namespace Dfe.EarlyYearsQualification.UnitTests.Extensions;

public static class MockLoggerExtensions
{
    public static void VerifyError<T>(this Mock<ILogger<T>> mockLogger, string expectedMessage)
    {
        Verify(mockLogger, LogLevel.Error, expectedMessage, Times.Once);
    }

    public static void VerifyWarning<T>(this Mock<ILogger<T>> mockLogger, string expectedMessage)
    {
        Verify(mockLogger, LogLevel.Warning, expectedMessage, Times.Once);
    }

    private static void Verify<T>(Mock<ILogger<T>> mockLogger,
                                  LogLevel expectedLevel,
                                  string expectedMessage,
                                  Func<Times> expectedTimes)
    {
        mockLogger
            .Verify(
                    logger => logger.Log(
                                         expectedLevel,
                                         It.IsAny<EventId>(),
                                         It.Is<It.IsAnyType>((@object, _) =>
                                                                 @object.ToString() == expectedMessage),
                                         It.IsAny<Exception>(),
                                         It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
                    expectedTimes);
    }
}