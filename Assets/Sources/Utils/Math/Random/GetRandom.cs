using System;
using System.Collections.Generic;
using UnityEngine;

namespace Sources.Utils
{
    public static class GetRandom
    {
        private static readonly int _coinValueDefault = 0;
        private static readonly int _percentChanceMin = 0;
        private static readonly int _percentChanceMax = 100;

        private static System.Random _random = new System.Random();

        public static T FromList<T>(List<T> list)
        {
            if (list == null || list.Count == 0)
                throw new ArgumentException($"{list} is null or empty");

            int index = _random.Next(0, list.Count);
            return list[index];
        }

        public static int GetCoinRandom(Vector2Int valueRange, int percentChance)
        {
            var valueRandom = _random.Next(valueRange.x, valueRange.y);
            var percentChanceRandom = _random.Next(_percentChanceMin, _percentChanceMax);
            
            if (percentChanceRandom > percentChance) return _coinValueDefault;
            return valueRandom;
        }
    }
}