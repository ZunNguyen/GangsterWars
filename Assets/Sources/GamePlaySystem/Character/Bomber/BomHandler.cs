using Cysharp.Threading.Tasks;
using Game.Character.Enemy;
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
        private GameData.GameData _gameData => Locator<GameData.GameData>.Instance;
        private UserProfile _userProfile => _gameData.GetProfileData<UserProfile>();

        private MainGamePlaySystem _mainGamePlaySystem => Locator<MainGamePlaySystem>.Instance;
        private BomberSystem _bomberSystem => Locator<BomberSystem>.Instance;

        private bool _outOfAmmo = false;
        private bool _isLose = false;
        private bool _outOfEnemies = false;

        public List<WeaponData> BomberModel { private set; get; } = new ();
        public ReactiveProperty<WeaponData> BomberModelCurrent = new ();
        public ReactiveProperty<EnemyController> EnemyTarget = new ();

        public void OnSetUp()
        {
            if (_userProfile.BomberDatas == null) return;

            BomberModel = _userProfile.BomberDatas;
            _bomberSystem.ReloadTimeHandler.CompleteReload += Start;
        }

        public void Start()
        {
            BomberModelCurrent.Value = GetRandomBomberModel();
            GetEnemyToAttack();
        }

        private bool IsEndBattle()
        {
            if (_outOfAmmo || _isLose || _outOfEnemies) return true;
            else return false;
        }

        private WeaponData GetRandomBomberModel()
        {
            var model = GetRandom.FromList(BomberModel);
            if (model.Quatity != 0)
            {
                return model;
            }
            return null;
        }

        private void GetEnemyToAttack()
        {
            if (_mainGamePlaySystem.SpawnEnemiesHandler.Enemies.Count == 0) return;
            EnemyTarget.Value = _mainGamePlaySystem.SpawnEnemiesHandler.Enemies[0];
        }

        public void EndActionThrow()
        {
            BomberModelCurrent.Value = null;
            EnemyTarget.Value = null;
            _bomberSystem.ReloadTimeHandler.Reloading();
        }

        private void OnDestroy()
        {
            _bomberSystem.ReloadTimeHandler.CompleteReload -= Start;
        }
    }
}