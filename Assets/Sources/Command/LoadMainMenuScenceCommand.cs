using Cysharp.Threading.Tasks;
using Game.Screens.MainMenuScreen;
using Sources.Command;
using Sources.Services;
using Sources.UISystem;
using Sources.Utils.Singleton;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sources.Command
{
    public class LoadMainMenuScenceCommand : Command
    {
        private UIManager _uiManager => Locator<UIManager>.Instance;

        public override async UniTask Execute()
        {
            await _uiManager.Show<MainMenuScreen>();
        }
    }
}