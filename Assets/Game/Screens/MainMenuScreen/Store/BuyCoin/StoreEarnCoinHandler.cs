using Cysharp.Threading.Tasks;
using Sources.DataBaseSystem;
using Sources.GamePlaySystem.MainMenuGame;
using Sources.Utils.Singleton;
using UnityEngine;
using UniRx;

namespace Game.Screens.MainMenuScreen
{
    public class StoreEarnCoinHandler : MonoBehaviour
    {
        private DataBase _dataBase => Locator<DataBase>.Instance;
        private EarnCoinConfig _buyCoinConfig => _dataBase.GetConfig<EarnCoinConfig>();

        private StoreSystem _storeSystem => Locator<StoreSystem>.Instance;

        private TabState _tab = TabState.TabCoin;

        [SerializeField] private Transform _posCoinTotal;

        [Header("Coin View Prefab")]
        [SerializeField] private EarnCoinView _buyCoinViewPrefab;
        [SerializeField] private Transform _holder;
        [SerializeField] private GameObject _store;

        private void Awake()
        {
            _store.SetActive(false);
            OnSetUp();
        }

        private void OnSetUp()
        {
            _storeSystem.TabCurrent.Subscribe(value =>
            {
                _store.SetActive(value == _tab);
            }).AddTo(this);

            SetBuyCoinViews();
        }

        private void SetBuyCoinViews()
        {
            var buyCoinInfos = _buyCoinConfig.GetAllInfos();

            foreach (var buyCoinInfo in buyCoinInfos)
            {
                var buyCoinView = Instantiate(_buyCoinViewPrefab, _holder, false);
                buyCoinView.OnSetUp(buyCoinInfo, _posCoinTotal);
            }
        }
    }
}