using Cysharp.Threading.Tasks;
using Sources.DataBaseSystem;
using Sources.GameData;
using Sources.GamePlaySystem.CoinController;
using Sources.SystemService;
using Sources.Utils.Singleton;
using UnityEngine;

namespace Sources.GamePlaySystem.MainMenuGame
{
    public class InitOpenCharacterSystem : InitSystemService<OpenCharacterSystem> { }

    public class OpenCharacterSystem : BaseSystem
    {
        public OpenBomberHandler OpenBomberHandler { get; private set; } = new();
        public OpenSniperHandler OpenSniperHandler { get; private set; } = new();

        public override async UniTask Init()
        {
            OpenBomberHandler.OnSetUp();
            OpenSniperHandler.OnSetUp();
        }
    }
}