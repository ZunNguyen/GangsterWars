using Sources.GamePlaySystem.Leader;
using Sources.Utils.Singleton;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.CanvasInGamePlay.Reload
{
    public class ReloadTimeLeader : ReloadTimeController
    {
        private LeaderSystem _leaderSystem => Locator<LeaderSystem>.Instance;

        protected override void GetSystem()
        {
            _timeReload = _leaderSystem.GunHandler.TimeReloadCurrent;
            base.GetSystem();
        }
    }
}