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
        public UserData UserData = new();
        public StoreData StoreData = new();

        public void Init()
        {
            Locator<GameData>.Set(this);
        }
    }
}