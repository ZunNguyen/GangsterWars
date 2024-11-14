using TMPro;
using UniRx;
using UnityEngine;


namespace Game.CanvasInGamePlay.Reload
{
    public abstract class ReloadTimeController : MonoBehaviour
    {
        [SerializeField] private Transform _circle;
        [SerializeField] private TMP_Text _time;

        protected ReactiveProperty<float> _timeReload;

        private void Awake()
        {
            gameObject.SetActive(false);
            GetSystem();
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
            }).AddTo(this);
        }
    }
}