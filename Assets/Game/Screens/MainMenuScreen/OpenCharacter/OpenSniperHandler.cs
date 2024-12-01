namespace Game.Screens.MainMenuScreen
{
    public class OpenSniperHandler : OpenCharacterAbstract
    {
        protected override void SetValue()
        {
            _openCharacterAbastract = _openCharacterSystem.OpenSniperHandler;
            _tabCurrentState = TabState.TabSniper;
        }
    }
}