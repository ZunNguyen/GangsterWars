using Cysharp.Threading.Tasks;
using System;

namespace Sources.FTUE
{
    [Serializable]
    public class FTUEShowUICommand : FTUECommand
    {
        public override string Description => GetType().Name;

        public override UniTask Execute()
        {
            throw new System.NotImplementedException();
        }
    }
}