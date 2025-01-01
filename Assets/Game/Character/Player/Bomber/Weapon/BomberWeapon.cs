using Cysharp.Threading.Tasks;
using DG.Tweening;
using Game.Character.Player.Abstract;
using Sources.Audio;
using Sources.DataBaseSystem;
using Sources.Extension;
using Sources.Utils;
using Sources.Utils.Singleton;
using UnityEngine;

namespace Game.Character.Bomber
{
    public class BomberWeapon : WeaponAbstract
    {
        private const float _throwSpeed = 15f;
        private const float _height = -3f;
        private readonly Vector3 _offsetPosTarget = new Vector3(-2f,0,0);

        private DataBase _dataBase => Locator<DataBase>.Instance;
        private BomberConfig _bomberConfig => _dataBase.GetConfig<BomberConfig>();
        private AudioManager _audioManager => Locator<AudioManager>.Instance;

        private Sequence _sequence;
        private Vector3 _originalScale;

        [SerializeField] private SpriteRenderer _sprite;
        [SerializeField] private Animator _animator;
        [SerializeField] private Collider2D _collider;

        private void Awake()
        {
            _originalScale = transform.localScale;
        }

        private void SetEnabled(bool status)
        {
            _animator.enabled = status;
            _collider.enabled = status;
            transform.localScale = _originalScale;
        }

        public override void OnSetUp(string weaponId, int damage)
        {
            SetEnabled(false);
            var bomInfo = _bomberConfig.GetWeaponInfo(weaponId) as BomberWeaponInfo;
            _sprite.sprite = bomInfo.Icon;
            
            base.OnSetUp(weaponId, damage);
        }

        public override void Moving()
        {
            if (_mainGamePlaySystem.SpawnEnemiesHandler.Enemies.Count == 0) return;

            var enemyTarget = _mainGamePlaySystem.SpawnEnemiesHandler.Enemies[0];
            if (enemyTarget == null) return;

            var enemyPos = enemyTarget.transform.position;
            enemyPos += _offsetPosTarget;
            var middlePoint = GetVector.GetHightPointBetweenTwoPoint(transform.position, enemyPos, _height);

            Vector3[] path = new Vector3[]
            {
                transform.position,
                middlePoint,
                enemyPos
            };

            var duration = TweenUtils.GetTimeDuration(transform.position, enemyPos, _throwSpeed);

            _sequence = DOTween.Sequence();
            _sequence.Append(transform.DOPath(path, duration, PathType.CatmullRom).SetEase(Ease.Linear).OnComplete(OnBombHit));
            _sequence.Join(transform.DORotate(new Vector3(0, 0, -360), duration, RotateMode.FastBeyond360)
                    .SetEase(Ease.Linear));
        }

        private async void OnBombHit()
        {
            var _token = this.GetCancellationTokenOnDestroy();
            try
            {
                await UniTask.Delay(500, cancellationToken: _token);
                _audioManager.Play(AudioKey.SFX_BOOM_01);
                SetEnabled(true);
            }
            catch{ }
        }

        public void OnRelease()
        {
            _spawnerManager.Release(this);
        }

        public void OnDestroy()
        {
            _sequence.Kill();
        }
    }
}