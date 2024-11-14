using Game.Character.Abstract;
using Sources.Extension;
using Sources.GamePlaySystem.Character;
using Sources.Utils.Singleton;
using UnityEngine;

namespace Game.Character.Sniper
{
    public class SniperAnimationHandler : AnimationHandler
    {
        private SniperSystem _sniperSystem => Locator<SniperSystem>.Instance;

        protected override void OnSetUp()
        {
            _reloadTimeHandler = _sniperSystem.ReloadTimeHandler;
            _weaponHandler = _sniperSystem.BomHandler;
            _animationShootKey = SniperKey.ANIMATIONKEY_SHOOTING;
            _animationReloadKey = SniperKey.ANIMATIONKEY_RELOADING;
        }
    }
}