using Microsoft.Extensions.Logging;
using Moq;

namespace Dfe.EarlyYearsQualification.UnitTests.Helpers;

public static class MockLoggerExtensions
{
  public static void VerifyError<T>(this Mock<ILogger<T>> mockLogger, string message)
  {
    mockLogger.Verify(
    logger => logger.Log(
        LogLevel.Error,
        It.IsAny<EventId>(),
        It.Is<It.IsAnyType>((@object, _) => @object.ToString() == message),
        It.IsAny<Exception>(),
        It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
    Times.Once);
  }

  public static void VerifyWarning<T>(this Mock<ILogger<T>> mockLogger, string message)
  {
    mockLogger.Verify(
    logger => logger.Log(
        LogLevel.Warning,
        It.IsAny<EventId>(),
        It.Is<It.IsAnyType>((@object, _) => @object.ToString() == message),
        It.IsAny<Exception>(),
        It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
    Times.Once);
  }
}