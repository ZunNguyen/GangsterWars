using Sources.DataBaseSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Sources.UI;
using Cysharp.Threading.Tasks;
using Sources.SpawnerSystem;
using Sources.Utils.Singleton;

namespace Game.Screens.GamePlayScreen
{
    public class GamePlayScreen : BaseUI
    {
        private SpawnerManager _spawnerManager => Locator<SpawnerManager>.Instance;

        [SerializeField] private GunHudViewController _gunHudViewController;
        [SerializeField] private UserHpController _userHpController;

        private void Start()
        {
            _gunHudViewController.OnSetUp();
            _userHpController.OnSetUp();
        }
    }
}