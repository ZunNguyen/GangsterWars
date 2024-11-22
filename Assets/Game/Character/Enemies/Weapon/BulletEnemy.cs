using DG.Tweening;
using Sources.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Character.Enemy.Weapon
{
    public class BulletEnemy : Abstract.WeaponAbstract
    {
        private const float _offsetTargetX = 10f;
        private const float _duration = 0.5f;

        [SerializeField] private Rigidbody2D _rb;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            DamageUser();
            OnRelease();
        }

        protected override void Moving()
        {
            var targetPosX = transform.position.x - _offsetTargetX;

            transform.DOMoveX(targetPosX, _duration).SetEase(Ease.InOutQuart);
        }
    }
}