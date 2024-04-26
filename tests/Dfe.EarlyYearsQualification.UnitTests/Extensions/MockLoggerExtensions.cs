using Microsoft.Extensions.Logging;
using Moq;

namespace Dfe.EarlyYearsQualification.UnitTests.Helpers;

public static class MockLoggerExtensions
{
  public static void VerifyError<T>(this Mock<ILogger<T>> _logger, string message)
  {
    _logger.Verify(
    logger => logger.Log(
        It.Is<LogLevel>(logLevel => logLevel == LogLevel.Error),
        It.IsAny<EventId>(),
        It.Is<It.IsAnyType>((@object, @type) => @object.ToString() == message),
        It.IsAny<Exception>(),
        It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
    Times.Once);
  }
}