using Game.Character.Player.Abstract;
using Sources.Extension;
using Sources.GamePlaySystem.Character;
using Sources.Utils.Singleton;

namespace Game.Character.Bomber
{
    public class BomberAnimationHandler : AnimationHandlerAbstract
    {
        private BomberSystem _bomberSystem => Locator<BomberSystem>.Instance;

        protected override void InitValue()
        {
            _reloadTimeHandler = _bomberSystem.ReloadTimeHandler;
            _weaponHandler = _bomberSystem.BomHandler;
            _animationShootKey = BomberKey.AnimationKey_Throwing;
            _animationReloadKey = BomberKey.AnimationKey_Reloading;
        }
    }
} 