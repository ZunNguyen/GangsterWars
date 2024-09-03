using Cysharp.Threading.Tasks;
using Sources.DataBaseSystem;
using Sources.DataBaseSystem.Leader;
using Sources.SystemService;
using Sources.Utils.Singleton;
using UniRx;

namespace Sources.GamePlaySystem.Leader
{
    public class GunModel
    {
        public string GunId;
        public string LevelDamage;
        public ReactiveProperty<int> BulletCount = new ReactiveProperty<int>(0);
    }

    public class InitLeaderSystemService : InitSystemService<LeaderSystem> { };

    public class LeaderSystem : BaseSystem
    {
        private const string _gunIdDefault = "gun-01";

        private DataBase _dataBase => Locator<DataBase>.Instance;
        private LeaderConfig _leaderConfig => _dataBase.GetConfig<LeaderConfig>();
        private GameData.GameData _gameData => Locator<GameData.GameData>.Instance;

        private string GunIdCurrent;

        public ReactiveDictionary<string, GunModel> GunModels { get; private set; } = new ();
        public ReactiveProperty<bool> IsCanShoot { get; private set; } = new ();
        
        public override async UniTask Init()
        {
            LoadGunModels();
            LoadGunCurrent();
            CheckCanShoot();
        }

        private void LoadGunModels()
        {
            var gunModels = _gameData.SetDataLeaderData();
        
            foreach(var gunModel in gunModels)
            {
                var key = gunModel.GunId;
                GunModels.Add(key, gunModel);
            }
        }

        private void LoadGunCurrent()
        {
            GunIdCurrent = _gunIdDefault;
        }

        private void CheckCanShoot()
        {
            var bulletCountCurrent = GunModels[GunIdCurrent].BulletCount.Value;
            IsCanShoot.Value = bulletCountCurrent > 0;
        }

        public void UpdateBullet()
        {
            GunModels[GunIdCurrent].BulletCount.Value -= 1;
            CheckCanShoot();
        }

        public void UpdateGunModel(string gunId)
        {
            GunIdCurrent = gunId;
        }
    }
}