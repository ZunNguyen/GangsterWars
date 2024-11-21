using Sources.SpawnerSystem;
using Sources.UI;
using Sources.Utils.Singleton;
using UnityEngine;

namespace Game.Screens.GamePlayScreen
{
    public class GamePlayScreen : BaseUI
    {
        private SpawnerManager _spawnerManager => Locator<SpawnerManager>.Instance;

        [SerializeField] private GunHudViewController _gunHudViewHandler;
        [SerializeField] private UserHpHandler _userHpHandler;
        [SerializeField] private EnemiesHpTotalHandler _enemiesHpTotalHandler;
        [SerializeField] private PanelResultHandler _panelResultHandler;
        [SerializeField] private PanelHandler _panelHandler;

        private void Start()
        {
            _gunHudViewHandler.OnSetUp();
            _userHpHandler.OnSetUp();
            _enemiesHpTotalHandler.OnSetUp();
            _panelResultHandler.OnSetUp();
            _panelHandler.OnSetUp();
        }
    }
}