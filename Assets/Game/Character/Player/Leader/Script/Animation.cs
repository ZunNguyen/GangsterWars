using Game.Character.Controller;
using Sources.DataBaseSystem;
using Sources.DataBaseSystem.Leader;
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

    public class Animation : AnimationController
    {
        private DataBase _dataBase => Locator<DataBase>.Instance;
        private LeaderConfig _leaderConfig => _dataBase.GetConfig<LeaderConfig>();
        private LeaderSystem _leaderSystem => Locator<LeaderSystem>.Instance;

        public ReactiveProperty<AnimationStateLeader> CurrentState { get; private set; }
            = new ReactiveProperty<AnimationStateLeader>(AnimationStateLeader.None);

        [SerializeField] private SpriteLibrary _spriteLibrary;

        private void Awake()
        {
            _leaderSystem.GunHandler.GunModelCurrent.Subscribe(value =>
            {
                var gunInfo = _leaderConfig.GetWeaponInfo(value.GunId);
                _spriteLibrary.spriteLibraryAsset = gunInfo.SpriteLibraryAsset;
            }).AddTo(this);
        }

        public void AnimationShoot()
        {
            var state = AnimationStateLeader.Shoot_7_sprite.ConvertToString();
            animator.SetTrigger(state);
        }
    }
}