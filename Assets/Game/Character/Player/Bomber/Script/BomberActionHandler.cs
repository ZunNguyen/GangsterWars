using Game.Character.Player.Abstract;
using Sources.GamePlaySystem.Character;
using Sources.Utils.Singleton;

namespace Game.Character.Bomber
{
    public class BomberActionHandler : ActionHandlerAbstract
    {
        private BomberSystem _bomberSystem => Locator<BomberSystem>.Instance;

        protected override void GetHandlerSystem()
        {
            _weaponHandler = _bomberSystem.BomHandler;
        }
    }
}