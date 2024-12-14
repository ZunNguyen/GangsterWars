using Cysharp.Threading.Tasks;
using Game.Screens.FTUEScreen;
using Sirenix.OdinInspector;
using Sources.FTUE.System;
using Sources.UISystem;
using Sources.Utils;
using Sources.Utils.Singleton;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sources.FTUE.Command
{
    public class FTUETapToNextStepCommand : FTUECommand
    {
        private UIManager _uiManager => Locator<UIManager>.Instance;
        private FTUESystem _ftueSystem => Locator<FTUESystem>.Instance;

        [SerializeField, ValueDropdown(nameof(_getAllFTUEKey))]
        private string _waitNextToStepKey;

        public override string Description => $"Tap to Next Step";

        public override async UniTask Execute()
        {
            var uiFTUEScreen = _uiManager.GetUIShowing<FTUEScreen>() as FTUEScreen;
            if (uiFTUEScreen == null)
            {
                Debug.Log($"UI {uiFTUEScreen.name} not yet show");
                return;
            }

            uiFTUEScreen.SetClickToNext();
            await _ftueSystem.WaitTapToNextStep();
        }
    }
}