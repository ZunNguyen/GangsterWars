using Sources.GamePlaySystem.MainGamePlay;
using Sources.GamePlaySystem.MainGamePlay.Enemies;
using Sources.Utils.Singleton;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.PosSpawnEnemies
{
    public class PosSpawnEnemiesController : MonoBehaviour
    {
        private MainGamePlaySystem _mainGamePlaySystem => Locator<MainGamePlaySystem>.Instance;

        [SerializeField] private List<PosSpawnEnemy> _posSpawnEnemies;
        [SerializeField] private Transform _posShieldPlayer;

        private void Awake()
        {
            for (int i = 0; i < _posSpawnEnemies.Count; i++)
            {
                _posSpawnEnemies[i].OnSetUp(i);
            }

            _mainGamePlaySystem.EnemiesController.SetShieldPlayerPos(_posShieldPlayer);
        }
    }
}