using Cysharp.Threading.Tasks;
using Sources.GameData;
using Sources.SystemService;
using Sources.Utils.Singleton;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sources.GamePlaySystem.Character
{
    public class InitSniperSystemService : InitSystemService<SniperSystem> { };

    public class SniperSystem : BaseSystem
    {
        private const float _timeReload = 4f;

        private GameData.GameData _gameData => Locator<GameData.GameData>.Instance;
        private UserProfile _userProfile => _gameData.GetProfileData<UserProfile>();

        public ReloadTimeHandler ReloadTimeHandler = new();
        public WeaponHandler BomHandler = new();

        public override async UniTask Init()
        {
            ReloadTimeHandler.OnSetUp(_timeReload);
            BomHandler.OnSetUp(_userProfile.SniperDatas, ReloadTimeHandler);
        }
    }
}