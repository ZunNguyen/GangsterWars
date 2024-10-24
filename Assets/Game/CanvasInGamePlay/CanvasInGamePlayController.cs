using Game.CanvasInGamePlay.HPBar;
using Game.CanvasInGamePlay.Reload;
using Sources.GamePlaySystem.MainGamePlay.Enemies;
using Sources.SpawnerSystem;
using Sources.Utils.Singleton;
using UnityEngine;

namespace Game.CanvasInGamePlay.Controller
{
    public class HpBarModel
    {
        public Canvas Canvas;
        public Transform TransformObject;
        public EnemyHandler EnemyHandler;
    }

    public class CanvasInGamePlayController : MonoBehaviour
    {
        private SpawnerManager _spawnerManager => Locator<SpawnerManager>.Instance;
        private HpBarModel _hpBarModel = new();

        [SerializeField] private Canvas _canvas;
        [SerializeField] private ReloadTimeController _reloadTime;
        [SerializeField] private HpBar _hpBar;

        public void OnSetUpHpBar(Transform transformObject, EnemyHandler enemyHandler)
        {
            _hpBarModel.Canvas = _canvas;
            _hpBarModel.TransformObject = transformObject;
            _hpBarModel.EnemyHandler = enemyHandler;

            var newHpBar = _spawnerManager.Get<HpBar>(_hpBar);
            newHpBar.transform.SetParent(this.transform);
            newHpBar.transform.localScale = Vector3.one;
            newHpBar.OnSetUp(_hpBarModel);
        }
    }
}