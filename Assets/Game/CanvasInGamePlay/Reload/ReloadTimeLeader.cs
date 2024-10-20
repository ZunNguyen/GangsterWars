using Sources.GamePlaySystem.Leader;
using Sources.Utils.Singleton;
using UniRx;

namespace Game.CanvasInGamePlay.Reload
{
    public class ReloadTimeLeader : ReloadTimeHandler
    {
        private LeaderSystem _leaderSystem => Locator<LeaderSystem>.Instance;

        protected override void Awake()
        {
            base.Awake();
            _leaderSystem.ReloadTimeHandler.TimeReloadCurrent.Subscribe(value =>
            {
                _time.text = value.ToString();
            }).AddTo(this);
        }
    }
}