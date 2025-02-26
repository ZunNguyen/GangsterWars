using Cysharp.Threading.Tasks;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sources.UISystem
{
    public class UITransitionHandler : MonoBehaviour
    {
        [SerializeField] private bool _useBlackBackGround;
        [SerializeField] private UITransition _uiTransition;
        [SerializeField] private CanvasGroup _rootCanvasGroup;
        [SerializeField] private GameObject _blackBackGround;
        [SerializeField] private float _duration = 0.5f;

        private void Awake()
        {
            _blackBackGround.gameObject.SetActive(_useBlackBackGround);
        }

        public async UniTask DoTransition(bool isTransitionEnter)
        {
            var transition = isTransitionEnter ? _uiTransition.TransitionEnter : _uiTransition.TransitionExit;
            var ease = isTransitionEnter ? _uiTransition.EaseEnter : _uiTransition.EaseExit;

            switch (transition)
            {
                case UITransition.Transition.None:
                    SetCanvasGroup(isTransitionEnter ? 1 : 0, true);
                    await UniTask.Delay((int)(_duration * 1000));
                    break;

                case UITransition.Transition.FadeIn:
                    SetCanvasGroup(0, true);
                    await _rootCanvasGroup.DOFade(1, _duration).SetEase(ease);
                    break;

                case UITransition.Transition.FadeOut:
                    SetCanvasGroup(1, true);
                    await _rootCanvasGroup.DOFade(0, _duration).SetEase(ease);
                    break;
            }
        }

        private async UniTask SetCanvasGroup(float alpha, bool blockRaycast, float duration = 0f)
        {
            _rootCanvasGroup.blocksRaycasts = blockRaycast;
            await _rootCanvasGroup.DOFade(alpha, duration);
        }
    }
}