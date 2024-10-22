using Imarat_Shariah.Services.Interfaces;

namespace Imarat_Shariah.Services
{
    public class TimeConversion : ITimeConversion
    {
        public DateTime GetIndianStandardTime(DateTime date)
        {
            TimeZoneInfo istZone = TimeZoneInfo.FindSystemTimeZoneById("Asia/Kolkata");
            return TimeZoneInfo.ConvertTimeFromUtc(date, istZone);
        }
    }
}
