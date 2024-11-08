using BestHTTP.Examples;
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static UnityEditor.Progress;

namespace Sources.DataBaseSystem
{
    public class GridHolder : DataBaseConfig
    {
        [SerializeField]
        private int rows;
        [SerializeField]
        private int columns;

        [SerializeField]
        private List<List<string>> matrix = new List<List<string>>(); // Ma trận dưới dạng List

        // Getter để truy cập ma trận
        public List<List<string>> Matrix => matrix;

        // Hàm khởi tạo lại ma trận theo số hàng và số cột
        public void InitializeMatrix()
        {
            matrix.Clear();
            for (int i = 0; i < rows; i++)
            {
                List<string> row = new List<string>();
                for (int j = 0; j < columns; j++)
                {
                    row.Add(string.Empty); // Khởi tạo với chuỗi rỗng
                }
                matrix.Add(row);
            }
        }

        public int Rows => rows;
        public int Columns => columns;

        public void SetRows(int value) => rows = value;
        public void SetColumns(int value) => columns = value;
    }

    [CustomEditor(typeof(GridHolder))]
    public class CustomScriptInscpector : Editor
    {
        public override void OnInspectorGUI()
        {
            GridHolder matrixData = (GridHolder)target;

            // Cho phép thay đổi số hàng và số cột trong Inspector
            int newRows = EditorGUILayout.IntField("Rows", matrixData.Rows);
            int newColumns = EditorGUILayout.IntField("Columns", matrixData.Columns);

            if (newRows != matrixData.Rows)
            {
                matrixData.SetRows(newRows);
            }

            if (newColumns != matrixData.Columns)
            {
                matrixData.SetColumns(newColumns);
            }

            if (GUILayout.Button("Initialize Matrix"))
            {
                matrixData.InitializeMatrix(); // Khởi tạo lại ma trận khi nhấn nút
                Undo.RecordObject(matrixData, "Initialized Matrix");
            }

            for (int i = 0; i < matrixData.Matrix.Count; i++)
            {
                EditorGUILayout.BeginHorizontal();
                for (int j = 0; j < matrixData.Matrix[i].Count; j++)
                {
                    matrixData.Matrix[i][j] = EditorGUILayout.TextField(matrixData.Matrix[i][j]);
                }
                EditorGUILayout.EndHorizontal();
            }

            // Đảm bảo sự thay đổi trong ma trận được cập nhật trong Editor
            if (GUI.changed)
            {
                EditorUtility.SetDirty(target);
            }
        }
    }
}