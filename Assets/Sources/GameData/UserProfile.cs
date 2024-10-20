using Sources.DataBaseSystem.Leader;
using Sources.DataBaseSystem;
using Sources.GamePlaySystem.Leader;
using Sources.Utils.Singleton;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using Sources.Extension;
using System;

namespace Sources.GameData
{
    [Serializable]
    public class BomberData
    {
        public string BomId;
        public ReactiveProperty<int> Quatity = new ReactiveProperty<int>(0);
        public string LevelDamage;
    }

    public class UserProfile : IProfileData
    {
        public List<GunModel> LeaderData = new();

        public List<string> WavesPassedData;
        
        public List<BomberData> BomberData = new();

        public bool IsActiveBomber = true;

        public void SetLeaderDataDefault()
        {
            var weaponData = new GunModel();
            weaponData.GunId = LeaderKey.GunId_01;
            weaponData.BulletTotal.Value = 50;
            weaponData.LevelDamage = LeaderKey.LevelDamage_01;

            LeaderData.Add(weaponData);
            Save();
        }

        public void SetBomberDataDefault()
        {
            var bomberData = new BomberData();
            bomberData.BomId = BomberKey.BomberId_Default;
            bomberData.Quatity.Value = 20;
            bomberData.LevelDamage = BomberKey.LevelDamage_Default;

            BomberData.Add(bomberData);
            Save();
        }
    }
}