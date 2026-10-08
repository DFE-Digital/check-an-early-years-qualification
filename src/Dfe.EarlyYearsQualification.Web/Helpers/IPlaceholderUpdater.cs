namespace Dfe.EarlyYearsQualification.Web.Helpers;

public interface IPlaceholderUpdater
{
    string Replace(string text);
    
    string Replace(string text,  string replacement);
}