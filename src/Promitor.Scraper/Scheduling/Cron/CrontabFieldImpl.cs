using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Runtime.Serialization;

/*
 * Based on example by Maarten Balliauw - https://blog.maartenballiauw.be/post/2017/08/01/building-a-scheduled-cache-updater-in-aspnet-core-2.html
 * Thank you!
 */
namespace Promitor.Scraper.Scheduling.Cron
{
    [Serializable]
    public sealed class CrontabFieldImpl : IObjectReference
    {
        public static readonly CrontabFieldImpl Day = new CrontabFieldImpl(CrontabFieldKind.Day, minValue: 1, maxValue: 31, names: null);

        public static readonly CrontabFieldImpl DayOfWeek = new CrontabFieldImpl(CrontabFieldKind.DayOfWeek, minValue: 0, maxValue: 6,
            names: new[]
            {
                "Sunday", "Monday", "Tuesday",
                "Wednesday", "Thursday", "Friday",
                "Saturday"
            });

        public static readonly CrontabFieldImpl Hour = new CrontabFieldImpl(CrontabFieldKind.Hour, minValue: 0, maxValue: 23, names: null);
        public static readonly CrontabFieldImpl Minute = new CrontabFieldImpl(CrontabFieldKind.Minute, minValue: 0, maxValue: 59, names: null);

        public static readonly CrontabFieldImpl Month = new CrontabFieldImpl(CrontabFieldKind.Month, minValue: 1, maxValue: 12,
            names: new[]
            {
                "January", "February", "March", "April",
                "May", "June", "July", "August",
                "September", "October", "November",
                "December"
            });

        private static readonly char[] Comma = {','};

        private static readonly CompareInfo Comparer = CultureInfo.InvariantCulture.CompareInfo;

        private static readonly CrontabFieldImpl[] FieldByKind = {Minute, Hour, Day, Month, DayOfWeek};

        private readonly string[] _names;

        private CrontabFieldImpl(CrontabFieldKind kind, int minValue, int maxValue, string[] names)
        {
            Debug.Assert(Enum.IsDefined(typeof(CrontabFieldKind), kind));
            Debug.Assert(minValue >= 0);
            Debug.Assert(maxValue >= minValue);
            Debug.Assert(names == null || names.Length == maxValue - minValue + 1);

            Kind = kind;
            MinValue = minValue;
            MaxValue = maxValue;
            _names = names;
        }

        public CrontabFieldKind Kind { get; }

        public int MaxValue { get; }

        public int MinValue { get; }

        public int ValueCount => MaxValue - MinValue + 1;

        public static CrontabFieldImpl FromKind(CrontabFieldKind kind)
        {
            if (!Enum.IsDefined(typeof(CrontabFieldKind), kind))
            {
                throw new ArgumentException(string.Format(
                    "Invalid crontab field kind. Valid values are {0}.",
                    string.Join(", ", Enum.GetNames(typeof(CrontabFieldKind)))), nameof(kind));
            }

            return FieldByKind[(int) kind];
        }

        public void Format(CrontabField field, TextWriter writer, bool noNames)
        {
            if (field == null)
            {
                throw new ArgumentNullException(nameof(field));
            }

            if (writer == null)
            {
                throw new ArgumentNullException(nameof(writer));
            }

            var next = field.GetFirst();
            var count = 0;

            while (next != -1)
            {
                var first = next;
                int last;

                do
                {
                    last = next;
                    next = field.Next(last + 1);
                } while (next - last == 1);

                if (count == 0
                    && first == MinValue && last == MaxValue)
                {
                    writer.Write(value: '*');
                    return;
                }

                if (count > 0)
                {
                    writer.Write(value: ',');
                }

                if (first == last)
                {
                    FormatValue(first, writer, noNames);
                }
                else
                {
                    FormatValue(first, writer, noNames);
                    writer.Write(value: '-');
                    FormatValue(last, writer, noNames);
                }

                count++;
            }
        }

        public void Parse(string str, CrontabFieldAccumulator acc)
        {
            if (acc == null)
            {
                throw new ArgumentNullException(nameof(acc));
            }

            if (string.IsNullOrEmpty(str))
            {
                return;
            }

            try
            {
                InternalParse(str, acc);
            }
            catch (FormatException e)
            {
                ThrowParseException(e, str);
            }
        }

