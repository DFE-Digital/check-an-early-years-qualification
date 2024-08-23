namespace Dfe.EarlyYearsQualification.Web.Models;

public class UserJourneyModel
{
    public string WhereWasQualificationAwarded { get; set; } = string.Empty;
    public string WhenWasQualificationStarted { get; set; } = string.Empty;
    public string LevelOfQualification { get; set; } = string.Empty;
    public string WhatIsTheAwardingOrganisation { get; set; } = string.Empty;

    public string SearchCriteria { get; set; } = string.Empty;

    public Dictionary<string, string> AdditionalQuestionsAnswers { get; init; } = new();
}