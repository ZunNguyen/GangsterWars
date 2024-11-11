using Sources.DataBaseSystem;
using Sources.GameData;
using Sources.GamePlaySystem.JourneyMap;
using Sources.SpawnerSystem;
using Sources.Utils.Singleton;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Game.Screens.JourneyScreen
{
    public class JourneyMapController : MonoBehaviour
    {
        private readonly Vector2 _anchorDefault = new Vector2(0, 0.5f);
        private readonly Vector2 _offsetBorder = new Vector2(500, 200);
        private readonly Vector2 _linkBarItemHorizontal = new Vector2(500, 30);
        private readonly Vector2 _linkBarItemVertical = new Vector2(30, 300);

        private const float _offsetItemX = 300;
        private const float _offsetItemY = 300;

        private SpawnerManager _spawnerManager => Locator<SpawnerManager>.Instance;

        private DataBase _dataBase => Locator<DataBase>.Instance;
        private JourneyMapConfig _journeyMapConfig => _dataBase.GetConfig<JourneyMapConfig>();
        private JourneyMapSystem _journeyMapSystem => Locator<JourneyMapSystem>.Instance;

        private JourneyMapData _journeyMapDataCurrent;
        private Dictionary<string, GameObject> _journeyPrefabs = new();

        [Header("Journey Item View")]
        [SerializeField] private Transform _journeyItemViewHolder;

        [Header("Link Item")]
        [SerializeField] private LinkBarItem _linkBarItemPrefab;
        [SerializeField] private Transform _linkBarItemHolder;

        private void Awake()
        {
            _journeyMapDataCurrent = _journeyMapSystem.JourneyMapDataCurrent;
            SetUpJourneyMap();
        }

        private void SetUpJourneyMap()
        {
            for (int row = 0; row < _journeyMapDataCurrent.Rows; row++)
            {
                for (int col = 0; col < _journeyMapDataCurrent.Collumns; col++)
                {
                    var cellIndex = row * _journeyMapDataCurrent.Collumns + col;
                    var dataState = _journeyMapSystem.GetDataState(_journeyMapDataCurrent.Data_1[cellIndex]);
                    if (dataState == DataState.Empty) break;
                    if (dataState == DataState.JourneyItem) SetJourneyItem(row, col);
                    if (dataState == DataState.HorizontalItem) SetHorizontalItem(row, col, true);
                    if (dataState == DataState.VerticalItem) SetHorizontalItem(row, col, false);
                }
            }
        }

        private void SetJourneyItem(int row, int col)
        {
            var cellIndex = row * _journeyMapDataCurrent.Collumns + col;
            var journeyItemId = _journeyMapDataCurrent.Data_1[cellIndex];
            if (!_journeyPrefabs.ContainsKey(journeyItemId))
            {
                var journeyInfo = _journeyMapConfig.GetJourneyItemInfo(journeyItemId);

                var path = _journeyMapConfig.PathHolderJourneyItemPrefab + "/" + journeyInfo.JourneyItem.name + ".prefab";
                var journeyItemPrefabSample = AssetDatabase.LoadAssetAtPath<GameObject>(path);
                _journeyPrefabs.Add(journeyItemId, journeyItemPrefabSample);
            }

            var journeyItemPrefab = _journeyPrefabs[journeyItemId];
            var newJourneyItemView = _spawnerManager.Get(journeyItemPrefab);
            newJourneyItemView.transform.SetParent(_journeyItemViewHolder);

            var journeyItemScript = newJourneyItemView.GetComponent<JourneyView>();
            var waveId = _journeyMapDataCurrent.Data_2[cellIndex];
            journeyItemScript.OnSetUp(waveId);

            var rect = newJourneyItemView.GetComponent<RectTransform>();
            rect.anchorMin = _anchorDefault;
            rect.anchorMax = _anchorDefault;
            rect.anchoredPosition = Vector2.zero;

            var targetPos = new Vector2();
            targetPos.x = _offsetBorder.x + col * _offsetItemX;
            targetPos.y = _offsetBorder.y + row * _offsetItemY;

            rect.anchoredPosition += targetPos;
        }

        private void SetHorizontalItem(int row, int col, bool isHorontal)
        {
            var newLinkBarItem = _spawnerManager.Get(_linkBarItemPrefab);
            newLinkBarItem.transform.SetParent(_linkBarItemHolder);

            var rectTransform = newLinkBarItem.GetComponent<RectTransform>();
            rectTransform.sizeDelta = isHorontal ? _linkBarItemHorizontal : _linkBarItemVertical;

            rectTransform.anchorMin = _anchorDefault;
            rectTransform.anchorMax = _anchorDefault;
            rectTransform.anchoredPosition = Vector2.zero;

            var targetPos = new Vector2();
            targetPos.x = _offsetBorder.x + col * _offsetItemX;
            targetPos.y = _offsetBorder.y + row * _offsetItemY;

            rectTransform.anchoredPosition += targetPos;
        }
    }
}