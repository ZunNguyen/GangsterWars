namespace Game.Screens.MainMenuScreen
{
    public class TabBomberHandler : OpenCharacterAbstract
    {
        protected override void SetValue()
        {
            _openCharacterAbastract = _openCharacterSystem.OpenBomberHandler;
            _tabCurrentState = TabState.TabBom;
        }
    }
}