namespace Dfe.EarlyYearsQualification.Web.Models.List;

public class ListModel
{
    public List<QualificationModel> Pre2014Qualifications { get; set; } = [];
    
    public List<QualificationModel> Post2014Qualifications { get; set; } = [];
    
    public List<QualificationModel> Post2024Qualifications { get; set; } = [];

    public string? SearchTerm { get; set; }

    public int[]? Levels { get; set; }

    public string? CopyLink { get; set; }

    public void OrderQualificationLists()
    {
        Pre2014Qualifications = Pre2014Qualifications.OrderBy(q => q.QualificationLevel).ThenBy(n => n.QualificationName).ToList();
        Post2014Qualifications = Post2014Qualifications.OrderBy(q => q.QualificationLevel).ThenBy(n => n.QualificationName).ToList();
        Post2024Qualifications = Post2024Qualifications.OrderBy(q => q.QualificationLevel).ThenBy(n => n.QualificationName).ToList();
    }
}