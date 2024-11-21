using Sources.GamePlaySystem.MainGamePlay;
using Sources.SpawnerSystem;
using Sources.Utils.Singleton;
using UnityEngine;

namespace Game.Character.Player.Abstract
{
    public abstract class WeaponAbstract : MonoBehaviour
    {
        protected MainGamePlaySystem _mainGamePlaySystem => Locator<MainGamePlaySystem>.Instance;
        protected SpawnerManager _spawnerManager => Locator<SpawnerManager>.Instance;

        public int Damage { get; private set; }

        public virtual void OnSetUp(string weaponId, int damage)
        {
            Damage = damage;
        }

        public abstract void Moving();
    }
}