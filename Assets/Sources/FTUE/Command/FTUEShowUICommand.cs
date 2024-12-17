using Cysharp.Threading.Tasks;
using Game.Screens.UIBlockerScreen;
using Sirenix.OdinInspector;
using Sources.UISystem;
using Sources.Utils;
using Sources.Utils.Singleton;
using System;
using System.Collections;
using UnityEngine;

namespace Sources.FTUE.Command
{
    [Serializable]
    public class FTUEShowUICommand : FTUECommand
    {
        private UIManager _uiManager => Locator<UIManager>.Instance;

        [SerializeField, ValueDropdown(nameof(_getAllUIName))]
        private string _uiTypeName;
        private IEnumerable _getAllUIName => IdGetter.GetAllUIName();

        [SerializeField] private bool _isOpen;

        public override string Description => $"Show UI {_uiTypeName}";

        public override async UniTask Execute()
        {
            var uiTargetShow = _uiManager.GetUIShowing(_uiTypeName);

            if (_isOpen)
            {
                if (uiTargetShow == null) await _uiManager.Show(_uiTypeName);
            }
            else
            {
                if (uiTargetShow != null) await _uiManager.Close(_uiTypeName);
            }
        }
    }
}