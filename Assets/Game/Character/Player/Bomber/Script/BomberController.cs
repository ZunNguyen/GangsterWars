using Sources.GamePlaySystem.MainMenuGame;
using Sources.Utils.Singleton;
using Unity.VisualScripting;
using UnityEngine;

namespace Game.Character.Bomber
{
    public class BomberController : MonoBehaviour
    {
        private OpenCharacterSystem _openCharacterSystem => Locator<OpenCharacterSystem>.Instance;

        [SerializeField] private BomberActionHandler _bomberActionHandler;
        [SerializeField] private BomberAnimationHandler _bomberAnimationHandler;

        private void Awake()
        {
            if (!_openCharacterSystem.OpenBomberHandler.IsAldreadyOpenCharacter) gameObject.SetActive(false);
            else
            {
                _bomberActionHandler.OnSetUp();
                _bomberAnimationHandler.OnSetUp();
            }
        }
    }
}