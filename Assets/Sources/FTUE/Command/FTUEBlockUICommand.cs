using Cysharp.Threading.Tasks;
using Game.Screens.UIBlockerScreen;
using Sirenix.OdinInspector;
using Sources.UISystem;
using Sources.Utils.Singleton;
using System;
using UnityEngine;

namespace Sources.FTUE.Command
{
    [Serializable]
    public class FTUEBlockUICommand : FTUECommand
    {
        [SerializeField] private bool _isOpen;
        private UIManager _uiManager => Locator<UIManager>.Instance;

        public override string Description => $"Block UI";

        public override async UniTask Execute()
        {
            var uiBlocker = _uiManager.GetUIShowing<UIBlockerScreen>();

            if (_isOpen)
            {
                if (uiBlocker == null) await _uiManager.Show<UIBlockerScreen>();
            }
            else
            {
                if (uiBlocker != null) await _uiManager.Close<UIBlockerScreen>();
            }
        }
    }
}