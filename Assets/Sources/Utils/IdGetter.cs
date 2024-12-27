using Sources.DataBaseSystem;
using Sources.Language;
using Sources.UISystem;
using System.Collections;
using System.Collections.Generic;

namespace Sources.Utils
{
    public static class IdGetter
    {
#if UNITY_EDITOR
        private static DataBase _dataBase => DataBase.EditorInstance;
        private static UIData _uiData => UIData.ActiveUIData;
        private static LanguageTable _languageTable => LanguageTable.Instance;
#endif


        public static IEnumerable GetAllFTUEKeyIds()
        {
#if UNITY_EDITOR
            var config = _dataBase.GetConfig<FTUEConfig>();
            if (config == null) return null;
            return config.FTUEKeyTables.GetAllKeyIds();
#endif
            return null;
        }

        public static IEnumerable GetAllUIName()
        {
#if UNITY_EDITOR
            var uis = _uiData.GetAllUIs();
            List<string> uisName = new ();

            foreach (var ui in uis)
            {
                uisName.Add(ui.name);
            }
            return uisName;
#endif
            return null;
        }

        public static IEnumerable GetAllLanguageIds()
        {
#if UNITY_EDITOR
            return _languageTable.LanguageItemIds;
#endif
            return null;
        }
    }
}