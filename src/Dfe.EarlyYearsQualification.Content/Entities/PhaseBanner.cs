using Contentful.Core.Models;

namespace Dfe.EarlyYearsQualification.Content.Entities;

public class PhaseBanner
{
    public string PhaseName { get; init; } = string.Empty;

    private readonly Document? _content;
    
    public Document? Content
    {
        get => _content;
        init
        {
            if (value == null)
            {
                _content = null;
            }
            else
            {
                var newContent = new List<IContent>();
                
                foreach (var content in value.Content)
                {
                    if (content is not Paragraph para) continue;
                    para.NodeType = nameof(PhaseBanner);

                    newContent.Add(para);
                }
                
                _content = new Document
                           {
                               NodeType = value.NodeType,
                               Content = newContent,
                               Data = value.Data
                           };
            }
        }
    }

    public bool Show { get; init; }
}