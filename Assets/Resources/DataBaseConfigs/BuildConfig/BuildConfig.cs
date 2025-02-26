using Sirenix.OdinInspector;
using Sources.DataBaseSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Sources.DataBaseSystem
{
#if UNITY_EDITOR
    [Serializable]
    public class EarnCoinBuildInfo
    {
        public string Id;
        public int Value;
        [Header("Seconds")]
        public int TimeToReload;
    }
#endif

    public class BuildConfig : DataBaseConfig
    {
        // For Use Joystick
        [Header("For Use Joystick")]
        public bool UseJoystick;

#if UNITY_EDITOR
        private DataBase _dataBase => DataBase.EditorInstance;
        private EarnCoinConfig _earnCoinConfig => _dataBase.GetConfig<EarnCoinConfig>();

        // For Ads
        [Header("For Ads")]

        [OnValueChanged(nameof(OnBuildWithAdsChanged))]
        public bool BuildWithAds;
        [SerializeField, ShowIf(nameof(BuildWithAds))] 
        private List<EarnCoinBuildInfo> _forWithAds;

        [OnValueChanged(nameof(OnBuildNoAdsChanged))]
        public bool BuildNoAds;
        [SerializeField, ShowIf(nameof(BuildNoAds))]
        private List<EarnCoinBuildInfo> _forNoAds;

        private void OnBuildWithAdsChanged()
        {
            if (BuildWithAds)
            {
                BuildNoAds = false;
                _earnCoinConfig.OnChangeBuildData(_forWithAds);
            }
        }

        private void OnBuildNoAdsChanged()
        {
            if (BuildNoAds)
            {
                BuildWithAds = false;
                _earnCoinConfig.OnChangeBuildData(_forNoAds);
            }
        }
#endif
    }
}