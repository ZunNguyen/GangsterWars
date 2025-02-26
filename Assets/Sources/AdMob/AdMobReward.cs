using GoogleMobileAds.Api;
using System.Threading.Tasks;
using UnityEngine;

namespace Sources.AdMob
{
    public class AdMobReward
    {
        // test ID: 3940256099942544/5224354917
        private string _adUnitId = "ca-app-pub-3940256099942544/5224354917";

        private RewardedAd _rewardedAd;

        public async Task<bool> ShowAd()
        {
            bool isAdLoaded = await LoadRewardedAd();
            if (!isAdLoaded)
            {
                Debug.Log("Ad failed to load");
                return false;
            }

            bool isShowAd = await ShowRewardedAd();
            return isShowAd;
        }

        public async Task<bool> LoadRewardedAd()
        {
            if (_rewardedAd != null)
            {
                _rewardedAd.Destroy();
                _rewardedAd = null;
            }

            var adRequest = new AdRequest();
            var tcs = new TaskCompletionSource<bool>();

            // send the request to load the ad.
            RewardedAd.Load(_adUnitId, adRequest,
                (RewardedAd ad, LoadAdError error) =>
                {
                    // if error is not null, the load request failed.
                    if (error != null || ad == null)
                    {
                        Debug.LogError("Rewarded ad failed to load an ad " + "with error : " + error);
                        tcs.SetResult(false);
                    }
                    else
                    {
                        Debug.Log("Rewarded ad loaded with response : " + ad.GetResponseInfo());
                        _rewardedAd = ad;
                        tcs.SetResult(true);
                    }
                });
            return await tcs.Task;
        }

        public async Task<bool> ShowRewardedAd()
        {
            if (_rewardedAd == null && !_rewardedAd.CanShowAd()) return false;

            var tcs = new TaskCompletionSource<bool>();

            _rewardedAd.Show((Reward reward) =>
            {
                tcs.SetResult(true);
            });

            return await tcs.Task;
        }
    }
}