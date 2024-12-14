using Cysharp.Threading.Tasks;
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

        public override string Description => $"Show ui {_uiTypeName}";

        public override async UniTask Execute()
        {
            var uiTargetShow = _uiManager.GetUIShowing(_uiTypeName);

            if (uiTargetShow != null) return;
            else await _uiManager.Show(_uiTypeName);
        }
    }
}