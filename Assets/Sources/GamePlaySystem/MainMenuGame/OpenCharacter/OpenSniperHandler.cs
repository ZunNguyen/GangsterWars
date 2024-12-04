using Sources.DataBaseSystem;

namespace Sources.GamePlaySystem.MainMenuGame
{
    public class OpenSniperHandler : OpenCharacterHandlerAbstract
    {
        private SniperConfig _sniperConfig => _dataBase.GetConfig<SniperConfig>();

        public override void OnSetUp()
        {
            CheckCharacterData(_userProfile.SniperDatas, _sniperConfig.OpenFee);
        }

        protected override void SetDataCharacter()
        {
            _userProfile.SetSniperDataDefault();
            _storeSystem.SetSniperStore();
        }
    }
}