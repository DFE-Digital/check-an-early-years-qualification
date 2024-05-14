using Contentful.Core.Models;
using Dfe.EarlyYearsQualification.Mock.Helpers;
using FluentAssertions;

namespace Dfe.EarlyYearsQualification.UnitTests.Mocks;

[TestClass]
public class ContentfulContentHelperTests
{
    [TestMethod]
    public void TextDocument_ReturnsDocumentWithSingleTextElement()
    {
        var document = ContentfulContentHelper.Text("Some text");

        document.Content.Should().ContainSingle(x => ((Text)x).Value == "Some text");
    }

    [TestMethod]
    public void ParagraphDocument_ReturnsDocumentWithSingleParagraphElementContainingSingleTextElement()
    {
        var document = ContentfulContentHelper.Paragraph("Some text");

        document.Content[0].Should().BeAssignableTo<Paragraph>()
                .Which.Content.Should().ContainSingle(x => ((Text)x).Value == "Some text");
    }
}