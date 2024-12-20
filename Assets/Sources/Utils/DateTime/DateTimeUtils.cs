using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sources.Utils.DateTime
{
    public static class DateTimeUtils
    {
        public static string ChangeSecondToDateTime(int seconds)
        {
            TimeSpan timeSpan = TimeSpan.FromSeconds(seconds);
            return timeSpan.ToString(@"hh\:mm\:ss");
        }
    }
}