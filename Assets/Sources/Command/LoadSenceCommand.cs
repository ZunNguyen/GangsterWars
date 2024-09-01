using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Sources.Command
{
    public class LoadSenceCommand : Command
    {
        private readonly string _scenceName;

        public LoadSenceCommand(string sceneName)
        {
            _scenceName = sceneName;
        }

        public override async UniTask Execute()
        {
            await SceneManager.LoadSceneAsync(_scenceName, LoadSceneMode.Single);
        }
    }
}