using System;
using System.Collections;
using System.Globalization;
using System.IO;

/*
 * Based on example by Maarten Balliauw - https://blog.maartenballiauw.be/post/2017/08/01/building-a-scheduled-cache-updater-in-aspnet-core-2.html
 * Thank you!
 */
namespace Promitor.Scraper.Scheduling.Cron
{
    /// <summary>
    ///     Represents a single crontab field.
    /// </summary>
    [Serializable]
    public sealed class CrontabField
    {
        private readonly BitArray _bits;
        private readonly CrontabFieldImpl _impl;
        private /* readonly */ int _maxValueSet;
        private /* readonly */ int _minValueSet;

        private CrontabField(CrontabFieldImpl impl, string expression)
        {
            _impl = impl ?? throw new ArgumentNullException(nameof(impl));
            _bits = new BitArray(impl.ValueCount);

            _bits.SetAll(value: false);
            _minValueSet = int.MaxValue;
            _maxValueSet = -1;

            _impl.Parse(expression, Accumulate);
        }

        /// <summary>
        ///     Parses a crontab field expression representing days in any given month.
        /// </summary>
        public static CrontabField Days(string expression)
        {
            return new CrontabField(CrontabFieldImpl.Day, expression);
        }

        /// <summary>
        ///     Parses a crontab field expression representing days of a week.
        /// </summary>
        public static CrontabField DaysOfWeek(string expression)
        {
            return new CrontabField(CrontabFieldImpl.DayOfWeek, expression);
        }

        /// <summary>
        ///     Parses a crontab field expression representing hours.
        /// </summary>
        public static CrontabField Hours(string expression)
        {
            return new CrontabField(CrontabFieldImpl.Hour, expression);
        }

        /// <summary>
        ///     Parses a crontab field expression representing minutes.
        /// </summary>
        public static CrontabField Minutes(string expression)
        {
            return new CrontabField(CrontabFieldImpl.Minute, expression);
        }

        /// <summary>
        ///     Parses a crontab field expression representing months.
        /// </summary>
        public static CrontabField Months(string expression)
        {
            return new CrontabField(CrontabFieldImpl.Month, expression);
        }

        /// <summary>
        ///     Parses a crontab field expression given its kind.
        /// </summary>
        public static CrontabField Parse(CrontabFieldKind kind, string expression)
        {
            return new CrontabField(CrontabFieldImpl.FromKind(kind), expression);
        }

        public void Format(TextWriter writer)
        {
            Format(writer, noNames: false);
        }

        public void Format(TextWriter writer, bool noNames)
        {
            _impl.Format(this, writer, noNames);
        }

        public override string ToString()
        {
            return ToString(format: null);
        }

        public string ToString(string format)
        {
            var writer = new StringWriter(CultureInfo.InvariantCulture);

            switch (format)
            {
                case "G":
                case null:
                    Format(writer, noNames: true);
                    break;
                case "N":
                    Format(writer);
                    break;
                default:
                    throw new FormatException();
            }

            return writer.ToString();
        }

        /// <summary>
        ///     Accumulates the given range (start to end) and interval of values
        ///     into the current set of the field.
        /// </summary>
        /// <remarks>
        ///     To set the entire range of values representable by the field,
        ///     set
        ///     <param name="start" />
        ///     and
        ///     <param name="end" />
        ///     to -1 and
        ///     <param name="interval" />
        ///     to 1.
        /// </remarks>
        private void Accumulate(int start, int end, int interval)
        {
            var minValue = _impl.MinValue;
            var maxValue = _impl.MaxValue;

            if (start == end)
            {
                if (start < 0)
                {
                    //
                    // We're setting the entire range of values.
                    //

                    if (interval <= 1)
                    {
                        _minValueSet = minValue;
                        _maxValueSet = maxValue;
                        _bits.SetAll(value: true);
                        return;
                    }

                    start = minValue;
                    end = maxValue;
                }
                else
                {
                    //
                    // We're only setting a single value - check that it is in range.
                    //

                    if (start < minValue)
                    {
                        throw new FormatException(string.Format(
                            "'{0} is lower than the minimum allowable value for this field. Value must be between {1} and {2} (all inclusive).",
                            start, _impl.MinValue, _impl.MaxValue));
                    }

                    if (start > maxValue)
                    {
                        throw new FormatException(string.Format(
                            "'{0} is higher than the maximum allowable value for this field. Value must be between {1} and {2} (all inclusive).",
                            end, _impl.MinValue, _impl.MaxValue));
                    }
                }
            }
            else
            {
                //
                // For ranges, if the start is bigger than the end value then
                // swap them over.
                //

                if (start > end)
                {
                    end ^= start;
                    start ^= end;
                    end ^= start;
                }

                if (start < 0)
                {
                    start = minValue;
                }
                else if (start < minValue)
                {
                    throw new FormatException(string.Format(
                        "'{0} is lower than the minimum allowable value for this field. Value must be between {1} and {2} (all inclusive).",
                        start, _impl.MinValue, _impl.MaxValue));
                }

                if (end < 0)
                {
                    end = maxValue;
                }
                else if (end > maxValue)
                {
                    throw new FormatException(string.Format(
                        "'{0} is higher than the maximum allowable value for this field. Value must be between {1} and {2} (all inclusive).",
                        end, _impl.MinValue, _impl.MaxValue));
                }
            }

            if (interval < 1)
            {
                interval = 1;
            }

            int i;

            //
            // Populate the _bits table by setting all the bits corresponding to
            // the valid field values.
            //

            for (i = start - minValue; i <= end - minValue; i += interval)
            {
                _bits[i] = true;
            }

            //
            // Make sure we remember the minimum value set so far Keep track of
            // the highest and lowest values that have been added to this field
            // so far.
            //

            if (_minValueSet > start)
            {
                _minValueSet = start;
            }

            i += minValue - interval;

            if (_maxValueSet < i)
            {
                _maxValueSet = i;
            }
        }

        private int IndexToValue(int index)
        {
            return index + _impl.MinValue;
        }

        private int ValueToIndex(int value)
        {
            return value - _impl.MinValue;
        }

        #region ICrontabField Members

        /// <summary>
        ///     Gets the first value of the field or -1.
        /// </summary>
        public int GetFirst()
        {
            return _minValueSet < int.MaxValue ? _minValueSet : -1;
        }

        /// <summary>
        ///     Gets the next value of the field that occurs after the given
        ///     start value or -1 if there is no next value available.
        /// </summary>
        public int Next(int start)
        {
            if (start < _minValueSet)
            {
                return _minValueSet;
            }

            var startIndex = ValueToIndex(start);
            var lastIndex = ValueToIndex(_maxValueSet);

            for (var i = startIndex; i <= lastIndex; i++)
            {
                if (_bits[i])
                {
                    return IndexToValue(i);
                }
            }

            return -1;
        }

        /// <summary>
        ///     Determines if the given value occurs in the field.
        /// </summary>
        public bool Contains(int value)
        {
            return _bits[ValueToIndex(value)];
        }

        #endregion
    }
}