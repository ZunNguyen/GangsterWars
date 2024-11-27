using Sources.DataBaseSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Sources.UI;
using Sources.Language;
using Sources.Utils.Singleton;

namespace Game.Screens.PanelSettingDialog
{
    public class PanelSettingDialog : BaseUI
    {
        private LanguageTable _languageTable => Locator<LanguageTable>.Instance;

        public void OnChangeLanguageClicked()
        {
            _languageTable.ChangeNexLanguageName();
        }
    }
}