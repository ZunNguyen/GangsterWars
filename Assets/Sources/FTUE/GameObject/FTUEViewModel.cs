using Sirenix.OdinInspector;
using Sources.Language;
using Sources.Utils;
using Sources.Utils.Singleton;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Sources.FTUE.GameObject
{
    [Serializable]
    public class ObjectData
    {
        [SerializeField] private UnityEngine.GameObject _object;
        public UnityEngine.GameObject Object => _object;

        [SerializeField] private Transform _holder;
        public Transform Holder => _holder;

        [SerializeField] private bool _isLanguageText;
        public bool IsLanguageText => _isLanguageText;

        [SerializeField, ShowIf(nameof(_isLanguageText))]
        private TMP_Text _languageText;
        
        [HideInInspector]
        public bool IsActiveSelf;

        public void SetTextLanguage(string text)
        {
            _languageText.text = text;
        }
    }

    public class FTUEViewModel : FTUEViewModelBase
    {
        private LanguageTable _languageTable => Locator<LanguageTable>.Instance;

        [SerializeField] private List<ObjectData> _objectDatas;
        public List<ObjectData> ObjectDatas => _objectDatas;

        private void Awake()
        {
            foreach (var objectData in _objectDatas)
            {
                objectData.IsActiveSelf = objectData.Object.activeSelf;
            }
        }

        public void BackViewModelOrigin()
        {
            foreach (var objetcData in _objectDatas)
            {
                objetcData.Object.transform.SetParent(objetcData.Holder);
                objetcData.Object.SetActive(objetcData.IsActiveSelf);
            }
        }

        public void SetLanguage(string languageId)
        {
            var languageItem = _languageTable.GetLanguageItem(languageId);

            foreach (var objetcData in _objectDatas)
            {
                if (objetcData.IsLanguageText)
                {
                    objetcData.SetTextLanguage(languageItem.GetText());
                }
            }
        }
    }
}