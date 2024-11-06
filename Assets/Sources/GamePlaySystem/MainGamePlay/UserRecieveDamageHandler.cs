using JetBrains.Annotations;
using Sources.DataBaseSystem;
using Sources.GameData;
using Sources.Utils.Singleton;
using System;
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
        private readonly int _maxHpTotal;
        private readonly int _maxHpShield;
        private readonly int _maxHpUser;

        private GameData.GameData _gameData => Locator<GameData.GameData>.Instance;
        private UserProfile _userProfile => _gameData.GetProfileData<UserProfile>();

        private DataBase _dataBase => Locator<DataBase>.Instance;
        private ShieldConfig _shieldConfig => _dataBase.GetConfig<ShieldConfig>();
        private int _hpCurrentShield;

        public ReactiveProperty<ShieldState> ShieldCurrentState { get;} = new ReactiveProperty<ShieldState>(ShieldState.Full);
        public ReactiveProperty<int> HpCurrentUser {  get;} = new ReactiveProperty<int>();
        public Action IsDead { get; private set; }
        public string ShieldId {  get; private set; }

        public UserRecieveDamageHandler()
        {
            _maxHpTotal = GetMaxHp();

            _hpCurrentShield = _maxHpShield = (int)(_maxHpTotal * 0.75f);
            HpCurrentUser.Value = _maxHpUser = (int)(_maxHpTotal * 0.25f);
        }

        private int GetMaxHp()
        {
            var shieldData = _userProfile.GetShieldDataCurrent();
            ShieldId = shieldData.ShieldId;
            var shieldInfo = _shieldConfig.GetShieldInfo(ShieldId);
            var levelInfo = shieldInfo.GetLevelUpgradeInfo(shieldData.LevelUpgradeId);
            return levelInfo.Damage;
        }

        public void OnSetUp()
        {
            GetShieldState();
        }

        private void GetShieldState()
        {
            if (_hpCurrentShield <= _maxHpShield * 0.75f && ShieldCurrentState.Value == ShieldState.Full)
            {
                ShieldCurrentState.Value = ShieldState.ThreeQuarter;
            }
            if (_hpCurrentShield <= _maxHpShield * 0.5f && ShieldCurrentState.Value == ShieldState.ThreeQuarter)
            {
                ShieldCurrentState.Value = ShieldState.Half;
            }
            if (_hpCurrentShield <= _maxHpShield * 0.25f && ShieldCurrentState.Value == ShieldState.Half)
            {
                ShieldCurrentState.Value = ShieldState.Quarter;
            }
            if (_hpCurrentShield <= 0 && ShieldCurrentState.Value != ShieldState.Empty)
            {
                ShieldCurrentState.Value = ShieldState.Empty;
            }
        }

        public void SubstractHp(int damage)
        {
            if (ShieldCurrentState.Value != ShieldState.Empty)
            {
                SubstractHpShield(damage);
                return;
            }
            else
            {
                SubstractHpUser(damage);
                return;
            }
        }

        private void SubstractHpShield(int damage)
        {
            _hpCurrentShield -= damage;
            GetShieldState();
        }

        private void SubstractHpUser(int damage)
        {
            HpCurrentUser.Value -= damage;
            CheckDead();
        }

        private void CheckDead()
        {
            if (_hpCurrentShield < 0) IsDead?.Invoke();
        }
    }
}