using Cysharp.Threading.Tasks;
using Game.Character.Enemy.Abstract;
using Sources.Extension;
using Sources.GamePlaySystem.Joystick;
using Sources.GamePlaySystem.Leader;
using Sources.Utils.Singleton;
using UniRx;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Game.Character.Leader
{
    public class LeaderAction : MonoBehaviour
    {
        private const float _radiusRaycast = 0.2f;

        private JoystickSystem _joystickSystem => Locator<JoystickSystem>.Instance;
        private LeaderSystem _leaderSystem => Locator<LeaderSystem>.Instance;

        private static LeaderAction _instance;
        public static LeaderAction Instance => _instance;

        public string NameObjectShoot { get; private set; }

        private void Awake()
        {
            if (_instance == null) _instance = this;
        }

        public void LeaderShooting()
        {
            _leaderSystem.GunHandler.Shooting();
        }

        public void SetNameObjectUserShoot(RaycastHit2D[] hits)
        {
            foreach (var hit in hits)
            {
                if (hit.collider != null)
                {
                    EnemyControllerAbstract enemy = hit.collider.GetComponentInParent<EnemyControllerAbstract>();
                    if (enemy != null)
                    {
                        NameObjectShoot = enemy.gameObject.name;
                        return;
                    }
                }
            }
            NameObjectShoot = "";
        }
    }
}