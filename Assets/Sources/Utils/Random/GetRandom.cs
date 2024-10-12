using System;
using System.Collections.Generic;

namespace Sources.Utils
{
    public static class GetRandom
    {
        private static Random _random = new Random();

        public static T FromList<T>(List<T> list)
        {
            if (list == null || list.Count == 0)
                throw new ArgumentException($"{list} is null or empty");

            int index = _random.Next(0, list.Count);
            return list[index];
        }
    }
}