using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wulkanizacja.Service.Application.Converters
{
    public class WeekYearToDateConverter
    {
        public DateTimeOffset ConvertWeekYearToDate(string weekYear)
        {
            if (string.IsNullOrWhiteSpace(weekYear))
            {
                throw new ArgumentException("Parametr 'weekYear' nie może być pusty ani null.", nameof(weekYear));
            }
            if (weekYear.Length != 4)
                throw new ArgumentException("Format tygodnia i roku powinien mieć 4 znaki, np. '5224'.", nameof(weekYear));

            int week = int.Parse(weekYear.Substring(0, 2));
            int shortYear = int.Parse(weekYear.Substring(2, 2));

            int year = 2000 + shortYear;

            DateTimeOffset date = ISOWeek.ToDateTime(year, week, DayOfWeek.Monday);
            return date;
        }
    }
}
