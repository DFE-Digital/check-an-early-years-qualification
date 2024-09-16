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
                    // TODO: Check that instance of IContent contains node type property instead of casting to paragraph
                    if (content is not Paragraph test) continue;
                    test.NodeType = "PhaseBanner";

                    newContent.Add(test);
                }
                
                _content = new Document()
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