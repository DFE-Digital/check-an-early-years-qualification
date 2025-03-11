using Contentful.Core.Models;

namespace Dfe.EarlyYearsQualification.Content.Entities;

public class HelpConfirmationPage
{
    public string SuccessMessage { get; init; } = string.Empty;

    public Document Body { get; init; } = new();
}