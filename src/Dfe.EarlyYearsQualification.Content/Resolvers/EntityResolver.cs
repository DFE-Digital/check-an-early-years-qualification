using Contentful.Core.Configuration;
using Dfe.EarlyYearsQualification.Content.Entities;

namespace Dfe.EarlyYearsQualification.Content.Resolvers;

public class EntityResolver : IContentTypeResolver
{
    /*
     * This is used to tell the Contentful serialiser how to deserialise custom embedded objects.
     * See here for more info: https://www.contentful.com/developers/docs/net/tutorials/rich-text/#working-with-custom-node-types
     */
    private readonly Dictionary<string, Type> _types = new()
                                                       {
                                                           { "govUkInsetText", typeof(GovUkInsetTextModel) },
                                                           { "embeddedParagraph", typeof(EmbeddedParagraph) }
                                                       };

    public Type? Resolve(string contentTypeId)
    {
        return _types.GetValueOrDefault(contentTypeId);
    }
}