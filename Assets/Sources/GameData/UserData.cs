using Sources.DataBaseSystem.Leader;
using Sources.DataBaseSystem;
using Sources.GamePlaySystem.Leader;
using Sources.Utils.Singleton;
using System.Collections.Generic;
using UniRx;

namespace Sources.GameData
{
    public class BomberData
    {
        public string BomId;
        public ReactiveProperty<int> Quatity;
        public string LevelDamage;
    }

    public class UserData : IProfileData
    {
        public List<GunModel> LeaderData { get; private set; } = new();

        public List<string> WavesPassedData;
        
        public List<BomberData> BomberData { get; private set; } = new();

        private DataBase _dataBase => Locator<DataBase>.Instance;
        private LeaderConfig _leaderConfig => _dataBase.GetConfig<LeaderConfig>();

        public List<GunModel> SetDataLeaderData()
        {
            var weapons = _leaderConfig.Weapons;
            foreach (var weapon in weapons)
            {
                var weaponData = new GunModel
                {
                    GunId = weapon.Id,
                    LevelDamage = "level-01",
                    BulletTotal = new UniRx.ReactiveProperty<int>(weapon.MaxBullet),
                };

                LeaderData.Add(weaponData);
            }

            return LeaderData;
        }
    }
}