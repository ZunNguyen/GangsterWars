using JetBrains.Annotations;
using Sources.DataBaseSystem;
using Sources.GameData;
using Sources.Utils.Singleton;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

namespace Sources.GamePlaySystem.MainGamePlay
{
    public enum ShieldState
    {
        Full = 100, // 100%
        ThreeQuarter = 75, // 75%
        Half = 50, // 50%
        Quarter = 25, // 25%
        Empty = 0, // 0%
    }

    public class UserRecieveDamageHandler
    {
        private readonly float _maxHp;

        private GameData.GameData _gameData => Locator<GameData.GameData>.Instance;

        private DataBase _dataBase => Locator<DataBase>.Instance;
        private ShieldConfig _shieldConfig => Locator<ShieldConfig>.Instance;

        public ReactiveProperty<ShieldState> ShieldCurrentState { get;} = new ReactiveProperty<ShieldState>();

        private float _hpCurrent;

        public UserRecieveDamageHandler()
        {
            _maxHp = GetMaxHp();
        }

        private float GetMaxHp()
        {
            var shieldInfo = _shieldConfig.GetShieldInfo(_gameData.StoreData.ShieldIdCurrent);
            var levelInfo = shieldInfo.GetLevelInfo(_gameData.StoreData.GetLevelShieldCurrent());
            return levelInfo.Hp;
        }

        public void OnSetUp()
        {
            _hpCurrent = _maxHp;
            GetShieldState(_hpCurrent);
        }

        private void GetShieldState(float hp)
        {
            if (hp <= _maxHp * 0.75f && ShieldCurrentState.Value == ShieldState.Full)
            {
                ShieldCurrentState.Value = ShieldState.ThreeQuarter;
            }
            if (hp <= _maxHp * 0.5f && ShieldCurrentState.Value == ShieldState.ThreeQuarter)
            {
                ShieldCurrentState.Value = ShieldState.Half;
            }
            if (hp <= _maxHp * 0.25f && ShieldCurrentState.Value == ShieldState.Half)
            {
                ShieldCurrentState.Value = ShieldState.Quarter;
            }
            if (hp <= 0 && ShieldCurrentState.Value != ShieldState.Empty)
            {
                ShieldCurrentState.Value = ShieldState.Empty;
            }
        }

        public void SubstractHpShield(float hp)
        {
            _hpCurrent -= hp;
            GetShieldState(_hpCurrent);
        }
    }
}