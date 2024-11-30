using Game.Character.Player.Abstract;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Character.Bomber.Variant
{
    public class OpenBomberHandler : OpenCharacterHandler
    {
        private void Start()
        {
            OnSetUp(_openCharacterSystem.OpenBomberHandler);
        }
    }
}