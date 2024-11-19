using Cysharp.Threading.Tasks;
using Sources.DataBaseSystem;
using Sources.DataBaseSystem.Leader;
using Sources.SystemService;
using Sources.Utils.Singleton;
using System;
using System.Collections.Generic;
using UniRx;

namespace Sources.GamePlaySystem.Leader
{
    public class InitLeaderSystemService : InitSystemService<LeaderSystem> { };

    public class LeaderSystem : BaseSystem
    {
        public GunHandler GunHandler;
        
        public override async UniTask Init()
        {
            
        }

        public void OnSetUp()
        {
            GunHandler = new GunHandler();
            GunHandler.OnSetUp();
        }
    }
}