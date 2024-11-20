using Sources.GamePlaySystem.GameResult;
using Sources.Utils.Singleton;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Screens.GamePlayScreen
{
    public class PanelResultHandler : MonoBehaviour
    {
        private GameResultSystem _gameResultSystem => Locator<GameResultSystem>.Instance;

        [SerializeField] private List<GameObject> _stars;
        [SerializeField] private Text _textTitle;
        [SerializeField] private Text _textMoney;

        private void Awake()
        {
            gameObject.SetActive(false);

            _gameResultSystem.IsUserWin += EndGame;
        }
        
        private void EndGame(bool isUserWin)
        {
            if (isUserWin) SetPanelWin();
            else SetPanelLose();
        }

        private void SetPanelWin()
        {

        }

        private void SetPanelLose()
        {

        }

        private void OnDestroy()
        {
            _gameResultSystem.IsUserWin -= EndGame;
        }

        public void OnBackHomeClicked()
        {

        }

        public void OnResetClicked()
        {

        }

        public void OnNextWaveClicked()
        {

        }
    }
}