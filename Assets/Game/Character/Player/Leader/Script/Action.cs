using Cysharp.Threading.Tasks;
using Game.Weapon.Bullet;
using Sources.Extension;
using Sources.GamePlaySystem.Leader;
using Sources.SpawnerSystem;
using Sources.Utils.Singleton;
using UniRx;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Game.Character.Leader
{
    public class Action : MonoBehaviour
    {
        private LeaderSystem _leaderSystem => Locator<LeaderSystem>.Instance;

        private bool _isShooting = false;
        private bool _isUseMachineGun = false;

        private void Awake()
        {
            _leaderSystem.GunHandler.GunModelCurrent.Subscribe(value =>
            {
                if (value.GunId == LeaderKey.GunId_04 || value.GunId == LeaderKey.GunId_05) _isUseMachineGun = true;
                else _isUseMachineGun = false;
            }).AddTo(this);
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
            {
                if (_isUseMachineGun)
                {
                    _isShooting = true;
                    CountTimePressMouse();
                }
                else
                {
                    _leaderSystem.GunHandler.Shooting();
                }
            }
            if (Input.GetMouseButtonUp(0))
            {
                _isShooting = false;
            }
        }

        private async void CountTimePressMouse()
        {
            while (_isShooting)
            {
                _leaderSystem.GunHandler.Shooting();

                await UniTask.Delay(200);
            }
        }
    }
}