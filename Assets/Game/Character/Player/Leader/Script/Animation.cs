using Game.Character.Controller;
using Sources.DataBaseSystem.Leader;
using System;
using UniRx;
using UnityEngine;

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
        public ReactiveProperty<AnimationStateLeader> CurrentState { get; }
            = new ReactiveProperty<AnimationStateLeader>(AnimationStateLeader.None);

        private void Awake()
        {
            CurrentState.Subscribe(value =>
            {
                var state = value.ConvertToString();
                animator.SetTrigger(state);
            }).AddTo(this);
        }
    }
}