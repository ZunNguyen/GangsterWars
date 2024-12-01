using Sources.DataBaseSystem;
using Sources.GameData;
using Sources.GamePlaySystem.CoinController;
using Sources.Utils.Singleton;
using System.Collections.Generic;
using UnityEngine;

namespace Sources.GamePlaySystem.MainMenuGame
{
    public abstract class OpenCharacterHandlerAbstract
    {
        private GameData.GameData _gameData => Locator<GameData.GameData>.Instance;
        protected UserProfile _userProfile => _gameData.GetProfileData<UserProfile>();
        protected DataBase _dataBase => Locator<DataBase>.Instance;

        private CoinControllerSystem _coinController => Locator<CoinControllerSystem>.Instance;
        protected StoreSystem _storeSystem => Locator<StoreSystem>.Instance;

        public int CharacterFee { get; private set; }
        public bool IsAldreadyOpenCharacter { get; private set; } = false;

        public abstract void OnSetUp();

        protected void CheckCharacterData(List<WeaponData> characterDatas, int characterFee)
        {
            if (characterDatas != null)
            {
                IsAldreadyOpenCharacter = true;
                CharacterFee = characterFee;
            }
        }

        public bool OpenCharacter()
        {
            var result = _coinController.PurchaseItem(CharacterFee);
            if (result == true) SetDataCharacter();
            return result;
        }

        protected abstract void SetDataCharacter();
    }
}