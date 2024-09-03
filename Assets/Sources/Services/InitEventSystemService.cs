using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Sources.Services
{
    public class InitEventSystemService : Service
    {
        public override async UniTask<IService.Result> Execute()
        {
            GameObject eventSystem = new GameObject("EventSystem");

            eventSystem.AddComponent<EventSystem>();
            eventSystem.AddComponent<StandaloneInputModule>();
            eventSystem.AddComponent<BaseInput>();

            GameObject.DontDestroyOnLoad(eventSystem);
            return IService.Result.Success;
        }
    }
}

