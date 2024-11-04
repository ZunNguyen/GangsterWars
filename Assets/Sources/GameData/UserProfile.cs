using Sources.DataBaseSystem;
using Sources.Extension;
using Sources.Utils.String;
using System.Collections.Generic;

namespace Sources.GameData
{
    public interface IWeaponData { }

    public class ShieldData
    {
        public string ShieldId;
        public string LevelUpgradeId;
        public bool IsChosed;
    }

    public class WeaponData : IWeaponData
    {
        public string WeaponId;
        public string LevelUpgradeId;
        public int Quatity;
    }

    public class UserProfile : IProfileData
    {
        public List<WeaponData> LeaderDatas;
        private Dictionary<string, WeaponData> _leaderDatasCache = new();

        public List<WeaponData> BomberDatas;
        private Dictionary<string, WeaponData> _bomberDatasCache = new();

        public List<WeaponData> SniperDatas;
        
        public List<ShieldData> ShieldDatas;

        public List<string> WavesPassedDatas;

        public int Coins;

        public void SetLeaderDataDefault()
        {
            LeaderDatas = new();

            var weaponDefault = new WeaponData
            {
                WeaponId = LeaderKey.GunId_Default,
                LevelUpgradeId = LevelUpgradeKey.LevelUpgrade_Default,
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
                WeaponId = BomberKey.BomberId_Default,
                LevelUpgradeId = LevelUpgradeKey.LevelUpgrade_Default,
                Quatity = BomberKey.Quality_Bom_Default,
            };

            BomberDatas.Add(weaponDefault);
            Save();
        }

        public void SetShieldDataDefault()
        {
            ShieldDatas = new();

            var shieldDefault = new ShieldData
            {
                ShieldId = ShieldKey.ShieldId_Default,
                LevelUpgradeId = LevelUpgradeKey.LevelUpgrade_Default,
                IsChosed = true,
            };

            ShieldDatas.Add(shieldDefault);
            Save();
        }

        public WeaponData GetWeaponData(string weaponId)
        {
            var weapons = GetCorrectListWeapon(weaponId).weapons;
            var weaponCache = GetCorrectListWeapon(weaponId).weaponCache;

            if (!weaponCache.ContainsKey(weaponId))
            {
                var weapon = weapons.Find(x => x.WeaponId == weaponId);
                weaponCache.Add(weaponId, weapon);
            }

            return weaponCache[weaponId];
        }

        private (List<WeaponData> weapons, Dictionary<string, WeaponData> weaponCache) GetCorrectListWeapon(string weaponId)
        {
            var baseWeaponId = StringUtils.GetBaseName(weaponId);

            var baseWeaponLeaderId = StringUtils.GetBaseName(LeaderDatas[0].WeaponId);
            if (baseWeaponId == baseWeaponLeaderId)
            {
                return (LeaderDatas, _leaderDatasCache);
            }

            var baseWeaponBomberId = StringUtils.GetBaseName(BomberDatas[0].WeaponId);
            if (baseWeaponId == baseWeaponBomberId)
            {
                return (BomberDatas, _bomberDatasCache);
            }

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
    }
}