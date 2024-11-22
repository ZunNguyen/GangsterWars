using Cysharp.Threading.Tasks;
using DG.Tweening;
using Sources.DataBaseSystem;
using Sources.GamePlaySystem.MainGamePlay;
using Sources.GamePlaySystem.MainGamePlay.Enemies;
using Sources.SpawnerSystem;
using Sources.Utils;
using Sources.Utils.Singleton;
using UnityEngine;

namespace Game.Character.Enemy.Abstract
{
    public abstract class WeaponAbstract : MonoBehaviour
    {
        private SpawnerManager _spawnerManager => Locator<SpawnerManager>.Instance;
        private MainGamePlaySystem _mainGamePlaySystem => Locator<MainGamePlaySystem>.Instance;

        private EnemyHandler _enemyHandler;

        protected Vector3 _targetPos;

        [SerializeField] private Animator _animator;

        private void SetEnabled(bool status)
        {
            _animator.enabled = status;
        }

        public void OnSetUp(EnemyHandler enemyHandler)
        {
            _enemyHandler = enemyHandler;
            GetTargetPos();
            SetEnabled(false);
            Moving();
        }

        private void GetTargetPos()
        {
            _targetPos = _mainGamePlaySystem.EnemiesController.ShieldPlayerPos.position;
        }

        protected abstract void Moving();

        protected async void OnBombHit()
        {
            await UniTask.Delay(500);
            SetEnabled(true);
            _enemyHandler.DamageUser();
        }

        public void OnCompletAnimation()
        {
            _spawnerManager.Release(this);
        }
    }
}