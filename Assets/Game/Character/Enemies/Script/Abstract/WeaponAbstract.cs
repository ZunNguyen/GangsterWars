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
        protected readonly TypeDamageUser _typeDamage = TypeDamageUser.LongRangeDamage;

        private SpawnerManager _spawnerManager => Locator<SpawnerManager>.Instance;
        protected MainGamePlaySystem _mainGamePlaySystem => Locator<MainGamePlaySystem>.Instance;

        protected EnemyHandler _enemyHandler;

        public virtual void OnSetUp(EnemyHandler enemyHandler, int indexPos)
        {
            _enemyHandler = enemyHandler;
            Moving();
        }

        protected abstract void Moving();

        protected virtual void DamageUser()
        {
            _enemyHandler.DamageUser(_typeDamage);
        }

        public void OnRelease()
        {
            _spawnerManager.Release(this);
        }
    }
}