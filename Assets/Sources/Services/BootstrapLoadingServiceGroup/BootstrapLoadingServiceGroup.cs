using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sources.Services.BootstrapLoadingService
{
    public class BootstrapLoadingServiceGroup : SequenceServiceGroup
    {
        public override UniTask<IService.Result> Excute()
        {
            return base.Excute();
        }
    }
}