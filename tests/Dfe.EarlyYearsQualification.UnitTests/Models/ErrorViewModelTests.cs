using Dfe.EarlyYearsQualification.Web.Models;
using FluentAssertions;

namespace Dfe.EarlyYearsQualification.UnitTests.Models;

[TestClass]
public class ErrorViewModelTests
{
    [TestMethod]
    public void RequestIdSet_ShowRequestId_IsTrue()
    {
        var model = new ErrorViewModel { RequestId = "xyz" };

        model.ShowRequestId.Should().BeTrue();
    }

    [TestMethod]
    public void RequestIdNotNull_ShowRequestId_IsFalse()
    {
        var model = new ErrorViewModel { RequestId = null };

        model.ShowRequestId.Should().BeFalse();
    }

    [TestMethod]
    public void RequestIdEmpty_ShowRequestId_IsFalse()
    {
        var model = new ErrorViewModel { RequestId = string.Empty };

        model.ShowRequestId.Should().BeFalse();
    }

    [TestMethod]
    public void RequestIdWhitespace_ShowRequestId_IsFalse()
    {
        var model = new ErrorViewModel { RequestId = "   " };

        model.ShowRequestId.Should().BeFalse();
    }
    
}