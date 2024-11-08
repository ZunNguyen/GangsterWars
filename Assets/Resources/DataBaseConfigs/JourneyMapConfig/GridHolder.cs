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
        private string[,] matrix = new string[3, 3]; // Ma trận 3x3

        public string[,] Matrix => matrix; // Getter cho ma trận
    }

    [CustomEditor(typeof(GridHolder))]
    public class CustomScriptInscpector : Editor
    {
        public override void OnInspectorGUI()
        {
            GridHolder matrixData = (GridHolder)target;

            // Duyệt qua ma trận và cho phép nhập liệu vào các ô
            for (int i = 0; i < matrixData.Matrix.GetLength(0); i++)
            {
                EditorGUILayout.BeginHorizontal();
                for (int j = 0; j < matrixData.Matrix.GetLength(1); j++)
                {
                    // Hiển thị mỗi ô trong ma trận dưới dạng TextField
                    matrixData.Matrix[i, j] = EditorGUILayout.TextField(matrixData.Matrix[i, j]);
                }
                EditorGUILayout.EndHorizontal();
            }

            // Đảm bảo thay đổi trong ma trận được cập nhật trong Editor
            if (GUI.changed)
            {
                EditorUtility.SetDirty(target);
            }
        }
    }
}