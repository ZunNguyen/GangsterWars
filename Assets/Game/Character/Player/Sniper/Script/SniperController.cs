using Sources.GamePlaySystem.MainMenuGame;
using Sources.Utils.Singleton;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Character.Sniper
{
    public class SniperController : MonoBehaviour
    {
        private OpenCharacterSystem _openCharacterSystem => Locator<OpenCharacterSystem>.Instance;

        private void Awake()
        {
            if (!_openCharacterSystem.OpenSniperHandler.IsAldreadyOpenCharacter) gameObject.SetActive(false);
        }
    }
}