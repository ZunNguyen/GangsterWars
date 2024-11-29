using Cysharp.Threading.Tasks;
using Game.Character.Player.Abstract;
using Sources.Audio;
using Sources.Extension;
using Sources.Utils.Singleton;
using UnityEngine;

namespace Game.Character.Leader
{
    public class PosSpawnGun02Handler : PosSpawnBulletHandlerAbstract
    {
        private readonly Vector3 _offsetTargetPosMouseClick = new Vector3(0.5f, 0.5f, -1);

        private AudioManager _audioManager => Locator<AudioManager>.Instance;

        protected override void OnSetUp()
        {
            _gunId = LeaderKey.GunId_02;
        }

        protected override async void Shooting()
        {
            if (!_isCanShoot) return;

            SpawnMuzzleFlash();
            Vector3 clickPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            clickPosition.z = -1;

            _audioManager.Play(AudioKey.SFX_SHOOT_PISTOL);
            SpawnBullet(_posSpawns[0].transform, clickPosition);

            await UniTask.DelayFrame(2);

            _audioManager.Play(AudioKey.SFX_SHOOT_PISTOL);
            clickPosition += _offsetTargetPosMouseClick;
            SpawnBullet(_posSpawns[1].transform, clickPosition);
        }
    }
}