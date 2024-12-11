using Game.Character.Enemy.Abstract;
using Sources.Extension;
using UnityEngine;

namespace Game.Character.Enemy.Shoot
{
    public class EnemyShootController : EnemyControllerAbstract
    {
        protected override void OnTriggerToStop(Collider2D collision)
        {
            if (collision.tag == CollisionTagKey.STOP_POINT && !_isAttacking) _enemyHandler.HaveColliderInFace();
        }
    }
}