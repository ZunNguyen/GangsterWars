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
        [SerializeField] private List<Transform> _targetPosToShoot;

        private void Awake()
        {
            for (int i = 0; i < _posSpawnEnemies.Count; i++)
            {
                _posSpawnEnemies[i].OnSetUp(i);
            }

            foreach (var targetPos in _targetPosToShoot)
            {
                _mainGamePlaySystem.EnemiesController.SetShieldPlayerPos(targetPos);
            }
        }
    }
}