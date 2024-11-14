using Game.Character.Abstract;
using Sources.Extension;
using Sources.GamePlaySystem.Character;
using Sources.Utils.Singleton;

namespace Game.Character.Bomber
{
    public class BomberAnimationHandler : AnimationHandler
    {
        private BomberSystem _bomberSystem => Locator<BomberSystem>.Instance;

        protected override void OnSetUp()
        {
            _reloadTimeHandler = _bomberSystem.ReloadTimeHandler;
            _weaponHandler = _bomberSystem.BomHandler;
            _animationShootKey = BomberKey.AnimationKey_Throwing;
            _animationReloadKey = BomberKey.AnimationKey_Reloading;
        }
    }
} 