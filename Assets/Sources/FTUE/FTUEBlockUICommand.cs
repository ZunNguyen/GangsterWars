using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sources.FTUE
{
    [Serializable]
    public class FTUEBlockUICommand : FTUECommand
    {
        public override string Description => GetType().Name;

        public override UniTask Execute()
        {
            throw new System.NotImplementedException();
        }
    }
}