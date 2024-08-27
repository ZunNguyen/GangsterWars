using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sources.Command
{
    public interface ICommand
    {
        UniTask Execute();
    }

    public abstract class Command : ICommand
    {
        public abstract UniTask Execute();
    }
}