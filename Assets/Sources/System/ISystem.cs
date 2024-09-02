using Cysharp.Threading.Tasks;
using Sources.Services;
using Sources.Utils.Singleton;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sources.SystemService
{
    public interface ISystem
    {
        public UniTask Init();
    }

    public abstract class BaseSystem : ISystem
    {
        public abstract UniTask Init();
    }

    public class InitSystemService<T> : Service where T : BaseSystem, new()
    {
        public override async UniTask<IService.Result> Execute()
        {
            var system = Locator<T>.Set(new T());
            await system.Init();

            return IService.Result.Success;
        }
    }
}

