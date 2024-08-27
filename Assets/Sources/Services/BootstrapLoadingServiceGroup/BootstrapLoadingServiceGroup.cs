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
        public BootstrapLoadingServiceGroup(string nameServiceGroup) : base(nameServiceGroup) { }

        private UIManager _uiManager => Locator<UIManager>.Instance;

        public override async UniTask<IService.Result> Execute()
        {
            var loadingScreen = await _uiManager.Show<LoadingScreen>(Progress);

            await base.Execute();
            
            return IService.Result.Success;
        }
    }
}