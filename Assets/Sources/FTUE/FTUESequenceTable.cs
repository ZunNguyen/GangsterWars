using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sources.FTUE
{
    [Serializable]
    public class FTUESequenceTable
    {
        public List<FTUESequenceInfo> FTUEMainMenu;
    }

    [Serializable]
    public class FTUESequenceInfo
    {
        [SerializeField] private string _completeTriggerKey;

        [SerializeReference]
        [ListDrawerSettings(ShowPaging = false,
            ListElementLabelName = "FullDescription",
            ShowIndexLabels = true)]
        [HideReferenceObjectPicker]
        private List<FTUECommand> _ftueCommands;
    }
}