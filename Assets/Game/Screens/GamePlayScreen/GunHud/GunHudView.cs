using Cysharp.Threading.Tasks;
using Sources.DataBaseSystem;
using Sources.DataBaseSystem.Leader;
using Sources.GamePlaySystem.Leader;
using Sources.Utils.Singleton;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UniRx;

namespace Game.Screens.GamePlayScreen
{
    public class GunHudView : MonoBehaviour
    {
        private DataBase _dataBase => Locator<DataBase>.Instance;
        private LeaderConfig _leaderConfig => _dataBase.GetConfig<LeaderConfig>();

        private LeaderSystem _leaderSystem => Locator<LeaderSystem>.Instance;

        private string _gunId;

        [SerializeField] private Image _icon;
        [SerializeField] private TMP_Text _countText;

        public void OnSetUp(string gunId)
        {
            _gunId = gunId;
            var weaponInfo = _leaderConfig.GetWeaponInfo(_gunId);

            _icon.sprite = weaponInfo.Icon;

            _leaderSystem.GunModels[gunId].BulletTotal.Subscribe(value =>
            {
                _countText.text = value.ToString();
            }).AddTo(this);
        }

        public void OnChoseClicked()
        {
            _leaderSystem.ChangeGunModel(_gunId);
        }
    }
}