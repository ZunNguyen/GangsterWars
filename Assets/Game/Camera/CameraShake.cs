using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.ShakeCamera
{
    public class CameraShake : MonoBehaviour
    {
        private readonly Vector3 _posCameraDefault = new Vector3(0,0,-10);

        public void Shake(float duration, float strength, int vibrato = 10, float randomness = 90f)
        {
            transform.DOShakePosition(duration, strength, vibrato, randomness, false, true)
                    .OnComplete(() =>
                    {
                        transform.localPosition = _posCameraDefault;
                    });
        }
    }
}