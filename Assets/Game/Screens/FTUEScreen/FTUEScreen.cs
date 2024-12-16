using Sources.DataBaseSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Sources.UI;
using Sources.FTUE.GameObject;
using Sources.FTUE.System;
using Sources.Utils.Singleton;

namespace Game.Screens.FTUEScreen
{
    public class FTUEScreen : BaseUI
    {
        private FTUESystem _ftueSystem => Locator<FTUESystem>.Instance;

        private bool _isCanClickNextStep = false;

        [SerializeField] private Transform _holderViewModels;
        
        public void SetUpViewModel(List<FTUEViewModel> ftueViewModels)
        {
            foreach (var model in ftueViewModels)
            {
                foreach (var objectData in model.ObjectDatas)
                {
                    objectData.GameObject.SetParent(_holderViewModels, false);
                }
            }
        }

        public void SetClickToNext()
        {
            _isCanClickNextStep = true;
        }

        public void OnNextStepClicked()
        {
            _ftueSystem.TriggerWaitToNextStep();
        }
    }
}