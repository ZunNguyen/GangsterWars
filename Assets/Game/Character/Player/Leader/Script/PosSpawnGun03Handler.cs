using Game.Character.Player.Abstract;
using Sources.Audio;
using Sources.Extension;
using Sources.Utils.Singleton;
using UnityEngine;

namespace Game.Character.Leader
{
    public class PosSpawnGun03Handler : PosSpawnBulletHandlerAbstract
    {
        private readonly Vector3 _offsetTargetPosShoot = new Vector3(0.2f, 0.2f, -1);

        private AudioManager _audioManager => Locator<AudioManager>.Instance;

        private Vector3 _offsetTargetPosShootCurrent = Vector3.zero;
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
                var posShoot = LeaderAction.Instance.PosShoot;

                posShoot += _offsetTargetPosShootCurrent;

                SpawnBullet(pos, posShoot);

                ++_countPosSpawn;
                UpdateTargetPosShoot();
            }

            _countPosSpawn = 0;
            _offsetTargetPosShootCurrent = Vector3.zero;
            _isChangeSign = false;
        }

        private void UpdateTargetPosShoot()
        {
            if (_isChangeSign)
            {
                _offsetTargetPosShootCurrent -= _countPosSpawn * _offsetTargetPosShoot;
            }
            else
            {
                _offsetTargetPosShootCurrent += _countPosSpawn * _offsetTargetPosShoot;
            }

            _isChangeSign = !_isChangeSign;
        }
    }
}