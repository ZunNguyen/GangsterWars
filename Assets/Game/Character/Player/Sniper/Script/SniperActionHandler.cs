using Game.Character.Abstract;
using Sources.GamePlaySystem.Character;
using Sources.Utils.Singleton;

namespace Game.Character.Sniper
{
    public class SniperActionHandler : ActionHandler
    {
        private SniperSystem _sniperSystem => Locator<SniperSystem>.Instance;

        protected override void GetHandlerSystem()
        {
            _weaponHandler = _sniperSystem.BomHandler;
        }
    }
}