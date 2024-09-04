using Sources.GamePlaySystem.Leader;
using Sources.Utils.Singleton;
using UnityEngine;

namespace Game.Screens.GamePlayScreen
{
    public class GunHudViewController : MonoBehaviour
    {
        private LeaderSystem _leaderSystem => Locator<LeaderSystem>.Instance;

        [SerializeField] private GunHudView _gunHudView;

        public void OnSetUp()
        {
            SetUpGunModelCurrent();
            SetUpGunModelInList();
        }

        private void SetUpGunModelCurrent()
        {
            var gunModel = _leaderSystem.GunHandler.GunModelCurrent;
            var gunHudView = Instantiate(_gunHudView, transform);
            gunHudView.OnSetUp(gunModel.Value.GunId);
        }

        private void SetUpGunModelInList()
        {
            var gunModels = _leaderSystem.GunHandler.GunModels;

            foreach (var gunModel in gunModels)
            {
                var gunHudView = Instantiate(_gunHudView, transform);
                gunHudView.OnSetUp(gunModel.Key);
            }
        }
    }
}