using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.PosSpawnEnemies
{
    public class PosSpawnEnemiesController : MonoBehaviour
    {
        private List<PosSpawnEnemy> _posSpawnEnemies;

        private void Awake()
        {
            for (int i = 0; i < _posSpawnEnemies.Count; i++)
            {
                _posSpawnEnemies[i].OnSetUp(i);
            }
        }
    }
}