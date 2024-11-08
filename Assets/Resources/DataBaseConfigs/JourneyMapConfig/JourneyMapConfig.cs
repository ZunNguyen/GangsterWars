using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Sources.DataBaseSystem
{
    [Serializable]
    public class JourneyItemView
    {
        [PreviewField(Height = 100)]
        public GameObject JourneyItem;
    }

    public class Grid
    {
        [SerializeField] private int rows;
        [SerializeField] private int columns;
        [SerializeField] private List<List<string>> matrix = new List<List<string>>();

        public List<List<string>> Matrix => matrix;

        public void InitializeMatrix()
        {
            matrix.Clear();
            for (int i = 0; i < rows; i++)
            {
                List<string> row = new List<string>();
                for (int j = 0; j < columns; j++)
                {
                    row.Add(string.Empty);
                }
                matrix.Add(row);
            }
        }

        public int Rows => rows;
        public int Columns => columns;

        public void SetRows(int value) => rows = value;
        public void SetColumns(int value) => columns = value;
    }

    public class JourneyMapConfig : DataBaseConfig
    {
        public List<JourneyItemView> JourneyItemViews = new();
        public List<Grid> grids = new List<Grid>();
    }

    [CustomEditor(typeof(JourneyMapConfig))]
    public class CustomScriptInscpector : Editor
    {
        public override void OnInspectorGUI()
        {
            JourneyMapConfig config = (JourneyMapConfig)target;

            //EditorGUILayout.LabelField("Journey Item Views", EditorStyles.boldLabel);
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
                EditorGUILayout.LabelField($"Grid {i + 1}", EditorStyles.boldLabel);

                Grid grid = config.grids[i];

                // Hiển thị số hàng và số cột cho từng `Grid`
                int newRows = EditorGUILayout.IntField("Rows", grid.Rows);
                int newColumns = EditorGUILayout.IntField("Columns", grid.Columns);

                if (newRows != grid.Rows)
                {
                    grid.SetRows(newRows);
                }
                if (newColumns != grid.Columns)
                {
                    grid.SetColumns(newColumns);
                }

                // Nút để khởi tạo lại ma trận của từng `Grid`
                if (GUILayout.Button("Initialize Matrix for Grid " + (i + 1)))
                {
                    grid.InitializeMatrix();
                    Undo.RecordObject(config, "Initialized Matrix");
                }

                // Hiển thị số cột ở đầu ma trận của mỗi `Grid`
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("", GUILayout.Width(60));
                for (int col = 0; col < grid.Columns; col++)
                {
                    EditorGUILayout.LabelField($"Col {col + 1}", GUILayout.Width(60));
                }
                EditorGUILayout.EndHorizontal();

                // Hiển thị ma trận của `Grid`
                for (int row = 0; row < grid.Matrix.Count; row++)
                {
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField($"Row {row + 1}", GUILayout.Width(60));

                    for (int col = 0; col < grid.Matrix[row].Count; col++)
                    {
                        grid.Matrix[row][col] = EditorGUILayout.TextField(grid.Matrix[row][col], GUILayout.Width(60));
                    }
                    EditorGUILayout.EndHorizontal();
                }

                // Nút xóa `Grid`
                if (GUILayout.Button("Remove Grid " + (i + 1)))
                {
                    config.grids.RemoveAt(i);
                    Undo.RecordObject(config, "Removed Grid");
                    break; // Để tránh lỗi khi thay đổi danh sách trong vòng lặp
                }

                EditorGUILayout.Space();
            }

            // Cập nhật lại thay đổi trên `Inspector`
            if (GUI.changed)
            {
                EditorUtility.SetDirty(target);
            }
        }
    }
}