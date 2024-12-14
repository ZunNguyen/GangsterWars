using Sources.DataBaseSystem;
using Sources.UISystem;
using System.Collections;
using System.Collections.Generic;

namespace Sources.Utils
{
    public static class IdGetter
    {
        private static DataBase _dataBase => DataBase.EditorInstance;
        private static UIData _uiData => UIData.ActiveUIData;

        public static IEnumerable GetAllFTUEKeyIds()
        {
            var config = _dataBase.GetConfig<FTUEConfig>();
            if (config == null) return null;
            return config.FTUEKey.GetAllKeyIds();
        }

        public static IEnumerable GetAllUIName()
        {
            var uis = _uiData.GetAllUIs();
            List<string> uisName = new ();

            foreach (var ui in uis)
            {
                uisName.Add(ui.name);
            }

            return uisName;
        }
    }
}