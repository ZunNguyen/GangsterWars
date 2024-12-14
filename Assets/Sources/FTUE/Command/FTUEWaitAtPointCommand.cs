using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;
using Sources.FTUE.System;
using Sources.Utils;
using Sources.Utils.Singleton;
using System.Collections;
using UnityEngine;

namespace Sources.FTUE.Command
{
    public class FTUEWaitAtPointCommand : FTUECommand
    {
        private FTUESystem _ftueSystem => Locator<FTUESystem>.Instance;

        [SerializeField, ValueDropdown(nameof(_getAllFTUEKey))]
        private string _waitPointKey;

        public override string Description => $"Wait at point {_waitPointKey}";

        public override async UniTask Execute()
        {
            await _ftueSystem.WaitForAtPoint(_waitPointKey);
        }
    }
}