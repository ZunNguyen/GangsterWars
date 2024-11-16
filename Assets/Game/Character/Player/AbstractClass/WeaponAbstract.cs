using Cysharp.Threading.Tasks;
using DG.Tweening;
using Sources.DataBaseSystem;
using Sources.GamePlaySystem.MainGamePlay;
using Sources.SpawnerSystem;
using Sources.Utils.Singleton;
using Sources.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Character.Abstract
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