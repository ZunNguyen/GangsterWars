using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sources.Utils.Singleton
{
    public static class Locator<T>
    {
        private static T _instance;
        public static T Instance => _instance;

        public static T Set(T ins)
        {
            if (ins == null)
            {
                Debug.LogError($"<color=red>{typeof(T).Name}</color> is null");
            }

            if (_instance == null)
            {
                Locator<T>._instance = ins;
            }
            else
            {
                Debug.LogWarning("Instance already set. Please check and remove the SetInstance method");
            }

            return Locator<T>.Instance;
        }
    }
}