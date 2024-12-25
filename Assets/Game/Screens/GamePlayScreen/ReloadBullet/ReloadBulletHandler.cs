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
using UnityEngine.UI;

namespace Game.Screens.GamePlayScreen
{
    public class ReloadBulletHandler : MonoBehaviour
    {
        private SpawnerManager _spawnerManager => Locator<SpawnerManager>.Instance;
        private LeaderSystem _leaderSystem => Locator<LeaderSystem>.Instance;
        private DataBase _dataBase => Locator<DataBase>.Instance;
        private LeaderConfig _leaderConfig => _dataBase.GetConfig<LeaderConfig>();

        private IDisposable _disposableGunMachine;
        private int _bulletPerClip;
        private List<Bullet> _bullets = new();

        [Header("Panel Reload Gun Machine")]
        [SerializeField] private Transform _panelReloadGunMachine;
        [SerializeField] private Slider _slider;

        [Header("Panel Reload Gun Normal")]
        [SerializeField] private Transform _panelReloadGunNormal;
        [SerializeField] private Bullet _bulletPrefab;

        private void Awake()
        {
            _leaderSystem.GunHandler.GunModelCurrent.Subscribe(value =>
            {
                var gunInfo = _leaderConfig.GetWeaponInfo(value.GunId) as LeaderWeaponInfo;
                _bulletPerClip = gunInfo.BulletsPerClip;

                if (value.GunId == LeaderKey.GunId_04 || value.GunId == LeaderKey.GunId_05)
                {
                    _panelReloadGunMachine.gameObject.SetActive(true);
                    _panelReloadGunNormal.gameObject.SetActive(false);
                    SetPanelReloadGunMachine(value);
                }
                else
                {
                    _panelReloadGunMachine.gameObject.SetActive(false);
                    _panelReloadGunNormal.gameObject.SetActive(true);
                    SetPanelReloadGunNormal(value);
                }
            }).AddTo(this);
        }

        private void SetPanelReloadGunMachine(GunModelView gunModel)
        {
            _slider.maxValue = _bulletPerClip;
            _disposableGunMachine?.Dispose();
            _disposableGunMachine = gunModel.BulletAvailable.Subscribe(value =>
            {
                _slider.value = value;
            }).AddTo(this);
        }

        private void SetPanelReloadGunNormal(GunModelView gunModel)
        {
            ReleaseBullet();
            GetBullet(gunModel);
        }

        private void ReleaseBullet()
        {
            for (int i = (_bullets.Count - 1); i >= 0; i--)
            {
                _spawnerManager.Release(_bullets[i]);
            }
        }

        private void GetBullet(GunModelView gunModel)
        {
            _bullets.Clear();
            for (int i = 0; i < _bulletPerClip; i++)
            {
                var bullet = _spawnerManager.Get(_bulletPrefab);
                bullet.transform.SetParent(_panelReloadGunNormal, false);
                bullet.OnSetUp(i, gunModel.BulletAvailable);
                _bullets.Add(bullet);
            }
        }
    }
}