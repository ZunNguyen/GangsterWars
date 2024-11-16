using Sources.GameData;
using Sources.GamePlaySystem.Character;
using Sources.Utils.Singleton;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.CanvasInGamePlay.Reload
{
    public class ReloadTimeSniper : ReloadTimeController
    {
        private GameData _gameData => Locator<GameData>.Instance;
        private UserProfile _userProfile => _gameData.GetProfileData<UserProfile>();

        private SniperSystem _sniperSystem => Locator<SniperSystem>.Instance;

        protected override void GetSystem()
        {
            if (_userProfile.SniperDatas == null) return;

            _timeReload = _sniperSystem.ReloadTimeHandler.TimeReloadCurrent;
            base.GetSystem();
        }
    }
}