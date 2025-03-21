namespace Dfe.EarlyYearsQualification.Web.Exceptions;

[Serializable]
public class DatesValidationException : Exception
{
    private const string ErrorMessage = "Failed to validate dates";

    public DatesValidationException() : base(ErrorMessage)
    {
    }

    public DatesValidationException(Exception innerException) : base(ErrorMessage, innerException)
    {
    }

    public DatesValidationException(Exception innerException, int? startedMonth, int? startedYear, int? awardedMonth, int? awardedYear) : base(ErrorMessage+$" (startedMonth:'{startedMonth}'|startedYear:'{startedYear}'|awardedMonth:'{awardedMonth}'|awardedYear:'{awardedYear}')", innerException)
    {
        StartedMonth = startedMonth;
        StartedYear = startedYear;
        AwardedMonth = awardedMonth;
        AwardedYear = awardedYear;
    }

    public int? StartedMonth { get; init; }
    public int? StartedYear { get; init; }
    public int? AwardedMonth { get; init; }
    public int? AwardedYear { get; init; }
}