using Contentful.Core.Models;

namespace Dfe.EarlyYearsQualification.Content.Entities;

public class RatioRequirement
{
    public string RatioRequirementName { get; init; } = string.Empty;

    public bool FullAndRelevantForLevel2Before2014 { get; set; }

    public bool FullAndRelevantForLevel2After2014 { get; init; }

    public bool FullAndRelevantForLevel3Before2014 { get; set; }

    public bool FullAndRelevantForLevel3After2014 { get; init; }

    public bool FullAndRelevantForLevel4Before2014 { get; set; }

    public bool FullAndRelevantForLevel4After2014 { get; init; }

    public bool FullAndRelevantForLevel5Before2014 { get; set; }

    public bool FullAndRelevantForLevel5After2014 { get; init; }

    public bool FullAndRelevantForLevel6Before2014 { get; set; }

    public bool FullAndRelevantForLevel6After2014 { get; init; }

    public bool FullAndRelevantForQtsEtcBefore2014 { get; set; }

    public bool FullAndRelevantForQtsEtcAfter2014 { get; init; }

    public bool FullAndRelevantForLevel7Before2014 { get; set; }

    public bool FullAndRelevantForLevel7After2014 { get; init; }

    public string RequirementHeading { get; set; } = string.Empty;

    public Document? RequirementForLevel2Before2014 { get; set; }

    public Document? RequirementForLevel2After2014 { get; init; }

    public Document? RequirementForLevel3Before2014 { get; set; }

    public Document? RequirementForLevel3After2014 { get; set; }

    public Document? RequirementForLevel4Before2014 { get; set; }

    public Document? RequirementForLevel4After2014 { get; set; }

    public Document? RequirementForLevel5Before2014 { get; set; }

    public Document? RequirementForLevel5After2014 { get; set; }

    public Document? RequirementForLevel6Before2014 { get; set; }

    public Document? RequirementForLevel6After2014 { get; set; }

    public Document? RequirementForQtsEtcBefore2014 { get; init; }

    public Document? RequirementForQtsEtcAfter2014 { get; set; }

    public Document? RequirementForLevel7Before2014 { get; set; }

    public Document? RequirementForLevel7After2014 { get; set; }

    public Document? RequirementForLevel2BetweenSept14AndAug19 { get; init; }
}