using Sources.DataBaseSystem.Leader;
using Sources.DataBaseSystem;
using Sources.GamePlaySystem.Leader;
using Sources.Utils.Singleton;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sources.GameData
{
    public class UserData
    {
        public List<GunModel> LeaderData { get; private set; } = new();

        public List<string> WavesPassedData;

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