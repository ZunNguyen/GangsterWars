using Sources.DataBaseSystem;
using Sources.FTUE.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Sources.DataBaseSystem
{
    public class FTUEConfig : DataBaseConfig
    {
        public List<FTUESequenceTable> FTUESequenceTables;
        public FTUEKeyTable FTUEKeyTables;
    }
}