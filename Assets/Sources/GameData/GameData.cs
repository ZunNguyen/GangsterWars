using Sources.DataBaseSystem;
using Sources.DataBaseSystem.Leader;
using Sources.GamePlaySystem.Leader;
using Sources.Utils.Singleton;
using System;
using System.Collections.Generic;

namespace Sources.GameData
{
    public class GameData
    {
        public void Init()
        {
            Locator<GameData>.Set(this);
        }

        public List<GunModel> LeaderData { get; private set; } = new();

        public List<string> WavesPassed;

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
                    BulletTotal = new UniRx.ReactiveProperty<int>(weapon.MaxBullet),
                };

                LeaderData.Add(weaponData);
            }

            return LeaderData;
        }
    }
}