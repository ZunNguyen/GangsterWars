using Cysharp.Threading.Tasks;
using Sources.GamePlaySystem.Leader;
using Sources.Utils.Singleton;
using UnityEngine;
using UniRx;
using Sources.Extension;
using Sources.DataBaseSystem;
using Sources.DataBaseSystem.Leader;
using System.Collections.Generic;
using System;
using Sources.SpawnerSystem;
using UnityEditor.PackageManager;

namespace Game.Screens.GamePlayScreen
{
    public class ReloadBullet : MonoBehaviour
    {
        private const float _widthPanelDefault = 300;
        private const float _widthOneBullet = 100;

        private SpawnerManager _spawnerManager => Locator<SpawnerManager>.Instance;
        private LeaderSystem _leaderSystem => Locator<LeaderSystem>.Instance;
        private DataBase _dataBase => Locator<DataBase>.Instance;
        private LeaderConfig _leaderConfig => _dataBase.GetConfig<LeaderConfig>();

        private int _bulletPerClip;
        private List<Bullet> _bullets = new();

        [SerializeField] private Bullet _bullet;
        [SerializeField] private RectTransform _panel;
        [SerializeField] private Transform _bulletHolder;

        private void Awake()
        {
            _leaderSystem.GunHandler.GunModelCurrent.Subscribe(value =>
            {
                SetUpViewReloadBullet(value);
            }).AddTo(this);
        }

        private void SetUpViewReloadBullet(GunModel gunModel)
        {
            if (gunModel.GunId == LeaderKey.GunId_01 || gunModel.GunId == LeaderKey.GunId_02 || gunModel.GunId == LeaderKey.GunId_03)
            {
                var gunInfo = _leaderConfig.GetWeaponInfo(gunModel.GunId);
                _bulletPerClip = gunInfo.BulletsPerClip;
                var sizeTarget = _widthPanelDefault + _bulletPerClip * _widthOneBullet;
                _panel.sizeDelta = new Vector2(sizeTarget, 0);

                ReleaseBullet();
                GetBullet(gunModel);
            }

            if (gunModel.GunId == LeaderKey.GunId_04)
            {

            }

            if (gunModel.GunId == LeaderKey.GunId_05)
            {

            }
        }

        private void ReleaseBullet()
        {
            if (_bullets.Count == 0) return;

            foreach (var bullet in _bullets)
            {
                _spawnerManager.Release(bullet);
            }
        }

        private void GetBullet(GunModel gunModel)
        {
            for (int i = 0; i < _bulletPerClip; i++)
            {
                var bullet = _spawnerManager.Get(_bullet);
                bullet.transform.SetParent(_bulletHolder);
                bullet.OnSetUp(i, gunModel.BulletAvailable);
                _bullets.Add(bullet);
            }
        }
    }
}