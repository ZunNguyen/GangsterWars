using BestHTTP.Extensions;
using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;
using Sources.Utils.Singleton;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Sources.Language
{
    [RequireComponent(typeof(Text))]
    public class LanguageText : MonoBehaviour
    {
        private LanguageTable _languageTable => Locator<LanguageTable>.Instance;

        private Text _text;

        [SerializeField]
        [ValueDropdown("GetAllLanguageItemIds"), OnValueChanged("UpdateLanguageItem")]
        private string _languageItemId;

        [SerializeField, InlineEditor, ReadOnly]
        private LanguageItem _languageItem;

        private IEnumerable<string> GetAllLanguageItemIds()
        {
            return LanguageTable.Instance.LanguageItemIds;
        }

        private void UpdateLanguageItem()
        {
            _languageItem = LanguageTable.Instance.GetLanguageItem(_languageItemId);
        }

        private async void Awake()
        {
            _text = GetComponent<Text>();

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