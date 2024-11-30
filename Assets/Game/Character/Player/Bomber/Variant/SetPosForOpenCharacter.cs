using Sources.GamePlaySystem.MainMenuGame;
using Sources.Utils.Singleton;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Character.Bomber.Variant
{
    public class SetPosForOpenCharacter : MonoBehaviour
    {
        private OpenCharacterSystem _openCharacterSystem => Locator<OpenCharacterSystem>.Instance;

        [SerializeField] private Camera _camera;

        private void Awake()
        {
            _openCharacterSystem.OpenBomberHandler.SetPosCharacter(transform, _camera);
        }
    }
}