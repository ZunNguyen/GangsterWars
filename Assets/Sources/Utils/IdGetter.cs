using Sources.DataBaseSystem;
using Sources.Language;
using Sources.UISystem;
using System.Collections;
using System.Collections.Generic;

namespace Sources.Utils
{
    public static class IdGetter
    {
        private static DataBase _dataBase => DataBase.EditorInstance;
        private static UIData _uiData => UIData.ActiveUIData;
        private static LanguageTable _languageTable => LanguageTable.Instance;

        public static IEnumerable GetAllFTUEKeyIds()
        {
            var config = _dataBase.GetConfig<FTUEConfig>();
            if (config == null) return null;
            return config.FTUEKeyTables.GetAllKeyIds();
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

        public static IEnumerable GetAllLanguageIds()
        {
            return _languageTable.LanguageItemIds;
        }
    }
}