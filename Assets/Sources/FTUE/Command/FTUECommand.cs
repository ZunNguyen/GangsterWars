using Cysharp.Threading.Tasks;
using Sources.Command;
using Sources.Utils;
using System;
using System.Collections;

namespace Sources.FTUE.Command
{
    public interface IFTUECommand : ICommand
    {
        public abstract string Description { get; }
    }

    [Serializable]
    public abstract class FTUECommand : IFTUECommand
    {
        public string FullDescription => $"[{GetType().Name}] - {Description}";
        public abstract string Description { get; }
        public abstract UniTask Execute();

        protected IEnumerable _getAllFTUEKey => IdGetter.GetAllFTUEKeyIds();
    }
}