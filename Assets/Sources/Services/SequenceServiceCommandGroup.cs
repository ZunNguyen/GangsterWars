using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

namespace Sources.Services
{
    public class SequenceServiceCommandGroup : Service
    {
        private Queue<Command.Command> _commandQueue = new Queue<Command.Command>();
        private string _commandName;
        protected override string _nameService => _commandName;
        public FloatReactiveProperty Progress { get; private set; } = new FloatReactiveProperty(); 

        public SequenceServiceCommandGroup(string commandName)
        {
            _commandName = commandName;
        }

        public void Add(Command.Command command)
        {
            _commandQueue.Enqueue(command);
        }

        public override async UniTask<IService.Result> Execute()
        {
            Progress.Value = 0;
            var totalCommand = _commandQueue.Count;

            while (_commandQueue.Count != 0)
            {
                var command = _commandQueue.Dequeue();
                try
                {
                    await command.Execute();
                }
                catch (Exception e)
                {
                    Debug.LogError(e);
                }

                Progress.Value = 1 - ((float)_commandQueue.Count / totalCommand);
            }

            return IService.Result.Success;
        }
    }
}