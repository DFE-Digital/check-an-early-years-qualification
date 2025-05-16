using Dfe.EarlyYearsQualification.Content.RatioRequirements;

namespace Dfe.EarlyYearsQualification.UnitTests.Ratios;

[TestClass]
public class RatioRequirementTests
{
    [TestMethod]
    public void UnqualifiedRatioRequirements_MatchesExpected()
    {
        var ratioRequirements = new UnqualifiedRatioRequirements();
        
        ratioRequirements.FullAndRelevantForLevel2Before2014.Should().BeTrue();
        ratioRequirements.FullAndRelevantForLevel2After2014.Should().BeTrue();
        ratioRequirements.FullAndRelevantForLevel3Before2014.Should().BeTrue();
        ratioRequirements.FullAndRelevantForLevel3After2014.Should().BeTrue();
        ratioRequirements.FullAndRelevantForLevel4Before2014.Should().BeTrue();
        ratioRequirements.FullAndRelevantForLevel4After2014.Should().BeTrue();
        ratioRequirements.FullAndRelevantForLevel5Before2014.Should().BeTrue();
        ratioRequirements.FullAndRelevantForLevel5After2014.Should().BeTrue();
        ratioRequirements.FullAndRelevantForLevel6Before2014.Should().BeTrue();
        ratioRequirements.FullAndRelevantForLevel6After2014.Should().BeTrue();
        ratioRequirements.FullAndRelevantForLevel7Before2014.Should().BeTrue();
        ratioRequirements.FullAndRelevantForLevel7After2014.Should().BeTrue();
        ratioRequirements.FullAndRelevantForQtsEtcBefore2014.Should().BeTrue();
        ratioRequirements.FullAndRelevantForQtsEtcAfter2014.Should().BeTrue();
    }
    
    [TestMethod]
    public void Level2RatioRequirements_MatchesExpected()
    {
        var ratioRequirements = new Level2RatioRequirements();
        
        ratioRequirements.FullAndRelevantForLevel2Before2014.Should().BeTrue();
        ratioRequirements.FullAndRelevantForLevel2After2014.Should().BeTrue();
        ratioRequirements.FullAndRelevantForLevel3Before2014.Should().BeTrue();
        ratioRequirements.FullAndRelevantForLevel3After2014.Should().BeTrue();
        ratioRequirements.FullAndRelevantForLevel4Before2014.Should().BeTrue();
        ratioRequirements.FullAndRelevantForLevel4After2014.Should().BeTrue();
        ratioRequirements.FullAndRelevantForLevel5Before2014.Should().BeTrue();
        ratioRequirements.FullAndRelevantForLevel5After2014.Should().BeTrue();
        ratioRequirements.FullAndRelevantForLevel6Before2014.Should().BeTrue();
        ratioRequirements.FullAndRelevantForLevel6After2014.Should().BeTrue();
        ratioRequirements.FullAndRelevantForLevel7Before2014.Should().BeTrue();
        ratioRequirements.FullAndRelevantForLevel7After2014.Should().BeTrue();
        ratioRequirements.FullAndRelevantForQtsEtcBefore2014.Should().BeTrue();
        ratioRequirements.FullAndRelevantForQtsEtcAfter2014.Should().BeTrue();
    }
    
    [TestMethod]
    public void Level3RatioRequirements_MatchesExpected()
    {
        var ratioRequirements = new Level3RatioRequirements();
        
        ratioRequirements.FullAndRelevantForLevel2Before2014.Should().BeFalse();
        ratioRequirements.FullAndRelevantForLevel2After2014.Should().BeFalse();
        ratioRequirements.FullAndRelevantForLevel3Before2014.Should().BeTrue();
        ratioRequirements.FullAndRelevantForLevel3After2014.Should().BeTrue();
        ratioRequirements.FullAndRelevantForLevel4Before2014.Should().BeTrue();
        ratioRequirements.FullAndRelevantForLevel4After2014.Should().BeTrue();
        ratioRequirements.FullAndRelevantForLevel5Before2014.Should().BeTrue();
        ratioRequirements.FullAndRelevantForLevel5After2014.Should().BeTrue();
        ratioRequirements.FullAndRelevantForLevel6Before2014.Should().BeTrue();
        ratioRequirements.FullAndRelevantForLevel6After2014.Should().BeTrue();
        ratioRequirements.FullAndRelevantForLevel7Before2014.Should().BeTrue();
        ratioRequirements.FullAndRelevantForLevel7After2014.Should().BeTrue();
        ratioRequirements.FullAndRelevantForQtsEtcBefore2014.Should().BeTrue();
        ratioRequirements.FullAndRelevantForQtsEtcAfter2014.Should().BeTrue();
    }
    
    [TestMethod]
    public void Level6RatioRequirements_MatchesExpected()
    {
        var ratioRequirements = new Level6RatioRequirements();
        
        ratioRequirements.FullAndRelevantForLevel2Before2014.Should().BeFalse();
        ratioRequirements.FullAndRelevantForLevel2After2014.Should().BeFalse();
        ratioRequirements.FullAndRelevantForLevel3Before2014.Should().BeFalse();
        ratioRequirements.FullAndRelevantForLevel3After2014.Should().BeFalse();
        ratioRequirements.FullAndRelevantForLevel4Before2014.Should().BeFalse();
        ratioRequirements.FullAndRelevantForLevel4After2014.Should().BeFalse();
        ratioRequirements.FullAndRelevantForLevel5Before2014.Should().BeFalse();
        ratioRequirements.FullAndRelevantForLevel5After2014.Should().BeFalse();
        ratioRequirements.FullAndRelevantForLevel6Before2014.Should().BeFalse();
        ratioRequirements.FullAndRelevantForLevel6After2014.Should().BeFalse();
        ratioRequirements.FullAndRelevantForLevel7Before2014.Should().BeFalse();
        ratioRequirements.FullAndRelevantForLevel7After2014.Should().BeFalse();
        ratioRequirements.FullAndRelevantForQtsEtcBefore2014.Should().BeTrue();
        ratioRequirements.FullAndRelevantForQtsEtcAfter2014.Should().BeTrue();
    }
}