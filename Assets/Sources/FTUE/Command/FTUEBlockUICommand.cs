using Cysharp.Threading.Tasks;
using System;

namespace Sources.FTUE.Command
{
    [Serializable]
    public class FTUEBlockUICommand : FTUECommand
    {
        public override string Description => GetType().Name;

        public override UniTask Execute()
        {
            throw new global::System.NotImplementedException();
        }
    }
}