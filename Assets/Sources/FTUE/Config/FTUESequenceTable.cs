using Sirenix.OdinInspector;
using Sources.FTUE.Command;
using Sources.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sources.FTUE.Config
{
    [Serializable]
    public class FTUESequenceTable
    {
        [SerializeField, ListDrawerSettings(ListElementLabelName = "GetDescription")] 
        private List<FTUESequenceInfo> FTUEMainMenu;
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
    }
}