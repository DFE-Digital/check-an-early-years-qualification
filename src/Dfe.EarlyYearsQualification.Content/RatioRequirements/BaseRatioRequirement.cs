namespace Dfe.EarlyYearsQualification.Content.RatioRequirements;

public abstract class BaseRatioRequirement
{
    public bool FullAndRelevantForLevel2Before2014 { get; protected init; }

    public bool FullAndRelevantForLevel2After2014 { get; protected init; }

    public bool FullAndRelevantForLevel3Before2014 { get; protected init; }

    public bool FullAndRelevantForLevel3After2014 { get; protected init; }

    public bool FullAndRelevantForLevel4Before2014 { get; protected init; }

    public bool FullAndRelevantForLevel4After2014 { get; protected init; }

    public bool FullAndRelevantForLevel5Before2014 { get; protected init; }

    public bool FullAndRelevantForLevel5After2014 { get; protected init; }

    public bool FullAndRelevantForLevel6Before2014 { get; protected init; }

    public bool FullAndRelevantForLevel6After2014 { get; protected init; }

    public bool FullAndRelevantForQtsEtcBefore2014 { get; protected init; }

    public bool FullAndRelevantForQtsEtcAfter2014 { get; protected init; }

    public bool FullAndRelevantForLevel7Before2014 { get; protected init; }

    public bool FullAndRelevantForLevel7After2014 { get; protected init; }
}