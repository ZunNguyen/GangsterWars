using Game.Character.Enemy.Abstract;
using Sources.GamePlaySystem.MainGamePlay.Enemies;

namespace Game.Character.Enemy.Normal
{
    public class AnimationEnemyNormalHandler : AnimationHandlerAbstract
    {
        private readonly TypeDamageUser _typeDamage = TypeDamageUser.ShortRangeDamage; 

        public override void OnAttackEnd()
        {
            _enemyHandler.DamageUser(_typeDamage);
        }
    }
}