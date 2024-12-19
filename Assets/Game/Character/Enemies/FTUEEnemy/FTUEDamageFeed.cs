using Cysharp.Threading.Tasks;
using DG.Tweening;
using Sources.SpawnerSystem;
using Sources.Utils.Singleton;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

namespace Game.Character.Enemy.FTUE
{
    public class FTUEDamageFeed : MonoBehaviour
    {
        private const float _offsetMovePosY = 1.5f;
        private const float _duration = 1f;

        private SpawnerManager _spawnerManager => Locator<SpawnerManager>.Instance;

        [SerializeField] private RectTransform _rect;
        [SerializeField] private TMP_Text _text;

        public async void ShowDamageFeed(int value)
        {
            _text.text = value.ToString();
            gameObject.SetActive(true);

            var targetMovePosY = _rect.anchoredPosition.y + _offsetMovePosY;
            _rect.DOAnchorPosY(targetMovePosY, _duration).OnComplete(async () =>
            {
                await UniTask.Delay(200);
                _spawnerManager.Release(this);
            });
        }
    }
}