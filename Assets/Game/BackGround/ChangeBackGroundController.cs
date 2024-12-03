using Sources.DataBaseSystem;
using Sources.GamePlaySystem.MainGamePlay;
using Sources.Utils.Singleton;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.BackGround
{
    public class ChangeBackGroundController : MonoBehaviour
    {
        private DataBase _dataBase => Locator<DataBase>.Instance;
        private WavesConfig _backGroundConfig => _dataBase.GetConfig<WavesConfig>();

        private MainGamePlaySystem _mainGamePlaySystem => Locator<MainGamePlaySystem>.Instance;

        [SerializeField] private SpriteRenderer _sprite;

        private void Awake()
        {
            var waveId = _mainGamePlaySystem.WaveIdCurrent;
            var waveInfo = _backGroundConfig.GetBGWaveInfo(waveId);
            _sprite.sprite = waveInfo.Sprite;
        }
    }
}