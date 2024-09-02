using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D.Animation;

namespace Game.Character.Leader
{
    public class LeaderController : MonoBehaviour
    {
        private SpriteLibraryAsset asset;
        [SerializeField] private SpriteLibrary _spriteLibrary;

        private void ChangeSpriteLibrary()
        {
            _spriteLibrary.spriteLibraryAsset = asset;
        }
    }
}