using Sources.GameData;
using Sources.GamePlaySystem.Bomber;
using Sources.GamePlaySystem.Leader;
using Sources.Utils.Singleton;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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