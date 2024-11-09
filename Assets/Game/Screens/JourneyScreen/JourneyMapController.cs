using Sources.DataBaseSystem;
using Sources.GameData;
using Sources.GamePlaySystem.JourneyMap;
using Sources.Utils.Singleton;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Screens.JourneyScreen
{
    public class JourneyMapController : MonoBehaviour
    {
        private DataBase _dataBase => Locator<DataBase>.Instance;
        private JourneyMapConfig _journeyMapConfig => _dataBase.GetConfig<JourneyMapConfig>();
        private JourneyMapSystem _journeyMapSystem => Locator<JourneyMapSystem>.Instance;

        private GridMap _gridMapCurrent;

        private void Awake()
        {
            _gridMapCurrent = _journeyMapSystem.GridMapCurrent;
            SetUpJourneyMap();
        }

        private void SetUpJourneyMap()
        {

            for (int row = 0; row < _gridMapCurrent.Rows; row++)
            {
                for (int col = 0; col < _gridMapCurrent.Columns; col++)
                {
                    var cell = _gridMapCurrent.Matrix[row][col];
                    Debug.Log(cell);
                }
            }
        }
    }
}