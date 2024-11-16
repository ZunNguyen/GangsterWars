using Game.CanvasInGamePlay.HPBar;
using Game.CanvasInGamePlay.Reload;
using Sources.GamePlaySystem.MainGamePlay.Enemies;
using Sources.SpawnerSystem;
using Sources.Utils.Singleton;
using UnityEngine;

namespace Game.CanvasInGamePlay.Controller
{
    public class CanvasModel
    {
        public Canvas Canvas;
        public Transform TransformObject;
        public EnemyHandler EnemyHandler;
    }

    public class CanvasInGamePlayController : MonoBehaviour
    {
        private SpawnerManager _spawnerManager => Locator<SpawnerManager>.Instance;
        private CanvasModel _canvasModel = new();

        [Header("HP Bar")]
        [SerializeField] private HpBar _hpBarPrefab;
        [SerializeField] private Transform _hpBarHolder;

        [Header("Damage Feed")]
        [SerializeField] private DamageFeed _damageFeedPrefab;
        [SerializeField] private Transform _damageFeedHolder;

        [SerializeField] private Canvas _canvas;

        public void OnSetUpHpBar(Transform transformObject, EnemyHandler enemyHandler)
        {
            _canvasModel.Canvas = _canvas;
            _canvasModel.TransformObject = transformObject;
            _canvasModel.EnemyHandler = enemyHandler;

            var newHpBar = _spawnerManager.Get(_hpBarPrefab);
            newHpBar.gameObject.SetActive(true);
            newHpBar.transform.SetParent(_hpBarHolder);
            newHpBar.transform.localScale = Vector3.one;
            newHpBar.OnSetUp(_canvasModel);

            var newDamageFeed = _spawnerManager.Get(_damageFeedPrefab);
            newDamageFeed.transform.SetParent(_damageFeedHolder);
            newDamageFeed.transform.localScale = Vector3.one;
            newDamageFeed.OnSetUp(_canvasModel);
        }
    }
}