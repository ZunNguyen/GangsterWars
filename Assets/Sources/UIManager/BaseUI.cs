using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sources.UIManager
{
    public class BaseUI : MonoBehaviour
    {
        [ValueDropdown("FetchAllUILayers")]
        [SerializeField]
        private string layer;
        public string Layer => layer;

        private List<string> FetchAllUILayers()
        {
            return UIManager.ActiveUIData.GetUILayers();
        }
    }
}

