using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

namespace Game.Screens.MainMenuScreen
{
    public class LevelUpgradeView : MonoBehaviour
    {
        private string _levelUpgradeId;

        [SerializeField] private GameObject _emptyBullet;

        public void OnSetUp(string levelUpgradeId, ReactiveProperty<List<string>> levelsUpgradeAvailable)
        {
            _levelUpgradeId = levelUpgradeId;

            levelsUpgradeAvailable.Subscribe(value  =>
            {
                if (value.Contains(levelUpgradeId)) _emptyBullet.SetActive(false);
                else _emptyBullet.SetActive(true);
            }).AddTo(this);
        }
    }
}