using Cysharp.Threading.Tasks;
using Sources.GamePlaySystem.Bomber;
using Sources.Utils.Singleton;
using UnityEngine;
using UniRx;
using Sources.DataBaseSystem;
using Sources.GamePlaySystem.MainGamePlay;
using Sources.SpawnerSystem;

namespace Game.Character.Bomber
{
    public class ActionHandler : MonoBehaviour
    {
        private DataBase _dataBase => Locator<DataBase>.Instance;
        private BomberConfig _bomberConfig => _dataBase.GetConfig<BomberConfig>();
        private BomberSystem _bomberSystem => Locator<BomberSystem>.Instance;
        private MainGamePlaySystem _mainGamePlaySystem => Locator<MainGamePlaySystem>.Instance;
        private SpawnerManager _spawnerManager => Locator<SpawnerManager>.Instance;

        private Weapon _weapon;

        [SerializeField] private Weapon _weaponPrefab;
        [SerializeField] private Transform _weaponPos;

        private async void Awake()
        {
            await UniTask.Delay(3000);

            _bomberSystem.BomHandler.Start();

            await UniTask.DelayFrame(1);

            _bomberSystem.BomHandler.BomberModelCurrent.Subscribe(value =>
            {
                if (value == null) return;
                _weapon = _spawnerManager.Get(_weaponPrefab);
                _weapon.GetIconWeapon(value.BomId);
                _weapon.transform.position = _weaponPos.position;
                _weapon.gameObject.SetActive(false);

            }).AddTo(this);
        }

        public void Throwing()
        {
            var enemyTarget = _mainGamePlaySystem.SpawnEnemiesHandler.Enemies[0];
            _weapon.gameObject.SetActive(true);
            _weapon.ThrowBomb(enemyTarget.transform.position);
            _bomberSystem.BomHandler.EndActionThrow();

            Debug.Log(enemyTarget.name);
        }
    }
}