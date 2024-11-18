using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

namespace Game.Screens.GamePlayScreen
{
    public class Bullet : MonoBehaviour
    {
        private IDisposable _disposableBulletAvailable;
        private int _bulletNumber;

        [SerializeField] private GameObject _bulletIcon;

        public void OnSetUp(int bulletIndex, ReactiveProperty<int> bulletAvailable)
        {
            _bulletNumber = bulletIndex + 1;
            _disposableBulletAvailable?.Dispose();
            _disposableBulletAvailable = bulletAvailable.Subscribe(value =>
            {
                if (value >= _bulletNumber) TurnOnBullet();
                if (value < _bulletNumber) TurnOffBullet();
            });
        }

        public void TurnOnBullet()
        {
            _bulletIcon.SetActive(true);
        }

        public void TurnOffBullet()
        {
            _bulletIcon.SetActive(false);
        }

        private void OnDestroy()
        {
            _disposableBulletAvailable?.Dispose();
        }
    }
}