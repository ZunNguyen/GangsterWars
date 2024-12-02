using Sources.GamePlaySystem.MainMenuGame;
using Sources.Utils.Singleton;
using Unity.VisualScripting;
using UnityEngine;

namespace Game.Character.Bomber
{
    public class BomberController : MonoBehaviour
    {
        private OpenCharacterSystem _openCharacterSystem => Locator<OpenCharacterSystem>.Instance;

        private void Awake()
        {
            if (!_openCharacterSystem.OpenBomberHandler.IsAldreadyOpenCharacter) gameObject.SetActive(false);
        }
    }
}