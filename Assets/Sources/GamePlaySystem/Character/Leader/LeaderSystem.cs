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
        public GunHandler GunHandler = new GunHandler();
        public ReloadTimeHandler ReloadTimeHandler = new ReloadTimeHandler();
        
        public override async UniTask Init()
        {
            GunHandler.OnSetUp();
            ReloadTimeHandler.OnSetUp();
        }
    }
}