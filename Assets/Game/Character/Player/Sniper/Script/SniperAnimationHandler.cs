using Game.Character.Player.Abstract;
using Sources.Extension;
using Sources.GamePlaySystem.Character;
using Sources.Utils.Singleton;
using UnityEngine;

namespace Game.Character.Sniper
{
    public class SniperAnimationHandler : AnimationHandlerAbstract
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