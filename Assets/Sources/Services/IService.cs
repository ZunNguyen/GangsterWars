using Cysharp.Threading.Tasks;
using System;
using System.Drawing;
using UnityEngine;

namespace Sources.Services
{
    public interface IService
    {
        public enum Result
        {
            Success,
            Failure
        }

        UniTask<Result> Execute();
    }

    public abstract class Service : IService
    {
        private float _starTime;
        protected virtual string _nameService => GetType().Name;
        public abstract UniTask<IService.Result> Execute();

        public async UniTask Run()
        {
            _starTime = Time.realtimeSinceStartup;

            var result = IService.Result.Failure;
            try
            {
                result = await Execute();
            }
            catch (Exception e)
            {
                Debug.LogError($"{_nameService}: {e}");
            }
            Finish(result);
        }

        private void Finish(IService.Result result)
        {
            var finishTime = Time.realtimeSinceStartup;
            var deltaTime = finishTime - _starTime;

            var isNameServiceGroup = GetType().Name != _nameService;
            var colorNameService = isNameServiceGroup ? "<color=white>" : "<color=orange>";

            if (result == IService.Result.Success)
                Debug.Log($"<color=yellow>[Service]</color> {colorNameService}{_nameService}</color>\n" +
                    $"Finish: <color=cyan>{deltaTime}</color>");

            if (result == IService.Result.Failure)
                Debug.Log($"<color=yellow>[Service]</color> <color=blue>{_nameService}</color> : <color=red>failse</color>");
        }
    }
}

