using Sirenix.OdinInspector;
using Sources.Utils;
using Sources.Utils.Singleton;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Sources.Language
{
    [RequireComponent(typeof(TMP_Text))]
    public class LanguageText : MonoBehaviour
    {
        private LanguageTable _languageTable => Locator<LanguageTable>.Instance;

        private TMP_Text _text;

        [SerializeField]
        [ValueDropdown(nameof(GetAllLanguageIds)), OnValueChanged(nameof(UpdateLanguageItem))]
        private string _languageItemId;
        private IEnumerable GetAllLanguageIds => IdGetter.GetAllLanguageIds();

        [SerializeField, InlineEditor, ReadOnly]
        private LanguageItem _languageItem;

        private void UpdateLanguageItem()
        {
#if UNITY_EDITOR
            _languageItem = LanguageTable.Instance.GetLanguageItem(_languageItemId);
#endif
        }

        private void Awake()
        {
            _text = GetComponent<TMP_Text>();

            SetLanguage();
        }

        public void OnSetUp(string languageId)
        {
            _languageItemId = languageId;
            Awake();
        }

        private void SetLanguage()
        {
            if (_languageItem != null)
            {
                SetText(_languageItem);
            }
            else if (!string.IsNullOrEmpty(_languageItemId))
            {
                SetText(_languageItemId);
            }
        }

        private void SetText(string languageItemId)
        {
            _languageItem = _languageTable.GetLanguageItem(languageItemId);
            SetText(_languageItem);
        }

        private void SetText(LanguageItem languageItem)
        {
            if (languageItem == null) return;

            _languageTable.OnChangeLanguageName += ChangeLanguageName;
            ChangeLanguageName();
        }

        private void ChangeLanguageName()
        {
            _text.text = _languageItem.GetText();
        }

        private void OnDestroy()
        {
            _languageTable.OnChangeLanguageName -= ChangeLanguageName;
        }
    }
}