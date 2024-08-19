namespace Dfe.EarlyYearsQualification.Web.Helpers;

public class PlaceholderUpdater : IPlaceholderUpdater
{
    private readonly Dictionary<string, string> _placeholders = new()
                                                                {
                                                                    { "$[actual-year]$", DateTimeOffset.Now.Year.ToString() }
                                                                };

    public string Replace(string valueToCheck)
    {
        if (string.IsNullOrEmpty(valueToCheck)) return valueToCheck;
        
        var result = valueToCheck;
        foreach (var placeholder in _placeholders)
        {
            if (valueToCheck.Contains(placeholder.Key))
            {
                result = result.Replace(placeholder.Key, placeholder.Value);
            }
        }

        return result;
    }
}