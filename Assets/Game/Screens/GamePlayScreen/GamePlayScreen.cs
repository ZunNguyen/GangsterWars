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

        [SerializeField] private GameObject a;

        [SerializeField] private GunHudViewController _gunHudViewController;
        [SerializeField] private UserHpController _userHpController;

        private void Start()
        {
            var x = _spawnerManager.Get(a);
            _spawnerManager.Release(x);

            _gunHudViewController.OnSetUp();
            _userHpController.OnSetUp();
        }

        public override UniTask OnTransitionEnter()
        {
            return base.OnTransitionEnter();
        }
    }
}