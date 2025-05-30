using Dfe.EarlyYearsQualification.Content.Entities;
using Dfe.EarlyYearsQualification.Content.Resolvers;

namespace Dfe.EarlyYearsQualification.UnitTests.ContractResolvers;

[TestClass]
public class EntityResolverTests
{
    [TestMethod]
    public void Resolve_GovUkInsetText_ReturnsExpectedType()
    {
        var sut = new EntityResolver();

        var result = sut.Resolve("govUkInsetText");

        result.Should().NotBeNull();
        result.Should().Be<GovUkInsetTextModel>();
    }
    
    [TestMethod]
    public void Resolve_EmbeddedParagraph_ReturnsExpectedType()
    {
        var sut = new EntityResolver();

        var result = sut.Resolve("embeddedParagraph");

        result.Should().NotBeNull();
        result.Should().Be<EmbeddedParagraph>();
    }
    
    [TestMethod]
    public void Resolve_UnKnown_ReturnsNull()
    {
        var sut = new EntityResolver();

        var result = sut.Resolve("dummy");

        result.Should().BeNull();
    }
}