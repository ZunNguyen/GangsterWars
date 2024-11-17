using Game.Character.Controller;
using Sources.DataBaseSystem;
using Sources.DataBaseSystem.Leader;
using Sources.Extension;
using Sources.GamePlaySystem.Leader;
using Sources.Utils.Singleton;
using System;
using UniRx;
using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.U2D.Animation;

namespace Game.Character.Leader
{
    public enum AnimationStateLeader
    {
        None,
        Idle,
        Reload,
        Shoot_4_sprite,
        Shoot_7_sprite
    }

    public static class AnimationStateLeaderEx
    {
        public static string ConvertToString(this AnimationStateLeader state)
        {
            return state switch
            {
                AnimationStateLeader.None => "None",
                AnimationStateLeader.Idle => "Idle",
                AnimationStateLeader.Reload => "Reload",
                AnimationStateLeader.Shoot_4_sprite => "Shoot_4_sprite",
                AnimationStateLeader.Shoot_7_sprite => "Shoot_7_sprite",

                _ => throw new ArgumentOutOfRangeException(nameof(state), state, null)
            };
        }
    }

    public class AnimationHandler : AnimationController
    {
        private DataBase _dataBase => Locator<DataBase>.Instance;
        private LeaderConfig _leaderConfig => _dataBase.GetConfig<LeaderConfig>();
        private LeaderSystem _leaderSystem => Locator<LeaderSystem>.Instance;

        private string _currentState;

        public ReactiveProperty<AnimationStateLeader> CurrentState { get; private set; }
            = new ReactiveProperty<AnimationStateLeader>(AnimationStateLeader.None);

        [SerializeField] private SpriteLibrary _spriteLibrary;

        private void Awake()
        {
            _leaderSystem.GunHandler.GunModelCurrent.Subscribe(value =>
            {
                var gunInfo = _leaderConfig.GetWeaponInfo(value.GunId) as LeaderWeaponInfo;
                _spriteLibrary.spriteLibraryAsset = gunInfo.SpriteLibraryAsset;

                if (value.GunId == LeaderKey.GunId_04 || value.GunId == LeaderKey.GunId_05)
                {
                    _currentState = AnimationStateLeader.Shoot_4_sprite.ConvertToString();
                }
                else _currentState = AnimationStateLeader.Shoot_7_sprite.ConvertToString();

            }).AddTo(this);

            _leaderSystem.GunHandler.TimeReloadCurrent.Subscribe(value =>
            {
                animator.SetFloat(LeaderKey.ANIMATIONKEY_RELOADING, value);
            }).AddTo(this);

            _leaderSystem.GunHandler.IsShooting += AnimationShoot;
        }

        private void AnimationShoot()
        {
            animator.SetTrigger(_currentState);
        }

        private void OnDestroy()
        {
            _leaderSystem.GunHandler.IsShooting -= AnimationShoot;
        }
    }
}