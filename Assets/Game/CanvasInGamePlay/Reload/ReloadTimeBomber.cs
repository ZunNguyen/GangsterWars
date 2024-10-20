using Sources.GamePlaySystem.Bomber;
using Sources.Utils.Singleton;
using UniRx;

namespace Game.CanvasInGamePlay.Reload
{
    public class ReloadTimeBomber : ReloadTimeHandler
    {
        private BomberSystem _bomberSystem => Locator<BomberSystem>.Instance;

        protected override void Awake()
        {
            base.Awake();
            _bomberSystem.ReloadTimeHandler.TimeReloadCurrent.Subscribe(value =>
            {
                _time.text = value.ToString();
            }).AddTo(this);
        }
    }
}