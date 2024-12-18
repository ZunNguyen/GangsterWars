using Cysharp.Threading.Tasks;
using Sources.DataBaseSystem;
using Sources.GameData;
using Sources.SystemService;
using Sources.Utils.Singleton;
using System;
using System.Collections.Generic;

namespace Sources.FTUE.System
{
    public class InitFTUESystemService : InitSystemService<FTUESystem> { }

    public class FTUESystem : BaseSystem
    {
        private GameData.GameData _gameData => Locator<GameData.GameData>.Instance;
        private FTUEProfile _ftueProfile => _gameData.GetProfileData<FTUEProfile>();

        private DataBase _dataBase => Locator<DataBase>.Instance;
        private FTUEConfig _ftueConfig => _dataBase.GetConfig<FTUEConfig>();

        private bool _isTapToNextStep;
        private List<string> _ftueTriggerIds = new();

        public Action SpawnEnemyFTUE;
        public Action PassFTUE;

        public override async UniTask Init()
        {
            InitFTUEMainMenu();
        }

        private async void InitFTUEMainMenu()
        {
            var sequenceTables = _ftueConfig.FTUESequenceTables;
            foreach (var sequenceTable in sequenceTables)
            {
                if (!_ftueProfile.IsPassFTUE(sequenceTable.FTUESequenceTableId))
                {
                    var ftueSequences = sequenceTable.FTUESequences;
                    foreach (var ftueSequence in ftueSequences)
                    {
                        await ftueSequence.Execute();
                    }

                    _ftueProfile.AddFTUEIdPass(sequenceTable.FTUESequenceTableId);
                }
            }

            PassFTUE?.Invoke();
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

        public void SpawnFTUEEnemy()
        {
            SpawnEnemyFTUE?.Invoke();
        }
    }
}