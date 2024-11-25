using Game.Character.Player.Abstract;
using Game.Effect.MuzzleFlash;
using Game.Screens.GamePlayScreen;
using Sources.Extension;
using Sources.SpawnerSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Character.Leader
{
    public class PosSpawnGun01Handler : PosSpawnBulletHandlerAbstract
    {
        private readonly Vector3 _offsetTargetPosMouseClick = new Vector3(0.3f, 0.3f, -1);

        private Vector3 _offsetTargetPosMouseClickCurrent = Vector3.zero;
        private bool _isChangeSign = false;
        private int _countPosSpawn = 0;

        protected override void OnSetUp()
        {
            _gunId = LeaderKey.GunId_Default;
        }

        protected override void Shooting()
        {
            if (!_isCanShoot) return;
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

            _offsetTargetPosMouseClickCurrent = Vector3.zero;
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