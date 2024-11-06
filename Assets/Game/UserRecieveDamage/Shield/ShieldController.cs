using Cysharp.Threading.Tasks;
using Sources.DataBaseSystem;
using Sources.GamePlaySystem.MainGamePlay;
using Sources.Utils.Singleton;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

namespace Game.UserReceiveDamage.Shield
{
    public class ShieldController : MonoBehaviour
    {
        private DataBase _dataBase => Locator<DataBase>.Instance;
        private ShieldConfig _shieldConfig => _dataBase.GetConfig<ShieldConfig>();

        private MainGamePlaySystem _mainGamePlaySystem => Locator<MainGamePlaySystem>.Instance;
        private ShieldWeaponInfo _shieldInfo;

        [SerializeField] private SpriteRenderer _iconShield;

        private void Awake()
        {
            GetShieldInfo();
            SubscribeShieldData();
        }

        private void GetShieldInfo()
        {
            _shieldInfo = _shieldConfig.GetWeaponInfo(_mainGamePlaySystem.UserRecieveDamageHandler.ShieldId) as ShieldWeaponInfo;
        }

        private void SubscribeShieldData()
        {
            _mainGamePlaySystem.UserRecieveDamageHandler.ShieldCurrentState.Subscribe(value =>
            {
                if (value == ShieldState.Empty)
                {
                    gameObject.SetActive(false);
                    return;
                }

                _iconShield.sprite = _shieldInfo.GetIconShield(value);
            }).AddTo(this);
        }
    }
}