using System.ComponentModel.DataAnnotations;
using Dfe.EarlyYearsQualification.Content.Entities;
using Dfe.EarlyYearsQualification.Web.Attributes;

namespace Dfe.EarlyYearsQualification.Web.Models.Content;

public class CheckAdditionalRequirementsPageModel
{
    [Required]
    public string QualificationId { get; set; } = string.Empty;
    
    public string Heading { get; set; } = string.Empty;

    public string QualificationLabel { get; set; } = string.Empty;

    public string QualificationName { get; set; } = string.Empty;

    public string QualificationLevelLabel { get; set; } = string.Empty;

    public int QualificationLevel { get; set; }

    public string AwardingOrganisationLabel { get; set; } = string.Empty;

    public string AwardingOrganisation { get; set; } = string.Empty;

    public string QuestionSectionHeading { get; set; } = string.Empty;

    public string InformationMessage { get; set; } = string.Empty;

    public string CtaButtonText { get; set; } = string.Empty;
    
    public NavigationLink? BackButton { get; set; }

    public List<AdditionalRequirementQuestionModel> AdditionalRequirementQuestions { get; set; } = [];

    [AnswerValidation]
    // ReSharper disable once PropertyCanBeMadeInitOnly.Global
    public Dictionary<string, string> Answers { get; set; } = [];
    
    public bool HasErrors { get; set; }

    public string ErrorMessage { get; set; } = string.Empty;
}