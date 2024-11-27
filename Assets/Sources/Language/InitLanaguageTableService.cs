using Cysharp.Threading.Tasks;
using Sources.Services;
using Sources.Utils.Singleton;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sources.Language
{
    public class InitLanaguageTableService : Service
    {
        private LanguageTable _languageTable;

        public InitLanaguageTableService(LanguageTable languageTable)
        {
            _languageTable = languageTable;
        }

        public override async UniTask<IService.Result> Execute()
        {
            Locator<LanguageTable>.Set(_languageTable);
            return IService.Result.Success;
        }
    }
}