using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.PosSpawnEnemies
{
    public class PosSpawnEnemy : MonoBehaviour
    {
        private int _indexPos;

        public void OnSetUp(int index)
        {
            _indexPos = index;
        }

        
    }
}