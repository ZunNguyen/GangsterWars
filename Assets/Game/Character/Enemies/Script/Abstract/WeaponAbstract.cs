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
        protected readonly DamageUserType _typeDamage = DamageUserType.LongRangeDamage;

        protected SpawnerManager _spawnerManager => Locator<SpawnerManager>.Instance;
        protected MainGamePlaySystem _mainGamePlaySystem => Locator<MainGamePlaySystem>.Instance;

        protected EnemyHandler _enemyHandler;

        public virtual void OnSetUp(EnemyHandler enemyHandler, int indexPos)
        {
            gameObject.SetActive(true);
            _enemyHandler = enemyHandler;
            Moving();
        }

        protected abstract void Moving();

        protected virtual void DamageUser()
        {
            _enemyHandler.DamageUser(_typeDamage);
        }

        public virtual void OnRelease()
        {
            _spawnerManager.Release(this);
        }
    }
}