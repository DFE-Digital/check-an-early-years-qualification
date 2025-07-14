namespace Dfe.EarlyYearsQualification.Web.Models.Content;

public class FooterModel
{
    public FooterSectionModel? LeftHandSideFooterSection { get; set; }
    
    public FooterSectionModel? RightHandSideFooterSection { get; set; }

    public required List<NavigationLinkModel?> NavigationLinks { get; init; }
}