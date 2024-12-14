using Cysharp.Threading.Tasks;
using Sources.SystemService;
using System.Collections.Generic;

namespace Sources.FTUE.System
{
    public class InitFTUESystemService : InitSystemService<FTUESystem> { }

    public class FTUESystem : BaseSystem
    {
        private bool _isTapToNextStep;
        private List<string> _ftueTriggerIds;

        public override UniTask Init()
        {
            throw new global::System.NotImplementedException();
        }

        public async UniTask WaitForAtPoint(string triggerId)
        {
            await UniTask.WaitUntil(() => _ftueTriggerIds.Contains(triggerId));
        }

        public async UniTask WaitTapToNextStep()
        {
            _isTapToNextStep = false;
            await UniTask.WaitUntil(() => _isTapToNextStep == true);
        }
    }
}