using Sources.GameData;
using Sources.GamePlaySystem.Character;
using Sources.Utils.Singleton;

namespace Game.CanvasInGamePlay.Reload
{
    public class ReloadTimeBomber : ReloadTimeController
    {
        private GameData _gameData => Locator<GameData>.Instance;
        private UserProfile _userProfile => _gameData.GetProfileData<UserProfile>();

        private BomberSystem _bomberSystem => Locator<BomberSystem>.Instance;

        protected override void GetSystem()
        {
            if (_userProfile.BomberDatas == null) return;

            _timeReload = _bomberSystem.ReloadTimeHandler.TimeReloadCurrent;
            base.GetSystem();
        }
    }
}