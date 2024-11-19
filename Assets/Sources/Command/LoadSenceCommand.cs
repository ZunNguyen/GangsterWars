using Cysharp.Threading.Tasks;
using Game.Screens.GamePlayScreen;
using Sources.UISystem;
using Sources.Utils.Singleton;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Sources.Command
{
    public class LoadSenceCommand : Command
    {
        private readonly string _scenceName;
        private UIManager _uiManager => Locator<UIManager>.Instance;

        public LoadSenceCommand(string sceneName)
        {
            _scenceName = sceneName;
        }

        public override async UniTask Execute()
        {
            await new ResetSpawnerManagerCommand().Execute();
            await SceneManager.LoadSceneAsync(_scenceName, LoadSceneMode.Single);
        }
    }
}