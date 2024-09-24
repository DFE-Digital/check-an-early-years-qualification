using Contentful.Core.Configuration;
using Dfe.EarlyYearsQualification.Content.Entities;

namespace Dfe.EarlyYearsQualification.Content.Resolvers;

public class EntityResolver : IContentTypeResolver
{
    private readonly Dictionary<string, Type> _types = new()
                                                       {
                                                           { "govUkInsetText", typeof(GovUkInsetTextModel) }
                                                       };

    public Type? Resolve(string contentTypeId)
    {
        return _types.GetValueOrDefault(contentTypeId);
    }
}