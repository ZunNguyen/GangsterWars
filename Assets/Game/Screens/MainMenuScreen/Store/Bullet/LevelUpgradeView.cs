using Sources.GamePlaySystem.MainMenuGame.Store;
using UniRx;
using UnityEngine;

namespace Game.Screens.MainMenuScreen
{
    public class LevelUpgradeView : MonoBehaviour
    {
        private string _levelUpgradeId;

        [SerializeField] private GameObject _emptyBullet;

        public void OnSetUp(string levelUpgradeId, WeaponViewModel weaponViewModel)
        {
            _levelUpgradeId = levelUpgradeId;

            weaponViewModel.LevelUpgradeFee.Subscribe(value =>
            {
                if (weaponViewModel.LevelUpgradeIdsPassed.Contains(levelUpgradeId)) _emptyBullet.SetActive(false);
                else _emptyBullet.SetActive(true);

            }).AddTo(this);
        }
    }
}