using System;
using System.Collections.Generic;

namespace Sources.Utils
{
    public static class ShortNumber
    {
        private static Dictionary<int, string> _suffixes = new Dictionary<int, string>
        {
            { 1000, "K" },
            { 1000000, "M" },
            { 1000000000, "B" },
        };

        public static string Get(int value)
        {
            if (value < 1000) return value.ToString();

            foreach (var suffix in _suffixes)
            {
                if (value >= suffix.Key)
                {
                    var shortValue = (float)value / suffix.Key;
                    shortValue = (float)Math.Floor(shortValue * 10f) / 10f;
                    var format = shortValue >= 100 ? "0" : "0.0";
                    return shortValue.ToString(format) + suffix.Value;
                }
            }

            return value.ToString();
        }

        public static string GetShorter(int value)
        {
            if (value < 1000) return value.ToString();

            foreach (var suffix in _suffixes)
            {
                if (value >= suffix.Key)
                {
                    var shortValue = (float)value / suffix.Key;
                    shortValue = (float)Math.Floor(shortValue * 10f) / 10f;
                    var format = "0";
                    return shortValue.ToString(format) + suffix.Value;
                }
            }

            return value.ToString();
        }
    }
}