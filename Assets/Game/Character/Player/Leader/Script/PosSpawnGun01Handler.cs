using Game.Character.Player.Abstract;
using Game.Effect.MuzzleFlash;
using Game.Screens.GamePlayScreen;
using Sources.Audio;
using Sources.Extension;
using Sources.SpawnerSystem;
using Sources.Utils;
using Sources.Utils.Singleton;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Character.Leader
{
    public class PosSpawnGun01Handler : PosSpawnBulletHandlerAbstract
    {
        private AudioManager _audioManager => Locator<AudioManager>.Instance;

        protected override void OnSetUp()
        {
            _gunId = LeaderKey.GUN_ID_DEFAULT;
        }

        protected override void Shooting()
        {
            if (!_isCanShoot) return;

            _audioManager.Play(AudioKey.SFX_SHOOT_PISTOL);
            SpawnMuzzleFlash();

            Vector3 clickPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            clickPosition.z = -1;

            SpawnBullet(_posSpawns[0].transform, clickPosition);
        }
    }
}