using Game.Character.Enemy.Abstract;
using Sources.Extension;
using UnityEngine;

namespace Game.Character.Enemy.Normal
{
    public class EnemyNormalController : EnemyControllerAbstract
    {
        protected override void OnTriggerToStop(Collider2D collision)
        {
            if (collision.tag == CollisionTagKey.SHIELD_USER && !_isAttacking)
            {
                _enemyHandler.OnAttack();
            }
        }
    }
}