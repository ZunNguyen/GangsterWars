using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sources.UISystem
{
    [Serializable]
    public class UITransition
    {
        public enum Transition
        {
            None,
            FadeIn,
            FadeOut,
            ZoomIn,
            ZoomOut,
        }

        public Transition TransitionEnter = Transition.FadeIn;
        public Transition TransitionExit = Transition.FadeOut;
        public Ease EaseEnter = Ease.OutCubic;
        public Ease EaseExit = Ease.OutCubic;
    }
}