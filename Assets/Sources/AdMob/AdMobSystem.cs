using Cysharp.Threading.Tasks;
using Sources.SystemService;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace Sources.AdMob
{
    public class InitAdMobSystemService : InitSystemService<AdMobSystem> { }

    public class AdMobSystem : BaseSystem
    {
        private AdMobReward _adMobReward = new();

        public override async UniTask Init(){}

        public async Task<bool> ShowAdReward()
        {
            bool isShowAd = await _adMobReward.ShowAd();
            return isShowAd;
        }
    }
}