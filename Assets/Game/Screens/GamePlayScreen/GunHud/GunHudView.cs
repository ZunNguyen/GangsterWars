using Cysharp.Threading.Tasks;
using DG.Tweening;
using Sources.Audio;
using Sources.DataBaseSystem;
using Sources.DataBaseSystem.Leader;
using Sources.Extension;
using Sources.GamePlaySystem.Leader;
using Sources.Utils.Singleton;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Screens.GamePlayScreen
{
    public class GunHudView : MonoBehaviour
    {
        private readonly Vector3 _targetScale = new Vector3(1.2f, 1.2f, 1.2f);
        private const float _duration = 0.2f;

        private DataBase _dataBase => Locator<DataBase>.Instance;
        private LeaderConfig _leaderConfig => _dataBase.GetConfig<LeaderConfig>();
        private AudioManager _audioManager => Locator<AudioManager>.Instance;

        private LeaderSystem _leaderSystem => Locator<LeaderSystem>.Instance;
        private GunHandler _gunHandler;

        private string _gunId;

        [SerializeField] private Image _icon;
        [SerializeField] private TMP_Text _countText;

        public void OnSetUp(string gunId)
        {
            _gunId = gunId;

            _gunHandler = _leaderSystem.GunHandler;

            if (_gunHandler.GunModels.ContainsKey(gunId))
            {
                _gunHandler.GunModels[gunId].BulletTotal.Subscribe(value =>
                {
                    if (value == InfinityKey.INFINITY_BULLET_INT) _countText.text = InfinityKey.INFINITY_SYMBOL;
                    else _countText.text = value.ToString();
                }).AddTo(this);
            }
            else
            {
                _gunHandler.GunModelCurrent.Value.BulletTotal.Subscribe(value =>
                {
                    if (value == InfinityKey.INFINITY_BULLET_INT)
                    {
                        _countText.fontSize = 40;
                        _countText.text = InfinityKey.INFINITY_SYMBOL;
                    }
                    else _countText.text = value.ToString();
                }).AddTo(this);
            }

            _gunHandler.GunModelCurrent.Subscribe(value =>
            {
                if (value.GunId == _gunId) EffectChangeGun();
                else transform.localScale = Vector3.one;
            }).AddTo(this);

            SetUpIcon();
        }

        private void SetUpIcon()
        {
            var weaponInfo = _leaderConfig.GetWeaponInfo(_gunId) as LeaderWeaponInfo;
            _icon.sprite = weaponInfo.Icon;

            if (_gunId == LeaderKey.GunId_05)
            {
                SetSizeIcon(170);
            }

            if (_gunId == LeaderKey.GunId_03 || _gunId == LeaderKey.GunId_04)
            {
                SetSizeIcon(150);
            }
        }

        private void SetSizeIcon(float value)
        {
            var rect = _icon.GetComponent<RectTransform>();
            Vector2 size = rect.sizeDelta;
            size.x = value;
            rect.sizeDelta = size;
        }

        public void OnChoseClicked()
        {
            _audioManager.Play(AudioKey.SFX_CLICK_01);
            _gunHandler.ChangeGunModel(_gunId);
        }

        private void EffectChangeGun()
        {
            transform.DOScale(_targetScale, _duration);
        }
    }
}