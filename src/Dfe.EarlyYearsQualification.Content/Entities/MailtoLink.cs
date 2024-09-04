using Contentful.Core.Models;

namespace Dfe.EarlyYearsQualification.Content.Entities;

public class MailtoLink
{
    public SystemProperties Sys { get; init; } = new();
    
    public string Text { get; init; } = string.Empty;

    public string Email { get; init; } = string.Empty;
}