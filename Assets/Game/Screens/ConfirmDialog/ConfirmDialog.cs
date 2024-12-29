using Sources.DataBaseSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Sources.UI;
using Sources.Audio;
using Sources.Command;
using Sources.Extension;
using System.Runtime.InteropServices;

namespace Game.Screens.ConfirmDialog
{
    public class ConfirmDialog : BaseUI
    {
        [DllImport("__Internal")]
        private static extern void ReloadPage();

        public async void OnBackClicked()
        {
            await Close();
        }

        public async void OnConfirmClicked()
        {
            await new ResetDataCommand().Execute();
            QuitGame();
        }

        public void QuitGame()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#elif UNITY_WEBGL
            ReloadPage();
#else
            Application.Quit();
#endif
        }
    }
}