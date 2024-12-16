using Cysharp.Threading.Tasks;
using Sources.DataBaseSystem;
using Sources.SystemService;
using Sources.Utils.Singleton;
using System.Collections.Generic;

namespace Sources.FTUE.System
{
    public class InitFTUESystemService : InitSystemService<FTUESystem> { }

    public class FTUESystem : BaseSystem
    {
        private DataBase _dataBase => Locator<DataBase>.Instance;
        private FTUEConfig _ftueConfig => _dataBase.GetConfig<FTUEConfig>();

        private bool _isTapToNextStep;
        private List<string> _ftueTriggerIds = new();

        public override async UniTask Init()
        {
            InitFTUEMainMenu();
        }

        private async void InitFTUEMainMenu()
        {
            var sequences = _ftueConfig.FTUESequenceTables.FTUEMainMenu;
            foreach (var sequence in sequences)
            {
                await sequence.Execute();
            }
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

        public void TriggerWaitPoint(string triggerId)
        {
            if (string.IsNullOrEmpty(triggerId)) return;

            if (!_ftueTriggerIds.Contains(triggerId)) _ftueTriggerIds.Add(triggerId);
        }

        public void TriggerWaitToNextStep()
        {
            _isTapToNextStep = true;
        }
    }
}