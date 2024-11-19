using Sources.Utils.Singleton;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace Sources.SpawnerSystem
{
    public class SpawnerManager
    {
        public static readonly string Indicator = " [Clone]-";

        private Dictionary<string, Spawner<Object>> _spawners = new Dictionary<string, Spawner<Object>>();

        public void Initialize()
        {
            Locator<SpawnerManager>.Set(this);
        }

        public void ResetAllSpawner()
        {
            foreach (var spawner in _spawners.Values)
            {
                spawner.Reset();
            }
        }

        public T Get<T>(T prefab) where T : Object
        {
            if (prefab == null)
            {
                throw new System.Exception($"{typeof(T).Name} is null. Please check again");
            }

            if (!_spawners.ContainsKey(prefab.name))
            {
                var newSpawner = new Spawner<Object>(prefab);
                _spawners.Add(prefab.name, newSpawner);
            }

            var obj = _spawners[prefab.name].Pool.Get();
            if (obj.GetType() != typeof(T) && obj is GameObject go)
            {
                return go.GetComponent<T>();
            }

            return obj as T;
        }

        public void Release<T>(T prefab) where T : Object
        {
            if (prefab == null) return;

            if (prefab is GameObject go)
                ReleaseHelper(go);

            else if (prefab is MonoBehaviour mono)
                ReleaseHelper(mono);

            else 
                Debug.LogError($"You can not release {prefab.name} because it is not a GameObject or Monobehaviour");
        }

        private void ReleaseHelper(Object prefab)
        {
            if (prefab == null) return;

            var key = GetKeyFromObject(prefab);

            if (!_spawners.ContainsKey(key))
            {
                Object.Destroy(prefab);
            }

            _spawners[key].Pool.Release(prefab);
        }

        private string GetKeyFromObject(Object obj)
        {
            if (obj == null) return "";

            var indexOfIndicator = obj.name.IndexOf(Indicator, System.StringComparison.Ordinal);

            if (indexOfIndicator == -1) return obj.name;

            return obj.name.Substring(0, indexOfIndicator);
        }
    }
}