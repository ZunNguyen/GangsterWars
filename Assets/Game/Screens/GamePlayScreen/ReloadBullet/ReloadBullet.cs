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
        private const float _heightPanel = 60;

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

        private void SetUpViewReloadBullet(GunModelView gunModel)
        {
            if (gunModel.GunId == LeaderKey.GunId_Default || gunModel.GunId == LeaderKey.GunId_02 || gunModel.GunId == LeaderKey.GunId_03)
            {
                var gunInfo = _leaderConfig.GetWeaponInfo(gunModel.GunId) as LeaderWeaponInfo;
                _bulletPerClip = gunInfo.BulletsPerClip;
                var sizeTarget = _widthPanelDefault + _bulletPerClip * _widthOneBullet;
                _panel.sizeDelta = new Vector2(sizeTarget, _heightPanel);

                ReleaseBullet();
                GetBullet(gunModel);
            }

            if (gunModel.GunId == LeaderKey.GunId_04)
            {
                return;
            }

            if (gunModel.GunId == LeaderKey.GunId_05)
            {
                return;
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

        private void GetBullet(GunModelView gunModel)
        {
            _bullets.Clear();
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