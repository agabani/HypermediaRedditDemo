using System;
using System.Collections.Generic;
using System.Linq;

namespace HypermediaClient.Extensions
{
    public static class TimeSpanExtensions
    {
        public static string Readable(this TimeSpan timeSpan)
        {
            var cutoff = new SortedList<long, string>
            {
                {60, "{3:S}"},
                {60*60, "{2:M}, {3:S}"},
                {24*60*60, "{1:H}, {2:M}"},
                {long.MaxValue, "{0:D}, {1:H}"}
            };

            var find = cutoff.Keys.ToList().BinarySearch((long) timeSpan.TotalSeconds);
            var near = find < 0 ? Math.Abs(find) - 1 : find;

            return string.Format(
                new HmsFormatter(),
                cutoff[cutoff.Keys[near]],
                timeSpan.Days,
                timeSpan.Hours,
                timeSpan.Minutes,
                timeSpan.Seconds);
        }

        public class HmsFormatter : ICustomFormatter, IFormatProvider
        {
            private static readonly Dictionary<string, string> Timeformats = new Dictionary<string, string>
            {
                {"S", "{0:P:seconds:second}"},
                {"M", "{0:P:minutes:minute}"},
                {"H", "{0:P:hours:hour}"},
                {"D", "{0:P:days:day}"}
            };

            public string Format(string format, object arg, IFormatProvider formatProvider)
            {
                return string.Format(new PluralFormatter(), Timeformats[format], arg);
            }

            public object GetFormat(Type formatType)
            {
                return formatType == typeof(ICustomFormatter) ? this : null;
            }
        }

        public class PluralFormatter : ICustomFormatter, IFormatProvider
        {
            public string Format(string format, object arg, IFormatProvider formatProvider)
            {
                if (arg != null)
                {
                    var parts = format.Split(':');

                    if (parts[0] == "P")
                    {
                        var partIndex = arg.ToString() == "1" ? 2 : 1;
                        return $"{arg} {(parts.Length > partIndex ? parts[partIndex] : "")}";
                    }
                }
                return string.Format(format, arg);
            }

            public object GetFormat(Type formatType)
            {
                return formatType == typeof(ICustomFormatter) ? this : null;
            }
        }
    }
}