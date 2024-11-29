using Game.Character.Player.Abstract;
using Sources.Audio;
using Sources.Extension;
using Sources.Utils;
using Sources.Utils.Singleton;
using UnityEngine;

namespace Game.Character.Leader
{
    public class PosSpawnGun05Handler : PosSpawnBulletHandlerAbstract
    {
        private const float _offsetMousClickPosX = 0.2f;
        private const float _offsetMousClickPosY = 0.5f;

        private AudioManager _audioManager => Locator<AudioManager>.Instance;

        protected override void OnSetUp()
        {
            _gunId = LeaderKey.GunId_05;
        }

        protected override void Shooting()
        {
            if (!_isCanShoot) return;

            _audioManager.Play(AudioKey.SFX_SHOOT_GUN_MACHINE);
            SpawnMuzzleFlash();

            Vector3 clickPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            clickPosition.z = -1;

            clickPosition.x = GetRandom.GetRandomFloat(clickPosition.x - _offsetMousClickPosX, clickPosition.x + _offsetMousClickPosX);
            clickPosition.y = GetRandom.GetRandomFloat(clickPosition.y - _offsetMousClickPosY, clickPosition.y + _offsetMousClickPosY);

            SpawnBullet(_posSpawns[0].transform, clickPosition);
        }
    }
}