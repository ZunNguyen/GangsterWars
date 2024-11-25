using Cysharp.Threading.Tasks;
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
        private DataBase _dataBase => Locator<DataBase>.Instance;
        private LeaderConfig _leaderConfig => _dataBase.GetConfig<LeaderConfig>();

        private LeaderSystem _leaderSystem => Locator<LeaderSystem>.Instance;
        private GunHandler _gunHandler;

        private string _gunId;

        [SerializeField] private Image _icon;
        [SerializeField] private Text _countText;

        public void OnSetUp(string gunId)
        {
            _gunId = gunId;

            _gunHandler = _leaderSystem.GunHandler;
            if (_gunHandler.GunModels.ContainsKey(gunId))
            {
                _gunHandler.GunModels[gunId].BulletTotal.Subscribe(value =>
                {
                    _countText.text = value.ToString();
                }).AddTo(this);
            }
            else
            {
                _gunHandler.GunModelCurrent.Value.BulletTotal.Subscribe(value =>
                {
                    _countText.text = value.ToString();
                }).AddTo(this);
            }

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
            _gunHandler.ChangeGunModel(_gunId);
        }
    }
}