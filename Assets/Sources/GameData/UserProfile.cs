using Sources.Extension;
using Sources.GamePlaySystem.MainGamePlay;
using Sources.Utils.String;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Sources.GameData
{
    public class BaseData
    {
        public string Id;
        public string LevelUpgradeId;
    }

    public class ShieldData : BaseData
    {
        public ShieldState State = ShieldState.Full;
        public bool IsChosed;
    }

    public class WeaponData : BaseData
    {
        public int Quatity;
    }

    public class UserProfile : IProfileData
    {
        public List<WeaponData> LeaderDatas;
        private Dictionary<string, BaseData> _leaderDatasCache = new();

        public List<WeaponData> BomberDatas;
        private Dictionary<string, BaseData> _bomberDatasCache = new();

        public List<WeaponData> SniperDatas;
        private Dictionary<string, BaseData> _sniperDatasCache = new();

        public List<ShieldData> ShieldDatas;
        private Dictionary<string, BaseData> _shieldDatasCache = new();

        public int Coins = 1000;

        public void SetLeaderDataDefault()
        {
            LeaderDatas = new();

            var weaponDefault = new WeaponData
            {
                Id = LeaderKey.GUN_ID_DEFAULT,
                LevelUpgradeId = LevelUpgradeKey.LEVELUPGRADE_DEFAULT,
                Quatity = LeaderKey.Quality_Bullet_Default,
            };

            LeaderDatas.Add(weaponDefault);
            Save();
        }

        public void SetBomberDataDefault()
        {
            BomberDatas = new();

            var weaponDefault = new WeaponData
            {
                Id = BomberKey.BOMBER_ID_DEFAULT,
                LevelUpgradeId = LevelUpgradeKey.LEVELUPGRADE_DEFAULT,
                Quatity = BomberKey.Quality_Bom_Default,
            };

            BomberDatas.Add(weaponDefault);
            Save();
        }

        public void SetSniperDataDefault()
        {
            SniperDatas = new();

            var weaponDefault = new WeaponData
            {
                Id = SniperKey.SNIPER_ID_DEFAULT,
                LevelUpgradeId = LevelUpgradeKey.LEVELUPGRADE_DEFAULT,
                Quatity = SniperKey.QUALITY_SNIPER_DEFAULT,
            };

            SniperDatas.Add(weaponDefault);
            Save();
        }

        public void SetShieldDataDefault()
        {
            ShieldDatas = new();

            var shieldDefault = new ShieldData
            {
                Id = ShieldKey.SHIELD_ID_DEFAULT,
                LevelUpgradeId = LevelUpgradeKey.LEVELUPGRADE_DEFAULT,
                IsChosed = true,
            };

            ShieldDatas.Add(shieldDefault);
            Save();
        }

        public BaseData GetWeaponBaseData(string weaponId)
        {
            var weapons = GetCorrectListWeapon(weaponId).weapons.ToList();
            var weaponCache = GetCorrectListWeapon(weaponId).weaponCache;

            if (!weaponCache.ContainsKey(weaponId))
            {
                var weapon = weapons.Find(x => x.Id == weaponId);
                weaponCache.Add(weaponId, weapon);
            }

            return weaponCache[weaponId];
        }

        private (IEnumerable<BaseData> weapons, Dictionary<string, BaseData> weaponCache) GetCorrectListWeapon(string weaponId)
        {
            var baseWeaponId = StringUtils.GetBaseName(weaponId);

            if (baseWeaponId == StringUtils.GetBaseName(LeaderKey.GUN_ID_DEFAULT)) return (LeaderDatas, _leaderDatasCache);
            if (baseWeaponId == StringUtils.GetBaseName(ShieldKey.SHIELD_ID_DEFAULT)) return (ShieldDatas, _shieldDatasCache);
            if (baseWeaponId == StringUtils.GetBaseName(BomberKey.BOMBER_ID_DEFAULT)) return (BomberDatas, _bomberDatasCache);
            if (baseWeaponId == StringUtils.GetBaseName(SniperKey.SNIPER_ID_DEFAULT)) return (SniperDatas, _sniperDatasCache);

            return (null, null);
        }

        public ShieldData GetShieldDataCurrent()
        {
            foreach (var shieldData in ShieldDatas)
            {
                if (shieldData.IsChosed) return shieldData;
            }
            return null;
        }

        public void ChoseShield(string shieldId)
        {
            foreach (var shieldData in ShieldDatas)
            {
                if (shieldData.Id == shieldId) shieldData.IsChosed = true; 
                else shieldData.IsChosed = false;
            }

            Save();
        }

        public void SubsctractQualityWeapon(string id)
        {
            var weapon = GetWeaponBaseData(id) as WeaponData;
            weapon.Quatity--;
            if (weapon.Quatity < 0) weapon.Quatity = 0;
            Save();
        }
    }
}