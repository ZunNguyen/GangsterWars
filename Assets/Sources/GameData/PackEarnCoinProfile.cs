using Sources.DataBaseSystem;
using Sources.Utils.Singleton;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sources.GameData
{
    public class PackEarnCoinData
    {
        public string Id;
        public int TimeNextEarn;
    }

    public class PackEarnCoinProfile : IProfileData
    {
        private DataBase _dataBase => Locator<DataBase>.Instance;
        private EarnCoinConfig _earnCoinConfig => _dataBase.GetConfig<EarnCoinConfig>();

        public List<PackEarnCoinData> PackEarnCoinDatas;
        public DateTime LastTimeUserPlay = new();
        
        public void SetDataDefault() 
        {
            PackEarnCoinDatas = new ();
            var earnCoinInfos = _earnCoinConfig.GetAllInfos();

            foreach (var info in earnCoinInfos)
            {
                var newPackEarnCoinData = new PackEarnCoinData
                {
                    Id = info.Id,
                    TimeNextEarn = 0
                };

                PackEarnCoinDatas.Add(newPackEarnCoinData);
            }

            Save();
        }

        public void SetLastTimeUserPlay(DateTime dateTime)
        {
            LastTimeUserPlay = dateTime;
            Save();
        }
    }
}