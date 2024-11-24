using Game.Character.Enemy.Shoot;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Character.Enemy.Shoot
{
    public class AnimationEnemyTwoShootHandler : AnimationEnemyShootHandler
    {
        [Header("Pos Shoot 2")]
        [SerializeField] private Transform _posSpawnWeapon_2;

        public void OnAttack_2()
        {
            SpawnBullet(_posSpawnWeapon_2);
            SpawnMuzzleFlash(_posSpawnWeapon_2);
        }
    }
}