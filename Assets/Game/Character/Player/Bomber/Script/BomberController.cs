using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Character.Bomber
{
    public class BomberController : MonoBehaviour
    {
        [SerializeField] private ActionHandler _actionHandler;
        public ActionHandler ActionHandler => _actionHandler;
    }
}