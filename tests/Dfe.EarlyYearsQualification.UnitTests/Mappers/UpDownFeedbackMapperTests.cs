using Dfe.EarlyYearsQualification.Content.Entities;
using Dfe.EarlyYearsQualification.Mock.Helpers;
using Dfe.EarlyYearsQualification.Web.Mappers;

namespace Dfe.EarlyYearsQualification.UnitTests.Mappers;

[TestClass]
public class UpDownFeedbackMapperTests
{
    [TestMethod]
    public void Map_PassInNullUpDownFeedback_ReturnsNull()
    {
        var result = UpDownFeedbackMapper.Map(null, "test");

        result.Should().BeNull();
    }

    [TestMethod]
    public void Map_PassInNullUpDownFeedbackBody_ReturnsNull()
    {
        var result = UpDownFeedbackMapper.Map(new UpDownFeedback(), null);

        result.Should().BeNull();
    }

    [TestMethod]
    public void Map_PassInUpDownFeedbackAndBody_ReturnsModel()
    {
        const string improveServiceContent = "ImproveServiceContent";
        var upDownFeedback = new UpDownFeedback
                             {
                                 Question = "Question",
                                 YesButtonText = "YesButtonText",
                                 YesButtonSubText = "YesButtonSubText",
                                 NoButtonText = "NoButtonText",
                                 NoButtonSubText = "NoButtonSubText",
                                 HelpButtonText = "HelpButtonText",
                                 HelpButtonLink = "HelpButtonLink",
                                 CancelButtonText = "CancelButtonText",
                                 ImproveServiceContent = ContentfulContentHelper.Paragraph(improveServiceContent),
                                 UsefulResponse = "UsefulResponse"
                             };

        var result = UpDownFeedbackMapper.Map(upDownFeedback, improveServiceContent);

        upDownFeedback.ImproveServiceContent.Should().NotBeNull();
        result.Should().NotBeNull();
        result!.Question.Should().Be(upDownFeedback.Question);
        result.YesButtonText.Should().Be(upDownFeedback.YesButtonText);
        result.YesButtonSubText.Should().Be(upDownFeedback.YesButtonSubText);
        result.NoButtonText.Should().Be(upDownFeedback.NoButtonText);
        result.NoButtonSubText.Should().Be(upDownFeedback.NoButtonSubText);
        result.HelpButtonText.Should().Be(upDownFeedback.HelpButtonText);
        result.HelpButtonLink.Should().Be(upDownFeedback.HelpButtonLink);
        result.CancelButtonText.Should().Be(upDownFeedback.CancelButtonText);
        result.ImproveServiceBody.Should().Be(improveServiceContent);
        result.UsefulResponse.Should().Be(upDownFeedback.UsefulResponse);
    }
}