namespace Dfe.EarlyYearsQualification.Web.Services.DatesAndTimes;

public class DateTimeAdapter : IDateTimeAdapter
{
    public DateTime Now()
    {
        return DateTime.Now;
    }
}