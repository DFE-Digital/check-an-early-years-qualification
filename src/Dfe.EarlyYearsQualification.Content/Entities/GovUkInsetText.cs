using Contentful.Core.Models;
using Newtonsoft.Json.Linq;

namespace Dfe.EarlyYearsQualification.Content.Entities;

public class GovUkInsetTextModel
{
    public SystemProperties Sys { get; } = new();

    public string? Name { get; set; }

    public JObject? Content { get; init; }
}