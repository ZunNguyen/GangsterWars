using DG.Tweening;
using TMPro;
using UniRx;
using UnityEngine;


namespace Game.CanvasInGamePlay.Reload
{
    public abstract class ReloadTimeController : MonoBehaviour
    {
        private readonly Vector3 _rotate = new Vector3(0, 0, -360f);
        private const float _duration = 3f;

        private Tween _tween;

        [SerializeField] private RectTransform _circle;
        [SerializeField] private TMP_Text _time;

        protected ReactiveProperty<float> _timeReload;

        private void Awake()
        {
            gameObject.SetActive(false);
            GetSystem();
            RotateCircle();
            _tween.Pause();
        }

        protected virtual void GetSystem()
        {
            OnSetUp();
        }

        private void OnSetUp()
        {
            if (_timeReload == null) return;

            _timeReload.Subscribe(value =>
            {
                gameObject.SetActive(value != 0);
                _time.text = value.ToString();

                if (value == 0) _tween.Pause();
                else _tween.Play();

            }).AddTo(this);
        }

        private void RotateCircle()
        {
            _tween = _circle.DORotate(_rotate, _duration, RotateMode.FastBeyond360).SetEase(Ease.Linear).SetLoops(-1);
        }
    }
}