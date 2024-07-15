using Dfe.EarlyYearsQualification.Web.Models.Error;
using FluentAssertions;

namespace Dfe.EarlyYearsQualification.UnitTests.Models;

[TestClass]
public class ErrorViewModelTests
{
    [TestMethod]
    public void RequestId_IsNotEmpty_ShowRequestId_IsTrue()
    {
        var model = new ErrorViewModel { RequestId = "xyz" };

        model.ShowRequestId.Should().BeTrue();
    }

    [TestMethod]
    public void RequestId_IsNull_ShowRequestId_IsFalse()
    {
        var model = new ErrorViewModel { RequestId = null };

        model.ShowRequestId.Should().BeFalse();
    }

    [TestMethod]
    public void RequestId_IsEmpty_ShowRequestId_IsFalse()
    {
        var model = new ErrorViewModel { RequestId = string.Empty };

        model.ShowRequestId.Should().BeFalse();
    }

    [TestMethod]
    public void RequestId_IsWhitespace_ShowRequestId_IsFalse()
    {
        var model = new ErrorViewModel { RequestId = "   " };

        model.ShowRequestId.Should().BeFalse();
    }
}