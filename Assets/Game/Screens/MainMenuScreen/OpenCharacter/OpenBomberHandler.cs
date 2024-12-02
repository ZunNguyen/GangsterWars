namespace Game.Screens.MainMenuScreen
{
    public class OpenBomberHandler : OpenCharacterAbstract
    {
        protected override void SetValue()
        {
            _openCharacterAbastract = _openCharacterSystem.OpenBomberHandler;
            _tabCurrentState = TabState.TabBom;
        }
    }
}