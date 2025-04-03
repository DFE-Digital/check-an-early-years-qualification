namespace Dfe.EarlyYearsQualification.Web.Models;

public class BannerError(string message, FieldId fieldId)
{
    public string Message { get; set; } = message;
    public FieldId FieldId { get; set; } = fieldId;
}