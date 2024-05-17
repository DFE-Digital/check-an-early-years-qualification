using Dfe.EarlyYearsQualification.UnitTests.Extensions;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;

namespace Dfe.EarlyYearsQualification.UnitTests.Mocks;

[TestClass]
public class MockLoggerExtensionsTests
{
    [TestMethod]
    public void Logger_NotCalled_VerifyError_Fails()
    {
        var mockLogger = new Mock<ILogger<object>>();

        var exceptionWasCaught = false;

        try
        {
            // ReSharper disable once InvokeAsExtensionMethod
            MockLoggerExtensions.VerifyError(mockLogger, "Any message");
        }
        catch (MockException ex)
        {
            ex.Message.Should().Contain("Expected invocation");
            exceptionWasCaught = true;
        }

        exceptionWasCaught.Should().BeTrue();
    }

    [TestMethod]
    public void Logger_ErrorCalled_VerifyErrorWithExpectedMessage_Succeeds()
    {
        const string expectedMessage = "Error message";

        var mockLogger = new Mock<ILogger<object>>();
        mockLogger.Object.LogError(expectedMessage);

        // ReSharper disable once InvokeAsExtensionMethod
        MockLoggerExtensions.VerifyError(mockLogger, expectedMessage);
    }

    [TestMethod]
    public void Logger_ErrorCalled_VerifyErrorWithDifferentMessage_Fails()
    {
        const string expectedMessage = "Error message";

        var mockLogger = new Mock<ILogger<object>>();
        mockLogger.Object.LogError($"Not the {expectedMessage}");

        var exceptionWasCaught = false;

        try
        {
            // ReSharper disable once InvokeAsExtensionMethod
            MockLoggerExtensions.VerifyError(mockLogger, expectedMessage);
        }
        catch (MockException ex)
        {
            ex.Message.Should().Contain("Expected invocation");
            exceptionWasCaught = true;
        }

        exceptionWasCaught.Should().BeTrue();
    }

    [TestMethod]
    public void Logger_ErrorCalledTwice_VerifyErrorWithExpectedMessage_Fails()
    {
        const string expectedMessage = "Error message";

        var mockLogger = new Mock<ILogger<object>>();
        mockLogger.Object.LogError(expectedMessage);
        mockLogger.Object.LogError(expectedMessage);

        var exceptionWasCaught = false;

        try
        {
            // ReSharper disable once InvokeAsExtensionMethod
            MockLoggerExtensions.VerifyError(mockLogger, expectedMessage);
        }
        catch (MockException ex)
        {
            ex.Message
              .Should().Contain("Expected invocation")
              .And.Contain("was 2 times");

            exceptionWasCaught = true;
        }

        exceptionWasCaught.Should().BeTrue();
    }

    [TestMethod]
    public void Logger_WarningCalled_VerifyErrorWithExpectedMessage_Fails()
    {
        const string expectedMessage = "Warning message";

        var mockLogger = new Mock<ILogger<object>>();
        mockLogger.Object.LogWarning(expectedMessage);

        var exceptionWasCaught = false;

        try
        {
            // ReSharper disable once InvokeAsExtensionMethod
            MockLoggerExtensions.VerifyError(mockLogger, expectedMessage);
        }
        catch (MockException ex)
        {
            ex.Message.Should().Contain("Expected invocation");
            exceptionWasCaught = true;
        }

        exceptionWasCaught.Should().BeTrue();
    }

    [TestMethod]
    public void Logger_NotCalled_VerifyWarning_Fails()
    {
        var mockLogger = new Mock<ILogger<object>>();

        var exceptionWasCaught = false;

        try
        {
            // ReSharper disable once InvokeAsExtensionMethod
            MockLoggerExtensions.VerifyWarning(mockLogger, "Any message");
        }
        catch (MockException ex)
        {
            ex.Message.Should().Contain("Expected invocation");
            exceptionWasCaught = true;
        }

        exceptionWasCaught.Should().BeTrue();
    }

    [TestMethod]
    public void Logger_WarningCalled_VerifyWarningWithExpectedMessage_Succeeds()
    {
        const string expectedMessage = "Warning message";

        var mockLogger = new Mock<ILogger<object>>();
        mockLogger.Object.LogWarning(expectedMessage);

        // ReSharper disable once InvokeAsExtensionMethod
        MockLoggerExtensions.VerifyWarning(mockLogger, expectedMessage);
    }

    [TestMethod]
    public void Logger_WarningCalled_VerifyWarningWithDifferentMessage_Fails()
    {
        const string expectedMessage = "Warning message";

        var mockLogger = new Mock<ILogger<object>>();
        mockLogger.Object.LogWarning($"Not the {expectedMessage}");

        var exceptionWasCaught = false;

        try
        {
            // ReSharper disable once InvokeAsExtensionMethod
            MockLoggerExtensions.VerifyWarning(mockLogger, expectedMessage);
        }
        catch (MockException ex)
        {
            ex.Message.Should().Contain("Expected invocation");
            exceptionWasCaught = true;
        }

        exceptionWasCaught.Should().BeTrue();
    }

    [TestMethod]
    public void Logger_WarningCalledTwice_VerifyWarningWithExpectedMessage_Fails()
    {
        const string expectedMessage = "Warning message";

        var mockLogger = new Mock<ILogger<object>>();
        mockLogger.Object.LogWarning(expectedMessage);
        mockLogger.Object.LogWarning(expectedMessage);

        var exceptionWasCaught = false;

        try
        {
            // ReSharper disable once InvokeAsExtensionMethod
            MockLoggerExtensions.VerifyWarning(mockLogger, expectedMessage);
        }
        catch (MockException ex)
        {
            ex.Message
              .Should().Contain("Expected invocation")
              .And.Contain("was 2 times");

            exceptionWasCaught = true;
        }

        exceptionWasCaught.Should().BeTrue();
    }

    [TestMethod]
    public void Logger_ErrorCalled_VerifyWarningWithExpectedMessage_Fails()
    {
        const string expectedMessage = "Error message";

        var mockLogger = new Mock<ILogger<object>>();
        mockLogger.Object.LogError(expectedMessage);

        var exceptionWasCaught = false;

        try
        {
            // ReSharper disable once InvokeAsExtensionMethod
            MockLoggerExtensions.VerifyWarning(mockLogger, expectedMessage);
        }
        catch (MockException ex)
        {
            ex.Message.Should().Contain("Expected invocation");
            exceptionWasCaught = true;
        }

        exceptionWasCaught.Should().BeTrue();
    }
}