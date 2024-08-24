using Cysharp.Threading.Tasks;
using Sources.Services;
using Sources.Utils.Singleton;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Sources.UISystem
{
    public class InitUISystemService : Service
    {
        private readonly UIData _uiData;
        private readonly UIManager _uiManagerPrefab;

        public InitUISystemService(UIData uiData, UIManager uiManagerPrefab)
        {
            _uiData = uiData;
            _uiManagerPrefab = uiManagerPrefab;
        }

        public override async UniTask<IService.Result> Excute()
        {
            var uiManager = Object.Instantiate(_uiManagerPrefab);
            uiManager.name = "UI Manager";

            Locator<UIData>.Set(_uiData);
            Locator<UIManager>.Set(_uiManagerPrefab);

            uiManager.Initialize();

            //Object.DontDestroyOnLoad(uiManager);

            return IService.Result.Success;
        }
    }
}