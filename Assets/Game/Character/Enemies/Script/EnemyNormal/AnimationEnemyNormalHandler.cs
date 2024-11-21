using Game.Character.Enemy.Abstract;

namespace Game.Character.Enemy.Normal
{
    public class AnimationEnemyNormalHandler : AnimationHandlerAbstract
    {
        public override void OnAttackEnd()
        {
            _enemyHandler.DamageUser();
        }
    }
}