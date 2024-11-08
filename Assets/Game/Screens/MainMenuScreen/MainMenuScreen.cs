using Sources.DataBaseSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Sources.UI;
using Sources.Command;
using Sources.GamePlaySystem.CoinController;
using Sources.Utils.Singleton;
using TMPro;
using Cysharp.Threading.Tasks;
using UniRx;
using Sources.UISystem;
using Game.Screens.JourneyScreen;

namespace Game.Screens.MainMenuScreen
{
    public class MainMenuScreen : BaseUI
    {
        private UIManager _uiManager => Locator<UIManager>.Instance;
        private CoinControllerSystem _coinControllerSystem => Locator<CoinControllerSystem>.Instance;

        [SerializeField] private TMP_Text _text;
        [SerializeField] private StoreController _storeController;
        [SerializeField] private TabHandler _tabHandler;

        protected override void Awake()
        {
            base.Awake();
            _coinControllerSystem.Coins.Subscribe(value =>
            {
                _text.text = value.ToString();
            }).AddTo(this);

            _storeController.OnSetUp();
            _tabHandler.OnSetUp();
        }

        public async void OnPlayGameClicked()
        {
            await Close();
            await _uiManager.Show<JourneyScreen.JourneyScreen>();

            //await OnTransitionExit();
            //await new LoadSenceCommand("GamePlay").Execute();
        }
    }
}