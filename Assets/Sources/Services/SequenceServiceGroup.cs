using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

namespace Sources.Services
{
    public class SequenceServiceGroup : Service
    {
        private Queue<Service> _queueService = new Queue<Service>();
        public FloatReactiveProperty Progress {  get; private set; } = new FloatReactiveProperty();
        private int totalServices;

        public void Add(Service service)
        {
            _queueService.Enqueue(service);
        }

        public override async UniTask<IService.Result> Execute()
        {
            Progress.Value = 0;
            totalServices = _queueService.Count;
            while (_queueService.Count != 0)
            {
                var service = _queueService.Dequeue();
                await service.Run();
                Progress.Value = 1 - ((float)_queueService.Count / totalServices);
            }
            return IService.Result.Success;
        }
    }
}

