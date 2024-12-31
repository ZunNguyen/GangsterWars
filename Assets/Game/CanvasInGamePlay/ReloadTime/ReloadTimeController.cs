using DG.Tweening;
using TMPro;
using UniRx;
using Unity.VisualScripting;
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

        [Header("Pos Character")]
        [SerializeField] private Transform _posShow;
        [SerializeField] private RectTransform _selfRect;
        [SerializeField] private Canvas _canvas;

        protected ReactiveProperty<float> _timeReload;

        private void Awake()
        {
            gameObject.SetActive(false);
            GetSystem();
            RotateCircle();
            SetPosObject();
            _tween.Pause();
        }

        private void SetPosObject()
        {
            var screenPos = RectTransformUtility.WorldToScreenPoint(Camera.main, _posShow.position);

            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                _canvas.transform as RectTransform,
                screenPos,
                _canvas.worldCamera,
                out Vector2 localPoint);

            _selfRect.anchoredPosition = localPoint;
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