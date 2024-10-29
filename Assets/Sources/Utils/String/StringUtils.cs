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
                Debug.Log($"<color = red>{input}</color> have not <color = blue>{_specialCharacter_1}<color> or <color = blue>{_specialCharacter_2}</color>");
                return input;
            }

            return input.Substring(0, splitIndex);
        }
    }
}