using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sources.Utils.String
{
    public static class StringUtils
    {
        private static readonly string _specialCharacter_1 = "_";
        private static readonly string _specialCharacter_2 = "-";
        
        public static string GetBaseName(string input)
        {
            var indexSpecialCharacter_1 = input.IndexOf(_specialCharacter_1);
            var indexSpecialCharacter_2 = input.IndexOf(_specialCharacter_2);

            var splitIndex = indexSpecialCharacter_1 >= 0
                ? (indexSpecialCharacter_2 >= 0 ? Math.Min(indexSpecialCharacter_1, indexSpecialCharacter_2) : indexSpecialCharacter_1)
                : indexSpecialCharacter_2;

            if (splitIndex <= 0)
            {
                return input;
            }

            return input.Substring(0, splitIndex);
        }

        private static readonly string _specialCharacterSeparate_1 = "-";
        public static string[] SeparateString_1(string input)
        {
            return input.Split(new[] { _specialCharacterSeparate_1 }, StringSplitOptions.RemoveEmptyEntries);
        }

        private static readonly string _specialCharacterSeparate_2 = "&";
        public static string[] SeparateString_2(string input)
        {
            return input.Split(new[] { _specialCharacterSeparate_2 }, StringSplitOptions.RemoveEmptyEntries);
        }
    }
}