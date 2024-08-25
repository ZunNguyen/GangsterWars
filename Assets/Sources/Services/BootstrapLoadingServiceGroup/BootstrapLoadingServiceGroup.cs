using Cysharp.Threading.Tasks;
using Game.Screens.LoadingScreen;
using Sources.UISystem;
using Sources.Utils.Singleton;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sources.Services.BootstrapLoadingService
{
    public class BootstrapLoadingServiceGroup : SequenceServiceGroup
    {
        private UIManager _uiManager => Locator<UIManager>.Instance;

        public override async UniTask<IService.Result> Excute()
        {
            await _uiManager.Show<LoadingScreen>(Progress);
            Debug.Log("1");

            await base.Excute();
            Debug.Log("2");

            await UniTask.Delay(10000);
            return IService.Result.Success;
        }
    }
}