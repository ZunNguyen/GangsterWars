using Cysharp.Threading.Tasks;
using DG.Tweening;
using Sirenix.OdinInspector;
using Sources.DataBaseSystem;
using Sources.UISystem;
using Sources.Utils.Singleton;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Sources.UI
{
    public class BaseUI : MonoBehaviour
    {
        [ValueDropdown(nameof(FetchAllUILayers))]
        [SerializeField]
        private string layer;

        private UITransitionHandler _uiTransitionHandler;

        private void Awake()
        {
            _uiTransitionHandler = GetComponent<UITransitionHandler>();
        }

        private List<string> FetchAllUILayers()
        {
            return UIData.ActiveUIData.GetUILayers();
        }
    }
}

