using Game.Character.Player.Abstract;
using Sources.Audio;
using Sources.Extension;
using Sources.Utils.Singleton;
using UnityEngine;

namespace Game.Character.Leader
{
    public class PosSpawnGun03Handler : PosSpawnBulletHandlerAbstract
    {
        private readonly Vector3 _offsetTargetPosMouseClick = new Vector3(0.2f, 0.2f, -1);

        private AudioManager _audioManager => Locator<AudioManager>.Instance;

        private Vector3 _offsetTargetPosMouseClickCurrent = Vector3.zero;
        private bool _isChangeSign = false;
        private int _countPosSpawn = 0;

        protected override void OnSetUp()
        {
            _gunId = LeaderKey.GunId_03;
        }

        protected override void Shooting()
        {
            if (!_isCanShoot) return;

            _audioManager.Play(AudioKey.SFX_SHOOT_SHOOTGUN);
            SpawnMuzzleFlash();

            foreach (var pos in _posSpawns)
            {
                Vector3 clickPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                clickPosition.z = -1;

                clickPosition += _offsetTargetPosMouseClickCurrent;

                SpawnBullet(pos, clickPosition);

                ++_countPosSpawn;
                UpdateTargetPosMouseClick();
            }

            _countPosSpawn = 0;
            _offsetTargetPosMouseClickCurrent = Vector3.zero;
            _isChangeSign = false;
        }

        private void UpdateTargetPosMouseClick()
        {
            if (_isChangeSign)
            {
                _offsetTargetPosMouseClickCurrent -= _countPosSpawn * _offsetTargetPosMouseClick;
            }
            else
            {
                _offsetTargetPosMouseClickCurrent += _countPosSpawn * _offsetTargetPosMouseClick;
            }

            _isChangeSign = !_isChangeSign;
        }
    }
}