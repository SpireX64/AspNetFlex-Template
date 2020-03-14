using System;
using System.Text.RegularExpressions;
using NodaTime;

namespace AspNetFlex.Domain.Infrastructure.Utils
{
    public static class DurationUtils
    {
        public static Duration FromString(string durationString)
        {
            const string regex = "^(\\d+d)?\\s*(\\d+h)?\\s*(\\d+m)?\\s*(\\d+s)?\\s*(\\d+ms)?$";
            var match = Regex.Match(durationString, regex);

            if (!match.Success)
                return Duration.Zero;

            var days = DirtyStringToInt(match.Groups[1].Value);
            var hours = DirtyStringToInt(match.Groups[2].Value);
            var minutes = DirtyStringToInt(match.Groups[3].Value);
            var seconds = DirtyStringToInt(match.Groups[4].Value);
            var milliseconds = DirtyStringToInt(match.Groups[5].Value);
            var time = new TimeSpan(days, hours, minutes, seconds, milliseconds);
            
            return Duration.FromTimeSpan(time);
        }

        private static int DirtyStringToInt(string dirtyString)
        {
            if (dirtyString == null)
                return 0;
            var cleanNumber = Regex.Replace(dirtyString, "[A-Za-z _]", "");
            return string.IsNullOrEmpty(cleanNumber) ? 0 : int.Parse(cleanNumber);
        }
    }
}