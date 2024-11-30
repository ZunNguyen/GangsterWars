using Game.Character.Player.Abstract;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Character.Sniper.Variant
{
    public class OpenSniperHandler : OpenCharacterHandler
    {
        private void Start()
        {
            OnSetUp(_openCharacterSystem.OpenSniperHandler);
        }
    }
}