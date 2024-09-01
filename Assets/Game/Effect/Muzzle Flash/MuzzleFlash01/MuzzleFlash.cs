using Sources.SpawnerSystem;
using Sources.Utils.Singleton;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Effect.MuzzleFlash
{
    public class MuzzleFlash : MonoBehaviour
    {
        private SpawnerManager _spawnerManager => Locator<SpawnerManager>.Instance;

        public void Release()
        {
            _spawnerManager.Release<GameObject>(gameObject);
        }
    }
}