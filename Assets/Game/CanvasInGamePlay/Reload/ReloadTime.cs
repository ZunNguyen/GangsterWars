using Cysharp.Threading.Tasks;
using Sources.DataBaseSystem;
using Sources.GamePlaySystem.Leader;
using Sources.Utils.Singleton;
using TMPro;
using UnityEngine;
using UniRx;

namespace Game.CanvasInGamePlay.Reload
{
    public class ReloadTime : MonoBehaviour
    {
        private LeaderSystem _leaderSystem => Locator<LeaderSystem>.Instance;

        [SerializeField] private Transform _circle;
        [SerializeField] private TMP_Text _time;

        private void Awake()
        {
            _leaderSystem.ReloadTimeHandler.TimeReloadCurrent.Subscribe(value =>
            {
                _time.text = value.ToString();
            }).AddTo(this);
        }
    }
}