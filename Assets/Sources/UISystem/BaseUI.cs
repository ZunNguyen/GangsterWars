using Sirenix.OdinInspector;
using Sources.UISystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sources.UI
{
    public class BaseUI : MonoBehaviour
    {
        [ValueDropdown("FetchAllUILayers")]
        [SerializeField]
        private string layer;
        public string Layer => layer;

        private List<string> FetchAllUILayers()
        {
            return UIData.ActiveUIData.GetUILayers();
        }
    }
}

