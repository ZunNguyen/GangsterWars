using Cysharp.Threading.Tasks;
using DG.Tweening;
using Game.ShakeCamera;
using Sources.Audio;
using Sources.DataBaseSystem;
using Sources.Extension;
using Sources.GamePlaySystem.MainGamePlay;
using Sources.GamePlaySystem.MainGamePlay.Enemies;
using Sources.Utils.Singleton;
using UniRx;
using UnityEngine;

namespace Game.UserReceiveDamage.Shield
{
    public class ShieldController : MonoBehaviour
    {
        private readonly Color _originalColor = new Color(1f, 1f, 1f);
        private readonly Color _targetColor = new Color(1f, 0.63f, 0.63f);
        private const float _duration = 0.4f;
        private const float _offsetX = 0.1f;

        private DataBase _dataBase => Locator<DataBase>.Instance;
        private ShieldConfig _shieldConfig => _dataBase.GetConfig<ShieldConfig>();

        private MainGamePlaySystem _mainGamePlaySystem => Locator<MainGamePlaySystem>.Instance;
        private AudioManager _audioManager => Locator<AudioManager>.Instance;

        private ShieldWeaponInfo _shieldInfo;

        private float _ogirinalX;
        private float _targetMoveX;
        private bool _isFirstSub = true;

        [SerializeField] private CameraShake _cameraShake;
        [SerializeField] private SpriteRenderer _iconShield;
        [SerializeField] private PolygonCollider2D _polygonCollider;

        private void Awake()
        {
            GetShieldInfo();
            SubscribeShieldData();
            SubscribeDamageShield();

            _ogirinalX = transform.position.x;
            _targetMoveX = _ogirinalX - _offsetX;
        }

        private void GetShieldInfo()
        {
            _shieldInfo = _shieldConfig.GetWeaponInfo(_mainGamePlaySystem.UserRecieveDamageHandler.ShieldId) as ShieldWeaponInfo;
        }

        private void SubscribeShieldData()
        {
            _mainGamePlaySystem.UserRecieveDamageHandler.ShieldCurrentState.Subscribe(value =>
            {
                if (value == ShieldState.Empty) _polygonCollider.enabled = false;

                _iconShield.sprite = _shieldInfo.GetIconShield(value);

                if (!_isFirstSub) _cameraShake.Shake(0.5f, 0.5f);
                if (_isFirstSub) _isFirstSub = false;
            }).AddTo(this);
        }

        private void SubscribeDamageShield()
        {
            _mainGamePlaySystem.UserRecieveDamageHandler.DamageShield += AnimationDamageShield;
        }

        private async void AnimationDamageShield(DamageUserType type)
        {
            if (type == DamageUserType.LongRangeDamage)
            {
                _iconShield.color = _targetColor;
                await UniTask.Delay(100);
                _iconShield.color = _originalColor;
                return;
            }

            _audioManager.Play(AudioKey.SFX_SHIELD_IMPACT);

            _iconShield.color = _targetColor;
            await _iconShield.transform.DOMoveX(_targetMoveX, _duration);
            _iconShield.transform.DOMoveX(_ogirinalX, _duration).OnComplete(() =>
            {
                _iconShield.color = _originalColor;
            });
        }

        private void OnDestroy()
        {
            _mainGamePlaySystem.UserRecieveDamageHandler.DamageShield -= AnimationDamageShield;
        }
    }
}