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
        private int totalServices;
        private string _nameServiceGroup;

        protected override string _nameService => _nameServiceGroup;
        
        public FloatReactiveProperty Progress { get; private set; } = new FloatReactiveProperty();

        public SequenceServiceGroup(string nameServiceGroup)
        {
            _nameServiceGroup = nameServiceGroup;
        }

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

