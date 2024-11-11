using Sources.DataBaseSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Sources.DataBaseSystem
{
    [Serializable]
    public class Cell
    {
        public string Value1;
        public string Value2;
    }

    public class GridMap
    {
        [SerializeField] private bool isLocked;
        [SerializeField] private List<List<Cell>> matrix = new List<List<Cell>>();

        public List<List<Cell>> Matrix => matrix;

        public void InitializeMatrix(int rows, int columns)
        {
            matrix.Clear();
            for (int i = 0; i < rows; i++)
            {
                List<Cell> row = new List<Cell>();
                for (int j = 0; j < columns; j++)
                {
                    row.Add(new Cell());
                }
                matrix.Add(row);
            }
        }
    }

    public class GridMapConfig : DataBaseConfig
    {
        public List<GridMap> GridMaps = new List<GridMap>();
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(GridMapConfig))]
    public class CustomScriptInscpector : Editor
    {
        private int _rows = 0;
        private int _columns = 0;
        private int _journeyMapIndex;

        public override void OnInspectorGUI()
        {
            GridMapConfig config = (GridMapConfig)target;

            if (GUILayout.Button("Add New Grid"))
            {
                config.GridMaps.Add(new GridMap());
                Undo.RecordObject(config, "Added New Grid");
            }

            for (int i = 0; i < config.GridMaps.Count; i++)
            {
                EditorGUILayout.Space();
                GridMap grid = config.GridMaps[i];

                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField($"Grid {i + 1}", EditorStyles.boldLabel);
                EditorGUILayout.EndHorizontal();

                _rows = EditorGUILayout.IntField("Rows", _rows);
                _columns = EditorGUILayout.IntField("Columns", _columns);
                _journeyMapIndex = EditorGUILayout.IntField("Journey Map Index", _journeyMapIndex);

                if (GUILayout.Button("Initialize Matrix for Grid " + (i + 1)))
                {
                    grid.InitializeMatrix(_rows, _columns);
                    Undo.RecordObject(config, "Initialized Matrix");
                }
                EditorGUILayout.Space();

                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("", GUILayout.Width(100));
                for (int col = 0; col < _columns; col++)
                {
                    EditorGUILayout.LabelField($"Col {col + 1}", GUILayout.Width(120));
                }
                EditorGUILayout.EndHorizontal();

                for (int row = 0; row < grid.Matrix.Count; row++)
                {
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.BeginVertical(GUILayout.Width(60));
                    EditorGUILayout.LabelField("Journey Id", GUILayout.Width(80));
                    EditorGUILayout.LabelField("Wave Id", GUILayout.Width(80));
                    EditorGUILayout.EndVertical();

                    for (int col = 0; col < grid.Matrix[row].Count; col++)
                    {
                        var cell = grid.Matrix[row][col];
                        EditorGUILayout.BeginVertical(GUILayout.Width(120));
                        cell.Value1 = EditorGUILayout.TextField(cell.Value1, GUILayout.Width(80));
                        cell.Value2 = EditorGUILayout.TextField(cell.Value2, GUILayout.Width(80));
                        EditorGUILayout.EndVertical();
                    }
                    EditorGUILayout.EndHorizontal();
                    EditorGUILayout.Space();
                }

                EditorGUILayout.Space();
                if (GUILayout.Button("Remove Grid " + (i + 1)))
                {
                    config.GridMaps.RemoveAt(i);
                    Undo.RecordObject(config, "Removed Grid");
                    break;
                }
                EditorGUILayout.Space();

                if (GUILayout.Button("Save Data"))
                {
                    var gridMapConfig = AssetDatabase.LoadAssetAtPath<JourneyMapConfig>("Assets/Resources/DataBaseConfigs/JourneyMapConfig/JourneyMapConfig.asset");

                    var journeyMapData = new JourneyMapData();
                    try
                    {
                        journeyMapData = gridMapConfig.JourneyMapDatas[_journeyMapIndex];
                    }
                    catch
                    {
                        var newJourneyMapData = new JourneyMapData();
                        gridMapConfig.JourneyMapDatas.Add(newJourneyMapData);
                        journeyMapData = gridMapConfig.JourneyMapDatas[_journeyMapIndex];
                    }

                    journeyMapData.Data_1.Clear();
                    journeyMapData.Data_2.Clear();
                    journeyMapData.Collumns = _columns;
                    journeyMapData.Rows = _rows;

                    for (int row = 0; row < _rows; row++)
                    {
                        for (int col = 0; col < _columns; col++)
                        {
                            var cell = grid.Matrix[row][col];
                            journeyMapData.Data_1.Add(cell.Value1);
                            journeyMapData.Data_2.Add(cell.Value2);
                        }
                    }

                    EditorUtility.SetDirty(gridMapConfig);
                    AssetDatabase.SaveAssets();

                    UnityEngine.Debug.Log("Save done");
                }

                if (GUILayout.Button("Load Data"))
                {
                    var gridMapConfig = AssetDatabase.LoadAssetAtPath<JourneyMapConfig>("Assets/Resources/DataBaseConfigs/JourneyMapConfig/JourneyMapConfig.asset");

                    var journeyMapData = gridMapConfig.JourneyMapDatas[_journeyMapIndex];
                    var rowCount = journeyMapData.Rows;
                    var colCount = journeyMapData.Collumns;
                    grid.InitializeMatrix(rowCount, colCount);

                    //2
                    for (int row = 0; row < rowCount; row++)
                    {
                        //3
                        for (int col = 0; col < colCount; col++)
                        {
                            var cell = grid.Matrix[row][col];
                            cell.Value1 = journeyMapData.Data_1[row * colCount + col];
                            cell.Value2 = journeyMapData.Data_2[row * colCount + col];
                        }
                    }

                    AssetDatabase.SaveAssets();
                    UnityEngine.Debug.Log("Load done");
                }
            }

            if (GUI.changed)
            {
                EditorUtility.SetDirty(target);
            }
        }
    }
#endif
}