        private static void FastFormatNumericValue(int value, TextWriter writer)
        {
            Debug.Assert(value >= 0 && value < 100);
            Debug.Assert(writer != null);

            if (value >= 10)
            {
                writer.Write((char) ('0' + value / 10));
                writer.Write((char) ('0' + value % 10));
            }
            else
            {
                writer.Write((char) ('0' + value));
            }
        }

        private static void ThrowParseException(Exception innerException, string str)
        {
            Debug.Assert(str != null);
            Debug.Assert(innerException != null);

            throw new FormatException(string.Format("'{0}' is not a valid crontab field expression.", str),
                innerException);
        }

        private void FormatValue(int value, TextWriter writer, bool noNames)
        {
            Debug.Assert(writer != null);

            if (noNames || _names == null)
            {
                if (value >= 0 && value < 100)
                {
                    FastFormatNumericValue(value, writer);
                }
                else
                {
                    writer.Write(value.ToString(CultureInfo.InvariantCulture));
                }
            }
            else
            {
                var index = value - MinValue;
                writer.Write(_names[index]);
            }
        }

        #region IObjectReference Members

        object IObjectReference.GetRealObject(StreamingContext context)
        {
            return FromKind(Kind);
        }

        #endregion

        private void InternalParse(string str, CrontabFieldAccumulator acc)
        {
            Debug.Assert(str != null);
            Debug.Assert(acc != null);

            if (str.Length == 0)
            {
                throw new FormatException("A crontab field value cannot be empty.");
            }

            //
            // Next, look for a list of values (e.g. 1,2,3).
            //

            var commaIndex = str.IndexOf(",", StringComparison.Ordinal);

            if (commaIndex > 0)
            {
                foreach (var token in str.Split(Comma))
                {
                    InternalParse(token, acc);
                }
            }
            else
            {
                var every = 1;

                //
                // Look for stepping first (e.g. */2 = every 2nd).
                // 

                var slashIndex = str.IndexOf("/", StringComparison.Ordinal);

                if (slashIndex > 0)
                {
                    every = int.Parse(str.Substring(slashIndex + 1), CultureInfo.InvariantCulture);
                    str = str.Substring(startIndex: 0, length: slashIndex);
                }

                //
                // Next, look for wildcard (*).
                //

                if (str.Length == 1 && str[index: 0] == '*')
                {
                    acc(start: -1, end: -1, interval: every);
                    return;
                }

                //
                // Next, look for a range of values (e.g. 2-10).
                //

                var dashIndex = str.IndexOf("-", StringComparison.Ordinal);

                if (dashIndex > 0)
                {
                    var first = ParseValue(str.Substring(startIndex: 0, length: dashIndex));
                    var last = ParseValue(str.Substring(dashIndex + 1));

                    acc(first, last, every);
                    return;
                }

                //
                // Finally, handle the case where there is only one number.
                //

                var value = ParseValue(str);

                if (every == 1)
                {
                    acc(value, value, interval: 1);
                }
                else
                {
                    Debug.Assert(every != 0);

                    acc(value, MaxValue, every);
                }
            }
        }

        private int ParseValue(string str)
        {
            Debug.Assert(str != null);

            if (str.Length == 0)
            {
                throw new FormatException("A crontab field value cannot be empty.");
            }

            var firstChar = str[index: 0];

            if (firstChar >= '0' && firstChar <= '9')
            {
                return int.Parse(str, CultureInfo.InvariantCulture);
            }

            if (_names == null)
            {
                throw new FormatException(string.Format(
                    "'{0}' is not a valid value for this crontab field. It must be a numeric value between {1} and {2} (all inclusive).",
                    str, MinValue, MaxValue));
            }

            for (var i = 0; i < _names.Length; i++)
            {
                if (Comparer.IsPrefix(_names[i], str, CompareOptions.IgnoreCase))
                {
                    return i + MinValue;
                }
            }

            throw new FormatException(string.Format(
                "'{0}' is not a known value name. Use one of the following: {1}.",
                str, string.Join(", ", _names)));
        }
    }
}