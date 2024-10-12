using Sources.GameData;
using Sources.GamePlaySystem.MainGamePlay;
using Sources.Utils;
using Sources.Utils.Singleton;
using System.Collections.Generic;
using UniRx;
using Unity.VisualScripting;

namespace Sources.GamePlaySystem.Bomber
{
    public class BomHandler
    {
        private MainGamePlaySystem _mainGamePlaySystem => Locator<MainGamePlaySystem>.Instance;
        private GameData.GameData _gameData => Locator<GameData.GameData>.Instance;
        private BomberSystem _bomberSystem => Locator<BomberSystem>.Instance;

        private bool _outOfAmmo = false;
        private bool _isLose = false;
        private bool _outOfEnemies = false;

        public List<BomberData> BomberModel { private set; get; } = new ();
        public ReactiveProperty<BomberData> BomberModelCurrent = new ();

        public void OnSetUp()
        {
            BomberModel = _gameData.UserData.BomberData;
            _bomberSystem.ReloadTimeHandler.CompleteReload += Start;
        }

        public void Start()
        {
            BomberModelCurrent.Value = GetRandomBomberModel();
        }

        private bool IsEndBattle()
        {
            if (_outOfAmmo || _isLose || _outOfEnemies) return true;
            else return false;
        }

        private BomberData GetRandomBomberModel()
        {
            while (true)
            {
                var model = GetRandom.FromList(BomberModel);
                if (model.Quatity.Value != 0)
                {
                    return model;
                }
            }
        }

        public void EndActionThrow()
        {
            BomberModelCurrent.Value.BomId = null;
            _bomberSystem.ReloadTimeHandler.Reloading();
        }

        private void OnDestroy()
        {
            _bomberSystem.ReloadTimeHandler.CompleteReload -= Start;
        }
    }
}