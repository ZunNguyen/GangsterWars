using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sources.FTUE.Command
{
    public class FTUEWaitDelayTimeCommand : FTUECommand
    {
        [SerializeField] private int _delayMilisecond;

        public override string Description => $"Wait dealy time (milisecond): {_delayMilisecond}";

        public override async UniTask Execute()
        {
            await UniTask.Delay(_delayMilisecond);
        }
    }
}