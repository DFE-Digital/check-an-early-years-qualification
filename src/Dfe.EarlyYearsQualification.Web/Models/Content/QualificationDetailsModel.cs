namespace Dfe.EarlyYearsQualification.Web.Models.Content;

public class QualificationDetailsModel : BasicQualificationModel
{
    public string? FromWhichYear { get; init; }

    public NavigationLinkModel? BackButton { get; init; }

    public DetailsPageModel? Content { get; init; }

    public List<AdditionalRequirementAnswerModel>? AdditionalRequirementAnswers { get; init; }

    public RatioRequirementModel RatioRequirements { get; set; } = new();

    public UpDownFeedbackModel? UpDownFeedback { get; init; }

    public string DateStarted { get; init; } = string.Empty;

    public string DateAwarded { get; init; } = string.Empty;

    public string QualificationNumberLabel { get; init; } = string.Empty;

    public List<RatioRowModel> OrderRatioRows()
    {
        var ratioRowModels = new List<RatioRowModel>
                             {
                                 new RatioRowModel
                                 {
                                     Level = 6,
                                     LevelText = "Level 6",
                                     ApprovalStatus = RatioRequirements.ApprovedForLevel6,
                                     SummaryCardBody = RatioRequirements.RequirementsForLevel6
                                 },
                                 new RatioRowModel
                                 {
                                     Level = 3,
                                     LevelText = "Level 3",
                                     ApprovalStatus = RatioRequirements.ApprovedForLevel3,
                                     SummaryCardBody = RatioRequirements.RequirementsForLevel3
                                 },
                                 new RatioRowModel
                                 {
                                     Level = 2,
                                     LevelText = "Level 2",
                                     ApprovalStatus = RatioRequirements.ApprovedForLevel2,
                                     SummaryCardBody = RatioRequirements.RequirementsForLevel2
                                 },
                                 new RatioRowModel
                                 {
                                     Level = 0,
                                     LevelText = "Unqualified",
                                     ApprovalStatus = RatioRequirements.ApprovedForUnqualified,
                                     SummaryCardBody = RatioRequirements.RequirementsForUnqualified
                                 }
                             };

        var approvedRows = ratioRowModels.Where(x => x.ApprovalStatus == QualificationApprovalStatus.Approved)
                                         .OrderByDescending(x => x.Level);

        var nonApprovedRows = ratioRowModels.Where(x => x.ApprovalStatus != QualificationApprovalStatus.Approved)
                                            .OrderBy(x => x.Level);

        return approvedRows.Concat(nonApprovedRows).ToList();
    }

    public QualificationResultModel QualificationResultModel => new()
                                                                {
                                                                    Heading = Content!
                                                                        .QualificationResultHeading,
                                                                    MessageHeading =
                                                                        Content
                                                                            .QualificationResultMessageHeading,
                                                                    MessageBody =
                                                                        Content
                                                                            .QualificationResultMessageBody,
                                                                    IsFullAndRelevant =
                                                                        !RatioRequirements
                                                                            .IsNotFullAndRelevant
                                                                };
}