using Dfe.EarlyYearsQualification.Web.Services.DatesAndTimes;

namespace Dfe.EarlyYearsQualification.Web.Helpers;

public class PlaceholderUpdater(IDateTimeAdapter dateTimeAdapter) : IPlaceholderUpdater
{
    
    private readonly Dictionary<string, string> _placeholders = new()
                                                                {
                                                                    { "$[actual-year]$", dateTimeAdapter.Now().Year.ToString() }
                                                                };

    public string Replace(string text)
    {
        if (string.IsNullOrEmpty(text)) return text;
        
        var result = text;
        foreach (var placeholder in _placeholders)
        {
            if (text.Contains(placeholder.Key))
            {
                result = result.Replace(placeholder.Key, placeholder.Value);
            }
        }

        return result;
    }
}