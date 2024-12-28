using DG.Tweening;
using Sources.Audio;
using Sources.DataBaseSystem;
using Sources.DataBaseSystem.Leader;
using Sources.Extension;
using Sources.GamePlaySystem.MainMenuGame;
using Sources.GamePlaySystem.MainMenuGame.Store;
using Sources.Language;
using Sources.SpawnerSystem;
using Sources.Utils;
using Sources.Utils.Singleton;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Screens.MainMenuScreen
{
    public class WeaponView : MonoBehaviour
    {
        private readonly Vector3 _targetScale = new Vector3(0.9f, 0.9f, 0.9f);
        private const float _duration = 0.15f;

        private StoreSystem _storeSystem => Locator<StoreSystem>.Instance;
        private SpawnerManager _spawnerManager => Locator<SpawnerManager>.Instance;
        private AudioManager _audioManager => Locator<AudioManager>.Instance;
        private LanguageTable _languageTable => Locator<LanguageTable>.Instance;

        private string _weaponId;
        private WeaponInfoBase _weaponInfo;
        private Sources.GamePlaySystem.MainMenuGame.Store.WeaponViewModel _weaponViewModel;
        private StoreHandlerBase _storeWeaponHandler;
        private ShieldStoreHandler _shieldStoreHandler;

        [Header("Level Upgrade")]
        [SerializeField] private LevelUpgradeView _levelUpgradePrefab;
        [SerializeField] private Transform _levelUpgradeHolder;

        [Header("Level Up")]
        [SerializeField] private GameObject _levelUpFee;
        [SerializeField] private TMP_Text _valueLevelUpFee;

        [Header("Reload")]
        [SerializeField] private GameObject _reload;
        [SerializeField] private TMP_Text _valueReload;

        [Header("Unlock")]
        [SerializeField] private GameObject _unlock;
        [SerializeField] private TMP_Text _valueUnlock;

        [Header("Check List")]
        [SerializeField] private GameObject _checkList;
        [SerializeField] private GameObject _iconCheckList;

        [Header("Wepon View")]
        [SerializeField] private RectTransform _iconHolder;
        [SerializeField] private Image _icon;
        [SerializeField] private LanguageText _languageText;
        [SerializeField] private GameObject _iconlock;
        [SerializeField] private GameObject _boxItemClock;

        [Header("Weapon Value")]
        [SerializeField] private GameObject _bulletCount;
        [SerializeField] private GameObject _shieldState;
        [SerializeField] private TMP_Text _bulletCountText;
        [SerializeField] private TMP_Text _shieldStateText;

        private void Awake()
        {
            _checkList.SetActive(false);
            _iconCheckList.SetActive(false);
        }

        public void OnSetUp(WeaponInfoBase weaponInfo, ReactiveProperty<bool> isChosed)
        {
            _weaponId = weaponInfo.Id;
            _storeWeaponHandler = _storeSystem.GetWeaponHandlerById(_weaponId);
            _weaponViewModel = _storeWeaponHandler.WeaponWiewModels[_weaponId];
            _weaponInfo = weaponInfo;

            if (weaponInfo is LeaderWeaponInfo leaderWeaponInfo) SetSizeIconGun(leaderWeaponInfo);
            isChosed.Subscribe(Effect).AddTo(this);

            SetIcon();
            SetWeaponValue();
            SetWeaponName();
            SetWeaponSate();
            SetLevelUpgrade();
            SetReloadFee();
            SetUnlockFee();
        }

        private void SetIcon()
        {
            _icon.sprite = _weaponInfo.Icon;
        }

        private void SetWeaponValue()
        {
            if (_weaponInfo is ShieldWeaponInfo shieldWeaponInfo)
            {
                _weaponViewModel.WeaponValue.Subscribe(value =>
                { 
                    _shieldStateText.text = value;
                }).AddTo(this);
            }
            else
            {
                _weaponViewModel.WeaponValue.Subscribe(value =>
                {
                    if (value == InfinityKey.INFINITY_SYMBOL) _bulletCountText.fontSize = 40;

                    _bulletCountText.text = value;
                }).AddTo(this);
            }
        }

        private void SetSizeIconGun(LeaderWeaponInfo leaderWeaponInfo)
        {
            if (leaderWeaponInfo.Id == LeaderKey.GunId_03 || leaderWeaponInfo.Id == LeaderKey.GunId_04)
            {
                Vector2 size = _iconHolder.sizeDelta;
                size.x = 150f;
                _iconHolder.sizeDelta = size;
            }

            if (leaderWeaponInfo.Id == LeaderKey.GunId_05)
            {
                Vector2 size = _iconHolder.sizeDelta;
                size.x = 175f;
                _iconHolder.sizeDelta = size;
            }
        }

        private void SetWeaponName()
        {
            var languageGunId = _weaponInfo.LanguageId;
            var languageItem = _languageTable.GetLanguageItem(languageGunId);
            _languageText.OnSetUp(languageGunId);
        }

        private void SetWeaponSate()
        {
            _weaponViewModel.State.Subscribe(state =>
            {
                if (state == ItemState.AlreadyHave)
                {
                    _iconlock.SetActive(false);
                    _unlock.SetActive(false);
                    _boxItemClock.SetActive(false);

                    _levelUpgradeHolder.gameObject.SetActive(true);
                    _levelUpFee.SetActive(true);

                    if (_weaponInfo is ShieldWeaponInfo shieldWeaponInfo) _shieldState.SetActive(true);
                    else _bulletCount.SetActive(true);

                    GetOnCheckList();
                }
                if (state == ItemState.CanUnlock)
                {
                    _iconlock.SetActive(true);
                    _unlock.SetActive(true);

                    _levelUpgradeHolder.gameObject.SetActive(false);
                    _levelUpFee.SetActive(false);
                }
                if (state == ItemState.CanNotUnlock)
                {
                    _iconlock.SetActive(true);

                    _unlock.SetActive(false);
                    _levelUpgradeHolder.gameObject.SetActive(false);
                    _levelUpFee.SetActive(false);
                }

            }).AddTo(this);
        }

        private void SetLevelUpgrade()
        {
            _weaponViewModel.LevelUpgradeFee.Subscribe(value =>
            {
                _valueLevelUpFee.text = ShortNumber.Get(value);
                if (value == 0)
                {
                    _levelUpFee.SetActive(false);
                }
            }).AddTo(this);

            for (int i = 0; i < _weaponInfo.LevelUpgrades.Count; i++)
            {
                var newLevelUpgrade = Instantiate(_levelUpgradePrefab, _levelUpgradeHolder);
                newLevelUpgrade.OnSetUp(_weaponInfo.LevelUpgrades[i].Id, _weaponViewModel);
            }
        }

        private void SetReloadFee()
        {
            _weaponViewModel.ReloadFee.Subscribe(value =>
            {
                _reload.SetActive(value != 0);
                _valueReload.text = ShortNumber.GetShorter(value);
            }).AddTo(this);
        }

        private void SetUnlockFee()
        {
            _valueUnlock.text = ShortNumber.GetShorter(_weaponViewModel.UnlockFee);
        }

        private void GetOnCheckList()
        {
            if (_storeWeaponHandler is ShieldStoreHandler shieldStoreHandler)
            {
                _shieldStoreHandler = shieldStoreHandler;
                _checkList.SetActive(true);
                _weaponViewModel.IsChosed.Subscribe(SubscribeCheckList).AddTo(this);
            }
        }

        private void SubscribeCheckList(bool isCheck)
        {
            if (isCheck) _iconCheckList.SetActive(true);
            else _iconCheckList.SetActive(false);
        }

        public void OnChooseShieldClicked()
        {
            _audioManager.Play(AudioKey.SFX_CLICK_02);
            _shieldStoreHandler.ChoseShield(_weaponId);
        }

        public void OnUnlockWeaponClicked()
        {
            var result = _storeWeaponHandler.UnlockNewWeapon(_weaponId);
            SetSFX(result);
        }

        public void OnLevelUpgradeClicked()
        {
            var result = _storeWeaponHandler.UpgradeNewLevelWeapon(_weaponId);
            SetSFX(result);
        }

        public void OnReloadClicked()
        {
            var result = _storeWeaponHandler.OnReloadWeapon(_weaponId);
            SetSFX(result);
        }

        private void SetSFX(ResultBuyItem result)
        {
            if (result == ResultBuyItem.Success) _audioManager.Play(AudioKey.SFX_DELETE_COIN);
            else _audioManager.Play(AudioKey.SFX_CLICK_ERROR);
        }

        private void Effect(bool isChosed)
        {
            if (!isChosed) return;

            transform.DOScale(_targetScale, _duration).OnComplete(() =>
            {
                transform.DOScale(Vector3.one, _duration);
            });
        }
    }
}