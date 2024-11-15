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
        private MainGamePlaySystem _mainGamePlaySystem => Locator<MainGamePlaySystem>.Instance;
        private SpawnerManager _spawnerManager => Locator<SpawnerManager>.Instance;
        private DataBase _dataBase => Locator<DataBase>.Instance;
        private BomberConfig _bomberConfig => _dataBase.GetConfig<BomberConfig>();

        private int _damage;
        public int Damage => _damage;

        [SerializeField] private SpriteRenderer _sprite;
        [SerializeField] private Animator _animator;
        [SerializeField] private Collider2D _collider;

        public void OnSetUp(string weaponId, int damage)
        {
            _damage = damage;
        }

        public abstract void ThrowBomb();

        public void OnCompletAnimation()
        {
            _spawnerManager.Release(this);
        }
    }
}