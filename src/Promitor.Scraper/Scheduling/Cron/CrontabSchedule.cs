using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

/*
 * Based on example by Maarten Balliauw - https://blog.maartenballiauw.be/post/2017/08/01/building-a-scheduled-cache-updater-in-aspnet-core-2.html
 * Thank you!
 */
namespace Promitor.Scraper.Scheduling.Cron
{
    /// <summary>
    ///     Represents a schedule initialized from the crontab expression.
    /// </summary>
    [Serializable]
    public sealed class CrontabSchedule
    {
        private static readonly char[] Separators = {' '};
        private readonly CrontabField _days;
        private readonly CrontabField _daysOfWeek;
        private readonly CrontabField _hours;
        private readonly CrontabField _minutes;
        private readonly CrontabField _months;

        private CrontabSchedule(string expression)
        {
            var fields = expression.Split(Separators, StringSplitOptions.RemoveEmptyEntries);

            if (fields.Length != 5)
            {
                throw new FormatException(string.Format(
                    "'{0}' is not a valid crontab expression. It must contain at least 5 components of a schedule "
                    + "(in the sequence of minutes, hours, days, months, days of week).",
                    expression));
            }

            _minutes = CrontabField.Minutes(fields[0]);
            _hours = CrontabField.Hours(fields[1]);
            _days = CrontabField.Days(fields[2]);
            _months = CrontabField.Months(fields[3]);
            _daysOfWeek = CrontabField.DaysOfWeek(fields[4]);
        }

        private static Calendar Calendar => CultureInfo.InvariantCulture.Calendar;

        public static CrontabSchedule Parse(string expression)
        {
            if (expression == null)
            {
                throw new ArgumentNullException(nameof(expression));
            }

            return new CrontabSchedule(expression);
        }

        public DateTime GetNextOccurrence(DateTime baseTime)
        {
            return GetNextOccurrence(baseTime, DateTime.MaxValue);
        }

        public DateTime GetNextOccurrence(DateTime baseTime, DateTime endTime)
        {
            const int nil = -1;

            var baseYear = baseTime.Year;
            var baseMonth = baseTime.Month;
            var baseDay = baseTime.Day;
            var baseHour = baseTime.Hour;
            var baseMinute = baseTime.Minute;

            var endYear = endTime.Year;
            var endMonth = endTime.Month;
            var endDay = endTime.Day;

            var year = baseYear;
            var month = baseMonth;
            var day = baseDay;
            var hour = baseHour;
            var minute = baseMinute + 1;

            //
            // Minute
            //

            minute = _minutes.Next(minute);

            if (minute == nil)
            {
                minute = _minutes.GetFirst();
                hour++;
            }

            //
            // Hour
            //

            hour = _hours.Next(hour);

            if (hour == nil)
            {
                minute = _minutes.GetFirst();
                hour = _hours.GetFirst();
                day++;
            }
            else if (hour > baseHour)
            {
                minute = _minutes.GetFirst();
            }

            //
            // Day
            //

            day = _days.Next(day);

            RetryDayMonth:

            if (day == nil)
            {
                minute = _minutes.GetFirst();
                hour = _hours.GetFirst();
                day = _days.GetFirst();
                month++;
            }
            else if (day > baseDay)
            {
                minute = _minutes.GetFirst();
                hour = _hours.GetFirst();
            }

            //
            // Month
            //

            month = _months.Next(month);

            if (month == nil)
            {
                minute = _minutes.GetFirst();
                hour = _hours.GetFirst();
                day = _days.GetFirst();
                month = _months.GetFirst();
                year++;
            }
            else if (month > baseMonth)
            {
                minute = _minutes.GetFirst();
                hour = _hours.GetFirst();
                day = _days.GetFirst();
            }

            //
            // The day field in a cron expression spans the entire range of days
            // in a month, which is from 1 to 31. However, the number of days in
            // a month tend to be variable depending on the month (and the year
            // in case of February). So a check is needed here to see if the
            // date is a border case. If the day happens to be beyond 28
            // (meaning that we're dealing with the suspicious range of 29-31)
            // and the date part has changed then we need to determine whether
            // the day still makes sense for the given year and month. If the
            // day is beyond the last possible value, then the day/month part
            // for the schedule is re-evaluated. So an expression like "0 0
            // 15,31 * *" will yield the following sequence starting on midnight
            // of Jan 1, 2000:
            //
            //  Jan 15, Jan 31, Feb 15, Mar 15, Apr 15, Apr 31, ...
            //

            var dateChanged = day != baseDay || month != baseMonth || year != baseYear;

            if (day > 28 && dateChanged && day > Calendar.GetDaysInMonth(year, month))
            {
                if (year >= endYear && month >= endMonth && day >= endDay)
                {
                    return endTime;
                }

                day = nil;
                goto RetryDayMonth;
            }

            var nextTime = new DateTime(year, month, day, hour, minute, second: 0, millisecond: 0, kind: baseTime.Kind);

            if (nextTime >= endTime)
            {
                return endTime;
            }

            //
            // Day of week
            //

            if (_daysOfWeek.Contains((int) nextTime.DayOfWeek))
            {
                return nextTime;
            }

            return GetNextOccurrence(new DateTime(year, month, day, hour: 23, minute: 59, second: 0, millisecond: 0, kind: baseTime.Kind), endTime);
        }

        public IEnumerable<DateTime> GetNextOccurrences(DateTime baseTime, DateTime endTime)
        {
            for (var occurrence = GetNextOccurrence(baseTime, endTime);
                occurrence < endTime;
                occurrence = GetNextOccurrence(occurrence, endTime))
            {
                yield return occurrence;
            }
        }

        public override string ToString()
        {
            var writer = new StringWriter(CultureInfo.InvariantCulture);

            _minutes.Format(writer, noNames: true);
            writer.Write(value: ' ');
            _hours.Format(writer, noNames: true);
            writer.Write(value: ' ');
            _days.Format(writer, noNames: true);
            writer.Write(value: ' ');
            _months.Format(writer, noNames: true);
            writer.Write(value: ' ');
            _daysOfWeek.Format(writer, noNames: true);

            return writer.ToString();
        }
    }
}