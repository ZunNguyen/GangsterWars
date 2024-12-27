using Cysharp.Threading.Tasks;
using Game.Character.Enemy.Abstract;
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

        private static Action _instance;
        public static Action Instance => _instance;

        private bool _isShooting = false;
        private bool _isUseMachineGun = false;
        private bool _isCountingTimePressMouse = false;
        
        public string NameObjectShoot { get; private set; }

        [SerializeField] private Camera _camera;

        private void Awake()
        {
            _leaderSystem.GunHandler.GunModelCurrent.Subscribe(value =>
            {
                if (value.GunId == LeaderKey.GunId_04 || value.GunId == LeaderKey.GunId_05) _isUseMachineGun = true;
                else _isUseMachineGun = false;
            }).AddTo(this);

            if (_instance == null) _instance = this;
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
                else _leaderSystem.GunHandler.Shooting();
            }
            if (Input.GetMouseButtonUp(0)) _isShooting = false;
        }

        private void SetNameObjectUserShoot()
        {
            Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);

            if (hit.collider != null)
            {
                EnemyControllerAbstract enemy = hit.collider.GetComponentInParent<EnemyControllerAbstract>();
                if (enemy != null) NameObjectShoot = enemy.gameObject.name;
            }
            else NameObjectShoot = "";
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