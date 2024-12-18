using System;
using System.Collections.Generic;

namespace Sources.GameData
{
    [Serializable]
    public class FTUEProfile : IProfileData
    {
        public List<string> FTUESequenceIds { get; private set; } = new();

        public bool IsPassFTUE(string ftueSequenceId)
        {
            return FTUESequenceIds.Contains(ftueSequenceId);
        }

        public void AddFTUEIdPass(string FTUEId)
        {
            FTUESequenceIds.Add(FTUEId);
            Save();
        }
    }
}