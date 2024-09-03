using Sources.GamePlaySystem.Leader;
using Sources.Utils.Singleton;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game.Screens.GamePlayScreen
{
    public class GunHudViewController : MonoBehaviour
    {
        private LeaderSystem _leaderSystem => Locator<LeaderSystem>.Instance;

        [SerializeField] private GunHudView _gunHudView;

        public void OnSetUp()
        {
            var gunModels = _leaderSystem.GunModels;

            foreach (var gunModel in gunModels)
            {
                var gunHudView = Instantiate(_gunHudView, transform);
                gunHudView.OnSetUp(gunModel.Key);
            }
        }
    }
}