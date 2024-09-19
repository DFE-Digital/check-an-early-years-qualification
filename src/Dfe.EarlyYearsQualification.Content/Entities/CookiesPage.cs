using Contentful.Core.Models;

namespace Dfe.EarlyYearsQualification.Content.Entities;

public class CookiesPage
{
    public string Heading { get; init; } = string.Empty;

    public Document? Body { get; init; }

    public List<Option> Options { get; init; } = [];

    public string ButtonText { get; init; } = string.Empty;

    public string SuccessBannerHeading { get; init; } = string.Empty;

    public string ErrorText { get; init; } = string.Empty;

    public NavigationLink? BackButton { get; init; }

    public string FormHeading { get; init; } = string.Empty;

    private readonly Document? _successBannerContent;

    private const string NodeTypeName = "SuccessBannerContent";
    
    public Document? SuccessBannerContent
    {
        get => _successBannerContent;

        init
        {
            if (value == null)
            {
                _successBannerContent = null;
            }
            else
            {
                var newContent = new List<IContent>();
                
                foreach (var content in value.Content)
                {
                    if (content is not Paragraph para) continue;
                    para.NodeType = NodeTypeName;

                    newContent.Add(para);
                }
                
                _successBannerContent = new Document
                                        {
                                            NodeType = value.NodeType,
                                            Content = newContent,
                                            Data = value.Data
                                        };
            }
        }
    }
}