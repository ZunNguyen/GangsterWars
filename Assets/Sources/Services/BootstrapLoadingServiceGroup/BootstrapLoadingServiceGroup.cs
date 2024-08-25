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
            _uiManager.Show<LoadingScreen>(Progress);

            await base.Excute();
            
            return IService.Result.Success;
        }
    }
}