using Sources.DataBaseSystem;
using Sources.GamePlaySystem.MainMenuGame;
using Sources.Utils.Singleton;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Screens.MainMenuScreen
{
    public class WeaponView : MonoBehaviour
    {
        private DataBase _dataBase => Locator<DataBase>.Instance;
        private StoreConfig _storeConfig => _dataBase.GetConfig<StoreConfig>();

        private StoreSystem _storeSystem => Locator<StoreSystem>.Instance;

        [SerializeField] private Image _icon;
        [SerializeField] private LevelUpgrade _levelUpgradePrefab;

        public void OnSetUp(string weaponId, string levelUpgradeId)
        {

        }
    }
}