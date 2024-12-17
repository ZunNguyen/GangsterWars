using Sources.GameData;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sources.GameData
{
    public class FTUEProfile : IProfileData
    {
        private List<string> FTUESequenceIds = new();

        public bool IsPassFTUE(string ftueSequenceId)
        {
            return FTUESequenceIds.Contains(ftueSequenceId);
        }
    }
}