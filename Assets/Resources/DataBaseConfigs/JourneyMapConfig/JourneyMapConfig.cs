using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
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

    [Serializable]
    public class JourneyItemView
    {
        public string Id;
        public GameObject JourneyItem;
    }

    public class Grid
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
        public bool IsLocked { get => isLocked; set => isLocked = value; }
    }

    public class JourneyMapConfig : DataBaseConfig
    {
        public List<JourneyItemView> JourneyItemViews = new();
        public List<Grid> grids = new List<Grid>();
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(JourneyMapConfig))]
    public class CustomScriptInscpector : Editor
    {
        public override void OnInspectorGUI()
        {
            JourneyMapConfig config = (JourneyMapConfig)target;

            SerializedProperty journeyItemViewsProp = serializedObject.FindProperty("JourneyItemViews");
            EditorGUILayout.PropertyField(journeyItemViewsProp, new GUIContent("Journey Item Views"), true);
            serializedObject.ApplyModifiedProperties();
            EditorGUILayout.Space();

            if (GUILayout.Button("Add New Grid"))
            {
                config.grids.Add(new Grid());
                Undo.RecordObject(config, "Added New Grid");
            }

            for (int i = 0; i < config.grids.Count; i++)
            {
                EditorGUILayout.Space();
                Grid grid = config.grids[i];

                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField($"Grid {i + 1}", EditorStyles.boldLabel);
                grid.IsLocked = GUILayout.Toggle(grid.IsLocked, grid.IsLocked ? "Unlock" : "Lock", "Button");
                EditorGUILayout.EndHorizontal();

                int newRows = grid.IsLocked ? grid.Rows : EditorGUILayout.IntField("Rows", grid.Rows);
                int newColumns = grid.IsLocked ? grid.Columns : EditorGUILayout.IntField("Columns", grid.Columns);

                if (!grid.IsLocked)
                {
                    if (newRows != grid.Rows) grid.SetRows(newRows);
                    if (newColumns != grid.Columns) grid.SetColumns(newColumns);

                    if (GUILayout.Button("Initialize Matrix for Grid " + (i + 1)))
                    {
                        grid.InitializeMatrix();
                        Undo.RecordObject(config, "Initialized Matrix");
                    }
                }

                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("", GUILayout.Width(60));
                for (int col = 0; col < grid.Columns; col++)
                {
                    EditorGUILayout.LabelField($"Col {col + 1}", GUILayout.Width(120));
                }
                EditorGUILayout.EndHorizontal();

                for (int row = 0; row < grid.Matrix.Count; row++)
                {
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.BeginVertical(GUILayout.Width(60));
                    EditorGUILayout.LabelField("Journey Id", GUILayout.Width(60));
                    EditorGUILayout.LabelField("Wave Id", GUILayout.Width(60));
                    EditorGUILayout.EndVertical();

                    for (int col = 0; col < grid.Matrix[row].Count; col++)
                    {
                        var cell = grid.Matrix[row][col];
                        EditorGUILayout.BeginVertical(GUILayout.Width(120));
                        if (grid.IsLocked)
                        {
                            EditorGUILayout.LabelField(cell.Value1, GUILayout.Width(60));
                            EditorGUILayout.LabelField(cell.Value2, GUILayout.Width(60));
                        }
                        else
                        {
                            cell.Value1 = EditorGUILayout.TextField(cell.Value1, GUILayout.Width(60));
                            cell.Value2 = EditorGUILayout.TextField(cell.Value2, GUILayout.Width(60));
                        }
                        EditorGUILayout.EndVertical();
                    }
                    EditorGUILayout.EndHorizontal();
                    EditorGUILayout.Space();
                }

                if (!grid.IsLocked && GUILayout.Button("Remove Grid " + (i + 1)))
                {
                    config.grids.RemoveAt(i);
                    Undo.RecordObject(config, "Removed Grid");
                    break;
                }

                EditorGUILayout.Space();
            }

            if (GUI.changed)
            {
                EditorUtility.SetDirty(target);
            }
        }
    }
#endif
}