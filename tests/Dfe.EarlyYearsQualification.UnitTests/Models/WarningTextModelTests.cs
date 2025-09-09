using Dfe.EarlyYearsQualification.Web.Models;

namespace Dfe.EarlyYearsQualification.UnitTests.Mappers;

[TestClass]
public class WarningTextModelTests
{
    [TestMethod]
    public void WarningTextModel_SetsInitialValues()
    {
        var model = new WarningTextModel();

        model.Should().NotBeNull();

        model.WarningText.Should().BeEmpty();
        model.ReduceMargin.Should().BeFalse();
    }
}