using JetBrains.Annotations;
using Sources.DataBaseSystem;
using Sources.GameData;
using Sources.GamePlaySystem.MainGamePlay.Enemies;
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
        TwoThird = 65, // 65%
        OneThird = 35, // 35%
        Empty = 0, // 0%
    }

    public class UserRecieveDamageHandler
    {
        private const float _percentHpShield = 0.75f;
        private const float _percentHpUser = 0.25f;

        private GameData.GameData _gameData => Locator<GameData.GameData>.Instance;
        private UserProfile _userProfile => _gameData.GetProfileData<UserProfile>();

        private DataBase _dataBase => Locator<DataBase>.Instance;
        private ShieldConfig _shieldConfig => _dataBase.GetConfig<ShieldConfig>();

        private int _maxHpTotal;
        private int _maxHpUser;
        private int _maxHpShield;
        private int _hpCurrentShield;
        private bool _isDeath;
        private ShieldData _shieldData;

        public int MaxHpBegin { get; private set; }
        public string ShieldId { get; private set; }
        public ReactiveProperty<ShieldState> ShieldCurrentState { get;} = new ReactiveProperty<ShieldState>(ShieldState.Full);
        public ReactiveProperty<int> HpCurrentUser {  get;} = new ReactiveProperty<int>();
        public Action IsDead;
        public Action<DamageUserType> DamageShield;
        public Action DamageUser;

        public void OnSetUp()
        {
            _isDeath = false;
            _maxHpTotal = GetMaxHp();

            HpCurrentUser.Value = _maxHpUser = (int)(_maxHpTotal * _percentHpUser);
            _maxHpShield = (int)(_maxHpTotal * _percentHpShield);
            _hpCurrentShield = (int)_shieldData.State / 100 * _maxHpTotal;
            MaxHpBegin = _hpCurrentShield + HpCurrentUser.Value;

            ShieldCurrentState.Value = GetShieldState();
        }

        private int GetMaxHp()
        {
            _shieldData = _userProfile.GetShieldDataCurrent();
            ShieldId = _shieldData.Id;
            var shieldInfo = _shieldConfig.GetWeaponInfo(ShieldId) as ShieldWeaponInfo;
            var levelInfo = shieldInfo.GetLevelUpgradeInfo(_shieldData.LevelUpgradeId);
            return levelInfo.DamageOrHp;
        }

        private ShieldState GetShieldState()
        {
            if (_hpCurrentShield > _maxHpShield * 0.35f && _hpCurrentShield <= _maxHpShield * 0.65f)
            {
                return ShieldState.TwoThird;
            }
            if (_hpCurrentShield > 0 && _hpCurrentShield <= _maxHpShield * 0.35f)
            {
                return ShieldState.OneThird;
            }
            if (_hpCurrentShield <= 0)
            {
                return ShieldState.Empty;
            }
            return ShieldState.Full;
        }

        public void SubstractHp(int damage, DamageUserType typeDamage)
        {
            if (ShieldCurrentState.Value != ShieldState.Empty)
            {
                DamageShield?.Invoke(typeDamage);
                SubstractHpShield(damage);
            }
            else
            {
                DamageUser?.Invoke();
                SubstractHpUser(damage);
            }
        }

        private void SubstractHpShield(int damage)
        {
            _hpCurrentShield -= damage;
            ShieldCurrentState.Value = _shieldData.State = GetShieldState();
            _userProfile.Save();
        }

        private void SubstractHpUser(int damage)
        {
            HpCurrentUser.Value -= damage;
            CheckDead();
        }

        private void CheckDead()
        {
            if (_isDeath) return;

            if (HpCurrentUser.Value <= 0)
            {
                _isDeath = true;
                IsDead?.Invoke();
            }
        }

        public int GetTotalHpWhenEnd()
        {
            return _hpCurrentShield + HpCurrentUser.Value;
        }
    }
}