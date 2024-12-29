using Game.Character.Enemy.Abstract;
using Sources.GamePlaySystem.MainGamePlay.Enemies;

namespace Game.Character.Enemy.Normal
{
    public class AnimationEnemyNormalHandler : AnimationHandlerAbstract
    {
        private readonly DamageUserType _typeDamage = DamageUserType.ShortRangeDamage; 

        public void OnAttack()
        {
            _enemyHandler.DamageUser(_typeDamage);
        }
    }
}