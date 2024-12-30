using Cysharp.Threading.Tasks;
using Game.Character.Leader;
using Sources.Extension;
using Sources.GamePlaySystem.Joystick;
using Sources.GamePlaySystem.Leader;
using Sources.Utils.Singleton;
using UniRx;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Game.Cursor
{
    public class ClickHandler : MonoBehaviour
    {
        private const float _radiusRaycast = 0.2f;

        private LeaderSystem _leaderSystem => Locator<LeaderSystem>.Instance;
        private JoystickSystem _joystickSystem => Locator<JoystickSystem>.Instance;

        private bool _isShooting = false;
        private bool _isUseMachineGun = false;
        private bool _isCountingTimePressMouse = false;

        [SerializeField] private Camera _camera;

        private void Awake()
        {
            if (_joystickSystem.IsUseJoystick) enabled = false;

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
                SetNameObjectUserShoot();

                if (_isUseMachineGun)
                {
                    _isShooting = true;
                    if (!_isCountingTimePressMouse) CountTimePressMouse();
                }
                else LeaderAction.Instance.LeaderShooting();
            }
            if (Input.GetMouseButtonUp(0)) _isShooting = false;
        }

        private void SetNameObjectUserShoot()
        {
            Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D[] raycastHits = Physics2D.CircleCastAll(ray.origin, _radiusRaycast, ray.direction);

            LeaderAction.Instance.SetNameObjectUserShoot(raycastHits);
        }

        private async void CountTimePressMouse()
        {
            _isCountingTimePressMouse = true;

            while (_isShooting)
            {
                SetNameObjectUserShoot();
                _leaderSystem.GunHandler.Shooting();

                await UniTask.Delay(200);
            }

            _isCountingTimePressMouse = false;
        }
    }
}