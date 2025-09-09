using Dfe.EarlyYearsQualification.Web.Models;

namespace Dfe.EarlyYearsQualification.UnitTests.Models;

[TestClass]
public class AdditionalInformationModelTests
{
    [TestMethod]
    public void AdditionalInformationModel_SetsInitialValues()
    {
        var model = new AdditionalInformationModel();

        model.Should().NotBeNull();

        model.AdditionalInformationBody.Should().BeEmpty();
        model.AdditionalInformationHeader.Should().BeEmpty();
        model.ShowAdditionalInformationBodyByDefault.Should().BeFalse();
    }
}