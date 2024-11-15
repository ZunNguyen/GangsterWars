using Cysharp.Threading.Tasks;
using DG.Tweening;
using Sources.DataBaseSystem;
using Sources.GamePlaySystem.MainGamePlay;
using Sources.SpawnerSystem;
using Sources.Utils.Singleton;
using Sources.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Character.Sniper
{
    public class SniperWeapon : MonoBehaviour
    {
        private MainGamePlaySystem _mainGamePlaySystem => Locator<MainGamePlaySystem>.Instance;
        private SpawnerManager _spawnerManager => Locator<SpawnerManager>.Instance;
        private DataBase _dataBase => Locator<DataBase>.Instance;
        private BomberConfig _bomberConfig => _dataBase.GetConfig<BomberConfig>();

        private int _damage;
        public int Damage => _damage;

        [SerializeField] private LineRenderer lineRenderer;
        [SerializeField] private float laserDuration = 0.1f;

        public void OnSetUp(string weaponId, int damage)
        {

        }

        public void ThrowBomb()
        {
            var enemyTarget = _mainGamePlaySystem.SpawnEnemiesHandler.Enemies[0];
            var enemyPos = enemyTarget.transform.position;

            Vector3 start = transform.position;
            Vector3 direction = transform.forward;

            // Dùng Raycast để xác định điểm kết thúc của tia laser
            RaycastHit hit;
            Vector3 end = enemyPos;
            if (Physics.Raycast(start, direction, out hit))
            {
                end = hit.point; // nếu bắn trúng mục tiêu
                                 // Bạn có thể thêm code để gây sát thương lên hit.collider nếu cần
            }
            else
            {
                end = start + direction * 100f; // khoảng cách tối đa nếu không trúng gì
            }

            // Thiết lập vị trí cho LineRenderer
            lineRenderer.SetPosition(0, start); // Điểm đầu của laser
            lineRenderer.SetPosition(1, end);   // Điểm cuối của laser
            lineRenderer.enabled = true;

            // Gọi hàm tắt laser sau một khoảng thời gian ngắn
            Invoke(nameof(DisableLaser), laserDuration);
        }

        private void DisableLaser()
        {
            lineRenderer.enabled = false;
        }
    }
}