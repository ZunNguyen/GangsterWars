using Mono.Cecil.Cil;
using UnityEngine;
using UnityEngine.Pool;

namespace Sources.SpawnerSystem
{
    public class Spawner<T> where T : Object
    {
        private readonly Object _prefab;
        private readonly ObjectPool<T> _pool;
        public ObjectPool<T> Pool => _pool;
        private int _counter;

        public Spawner(T prefab)
        {
            _prefab = prefab;
            _pool = new ObjectPool<T>(CreateFunc, GetFunc, ReleaseFunc, DestroyFunc);
        }

        private T CreateFunc()
        {
            var obj = UnityEngine.Object.Instantiate(_prefab);
            obj.name = $"{_prefab.name}{SpawnerManager.Indicator}{_counter++}";

            return obj as T;
        }

        private void GetFunc(T obj)
        {
            if (obj == null) return;

            switch (obj)
            {
                case GameObject gobj:
                    if (!gobj.activeSelf)
                        gobj.SetActive(true);
                    break;

                case MonoBehaviour mono:
                    if (!mono.gameObject.activeSelf)
                        mono.gameObject.SetActive(true);
                    break;
            }
        }

        private void ReleaseFunc(T obj)
        {
            if (obj == null) return;

            switch (obj)
            {
                case GameObject gobj:
                    if (gobj.activeSelf)
                        gobj.SetActive(false);

                    //gobj.transform.SetParent(null);
                    break;

                case MonoBehaviour mono:
                    if (mono.gameObject.activeSelf)
                        mono.gameObject.SetActive(false);

                    //mono.transform.SetParent(null);
                    break;
            }
        }

        private void DestroyFunc(T obj)
        {
            if (obj == null) return;

            Object.Destroy(obj);
        }

        public void Reset()
        {
            _counter = 0;
            _pool.Clear();
        }
    }
}