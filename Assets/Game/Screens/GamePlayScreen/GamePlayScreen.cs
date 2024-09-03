using Sources.DataBaseSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Sources.UI;
using Cysharp.Threading.Tasks;

namespace Game.Screens.GamePlayScreen
{
    public class GamePlayScreen : BaseUI
    {
        [SerializeField] private GunHudViewController _gunHudViewController;

        private void Start()
        {
            _gunHudViewController.OnSetUp();
        }

        public override UniTask OnTransitionEnter()
        {
            return base.OnTransitionEnter();
        }
    }
}