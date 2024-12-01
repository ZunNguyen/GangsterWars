using Sources.DataBaseSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sources.GamePlaySystem.MainMenuGame
{
    public class OpenBomberHandler : OpenCharacterHandlerAbstract
    {
        private BomberConfig _bomberConfig => _dataBase.GetConfig<BomberConfig>();

        public override void OnSetUp()
        {
            CheckCharacterData(_userProfile.BomberDatas, _bomberConfig.OpenFee);
        }

        protected override void SetDataCharacter()
        {
            _userProfile.SetBomberDataDefault();
            _storeSystem.SetBomberStore();
        }
    }
}