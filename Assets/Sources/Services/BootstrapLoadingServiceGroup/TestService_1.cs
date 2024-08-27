using Cysharp.Threading.Tasks;
using Sources.Services;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestService_1 : Service
{
    public override async UniTask<IService.Result> Execute()
    {
        await UniTask.Delay(1000);
        return IService.Result.Success;
    }
}
