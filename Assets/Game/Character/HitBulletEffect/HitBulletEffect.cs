using Sources.SpawnerSystem;
using Sources.Utils.Singleton;
using UnityEngine;

namespace Game.Character.BulletEffect
{
    public class HitBulletEffect : MonoBehaviour
    {
        private SpawnerManager _spawnerManager => Locator<SpawnerManager>.Instance;

        public void OnSetUp(Vector2 posOrigin, Vector2 nowPos)
        {
            Vector2 direction = (posOrigin - nowPos).normalized;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            angle += 180f;
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));

            transform.position = nowPos;
        }

        public void Release()
        {
            _spawnerManager.Release(gameObject);
        }
    }
}