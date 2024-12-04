using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Sources.Utils
{
    [RequireComponent(typeof(Button))]
    public class ButtonEffect : MonoBehaviour, IPointerClickHandler
    {
        private readonly Vector3 _targetScale = new Vector3(0.9f, 0.9f, 0.9f);
        private const float _duration = 0.15f;

        public void OnPointerClick(PointerEventData eventData)
        {
            Effect();
        }

        private void Effect()
        {
            transform.DOScale(_targetScale, _duration).OnComplete(() =>
            {
                transform.DOScale(Vector3.one, _duration);
            });    
        }
    }
}