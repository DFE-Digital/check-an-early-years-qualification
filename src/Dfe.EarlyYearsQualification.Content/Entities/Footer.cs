namespace Dfe.EarlyYearsQualification.Content.Entities;

public class Footer
{
    public FooterSection? LeftHandSideFooterSection { get; init; }
    
    public FooterSection? RightHandSideFooterSection { get; init; }

    public required List<NavigationLink> NavigationLinks { get; set; }
}