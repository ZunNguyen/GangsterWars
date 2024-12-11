using Cysharp.Threading.Tasks;
using Sources.DataBaseSystem;
using Sources.GamePlaySystem.MainMenuGame;
using Sources.Utils.Singleton;
using UnityEngine;
using UniRx;

namespace Game.Screens.MainMenuScreen
{
    public class StoreBuyCoinHandler : MonoBehaviour
    {
        private DataBase _dataBase => Locator<DataBase>.Instance;
        private BuyCoinConfig _buyCoinConfig => _dataBase.GetConfig<BuyCoinConfig>();

        private StoreSystem _storeSystem => Locator<StoreSystem>.Instance;

        private TabState _tab = TabState.TabCoin;

        [SerializeField] private RectTransform _posCoinTotal;

        [Header("Coin View Prefab")]
        [SerializeField] private BuyCoinView _buyCoinViewPrefab;
        [SerializeField] private Transform _holder;

        private void Start()
        {
            _storeSystem.TabCurrent.Subscribe(value =>
            { 
                gameObject.SetActive(value == _tab);
            }).AddTo(this);

            SetBuyCoinViews();
        }

        private void SetBuyCoinViews()
        {
            var buyCoinInfos = _buyCoinConfig.GetAllInfos();

            foreach (var buyCoinInfo in buyCoinInfos)
            {
                var buyCoinView = Instantiate(_buyCoinViewPrefab, _holder);
                buyCoinView.OnSetUp(buyCoinInfo, _posCoinTotal);
            }
        }
    }
}