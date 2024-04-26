namespace Dfe.EarlyYearsQualification.Web.Models.Content;

public class CookiesPageModel
{
  public string Heading { get; init; } = string.Empty;

  public string BodyContent { get; init; } = string.Empty;

  public List<OptionModel> Options { get; set; } = [];

  public string ButtonText { get; init; } = string.Empty;
}
