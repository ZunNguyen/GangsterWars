using Sources.DataBaseSystem;
using System.Collections;

namespace Sources.Utils
{
    public static class IdGetter
    {
        private static DataBase _dataBase => DataBase.EditorInstance;

        public static IEnumerable GetAllFTUEKeyIds()
        {
            var config = _dataBase.GetConfig<FTUEConfig>();
            if (config == null) return null;
            return config.FTUEKey.GetAllKeyIds();
        }
    }
}