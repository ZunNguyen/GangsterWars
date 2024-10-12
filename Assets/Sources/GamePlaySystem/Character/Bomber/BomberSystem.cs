using Cysharp.Threading.Tasks;
using Sources.SystemService;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sources.GamePlaySystem.Bomber
{
    public class InitBomberSystemService : InitSystemService<BomberSystem> { };

    public class BomberSystem : BaseSystem
    {
        public ReloadTimeHandler ReloadTimeHandler = new ();
        public BomHandler BomHandler = new ();

        public override async UniTask Init()
        {
            ReloadTimeHandler.OnSetUp();
            BomHandler.OnSetUp();
        }
    }
}