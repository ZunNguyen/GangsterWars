using Cysharp.Threading.Tasks;
using Sources.GamePlaySystem.Bomber;
using Sources.Utils.Singleton;
using UnityEngine;
using UniRx;
using Sources.DataBaseSystem;
using Sources.GamePlaySystem.MainGamePlay;

namespace Game.Character.Bomber
{
    public class ActionHandler : MonoBehaviour
    {
        private DataBase _dataBase => Locator<DataBase>.Instance;
        private BomberConfig _bomberConfig => _dataBase.GetConfig<BomberConfig>();
        private BomberSystem _bomberSystem => Locator<BomberSystem>.Instance;
        private MainGamePlaySystem _mainGamePlaySystem => Locator<MainGamePlaySystem>.Instance;

        [SerializeField] private SpriteRenderer _sprite;

        private void Awake()
        {
            _bomberSystem.BomHandler.BomberModelCurrent.Subscribe(value =>
            {
                if (value.BomId == null) return;
                GetIconWeapon(value.BomId);

            }).AddTo(this);

            _sprite.gameObject.SetActive(false);
        }

        private void GetIconWeapon(string bomId)
        {
            var bomInfo = _bomberConfig.GetWeaponInfo(bomId);
            _sprite.sprite = bomInfo.Icon;
        }

        public void Throwing()
        {
            var enemyTarget = _mainGamePlaySystem.SpawnEnemiesHandler.Enemies[0];

            Debug.Log(enemyTarget.name);
        }
    }
}