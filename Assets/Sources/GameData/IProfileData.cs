using Sources.Utils.Singleton;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sources.GameData
{
    public class IProfileData
    {
        private GameData _gameData => Locator<GameData>.Instance;

        public virtual void Save()
        {
            _gameData.SaveData(this);
        }
    }
}