using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;
using Sources.DataBaseSystem;
using Sources.Extension;
using Sources.FTUE.Command;
using Sources.Utils;
using Sources.Utils.Singleton;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sources.FTUE.Config
{
    [Serializable]
    public class FTUESequenceTable
    {
        [SerializeField, ValueDropdown(nameof(GetAllFTUESequenceTableId))]
        private string _ftueSequenceTableId;
        public string FTUESequenceTableId => _ftueSequenceTableId;
        private IEnumerable GetAllFTUESequenceTableId => IdGetter.GetAllFTUEKeyIds();

        [SerializeField, ListDrawerSettings(ListElementLabelName = "GetDescription")] 
        public List<FTUESequenceInfo> FTUESequences;
    }

    [Serializable]
    public class FTUESequenceInfo
    {
        [SerializeField, ValueDropdown(nameof(GetComleteTriggerKey))]
        private string _completeTriggerKey;
        private IEnumerable GetComleteTriggerKey => IdGetter.GetAllFTUEKeyIds();

        [SerializeReference]
        [ListDrawerSettings(ShowPaging = false,
            ListElementLabelName = "FullDescription",
            ShowIndexLabels = true)]
        [HideReferenceObjectPicker]
        private List<FTUECommand> _ftueCommands;

        private string GetDescription()
        {
            return _completeTriggerKey;
        }

        public async UniTask Execute()
        {
            if (_completeTriggerKey == FTUEKey.CompleteTrigger_GP_ShowJoystick)
            {
                if (!IsCanShowFTUEJoystick()) return;
            }

            int index = 0;
            foreach (var command in _ftueCommands)
            {
                Debug.Log($"Start {index} {command.FullDescription}");
                await command.Execute();
                Debug.Log($"End {index} {command.FullDescription}");
                index++;
            }
        }

        private bool IsCanShowFTUEJoystick()
        {
            var dataBase = Locator<DataBase>.Instance;
            var buildConfig = dataBase.GetConfig<BuildConfig>();

            if (buildConfig.UseJoystick) return true;
            return false;
        }
    }
}