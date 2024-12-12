using Cysharp.Threading.Tasks;
using Sources.Command;
using System;

namespace Sources.FTUE
{
    public interface IFTUECommand : ICommand
    {
        public abstract string Description { get; }
    }

    [Serializable]
    public abstract class FTUECommand : IFTUECommand
    {
        public string FullDescription => Description;
        public abstract string Description { get; }
        public abstract UniTask Execute();
    }
}