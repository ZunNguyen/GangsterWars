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
        [SerializeField] private int rows;
        [SerializeField] private int columns;
        [SerializeField] private bool isLocked;
        [SerializeField] private List<List<Cell>> matrix = new List<List<Cell>>();

        public List<List<Cell>> Matrix => matrix;

        public void InitializeMatrix()
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

        public int Rows => rows;
        public int Columns => columns;

        public void SetRows(int value) => rows = value;
        public void SetColumns(int value) => columns = value;
    }

    public class GridMapConfig : DataBaseConfig
    {
        public List<GridMap> GridMaps = new List<GridMap>();
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(GridMapConfig))]
    public class CustomScriptInscpector : Editor
    {
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

                int newRows = EditorGUILayout.IntField("Rows", grid.Rows);
                int newColumns = EditorGUILayout.IntField("Columns", grid.Columns);

                if (newRows != grid.Rows) grid.SetRows(newRows);
                if (newColumns != grid.Columns) grid.SetColumns(newColumns);

                if (GUILayout.Button("Initialize Matrix for Grid " + (i + 1)))
                {
                    grid.InitializeMatrix();
                    Undo.RecordObject(config, "Initialized Matrix");
                }
                EditorGUILayout.Space();

                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("", GUILayout.Width(100));
                for (int col = 0; col < grid.Columns; col++)
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
                    //gridMapConfig.DataValue_1.Clear();
                    //gridMapConfig.DataValue_2.Clear();

                    //for (int row = 0; row < grid.Matrix.Count; row++)
                    //{
                    //    for (int col = 0; col < grid.Matrix[row].Count; col++)
                    //    {
                    //        var cell = grid.Matrix[row][col];
                    //        gridMapConfig.DataValue_1.Add(cell.Value1);
                    //        gridMapConfig.DataValue_2.Add(cell.Value2);
                    //    }
                    //}

                    AssetDatabase.SaveAssets();
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