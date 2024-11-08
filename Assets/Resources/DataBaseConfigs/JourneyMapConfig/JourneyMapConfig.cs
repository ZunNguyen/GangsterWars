using System;
using UnityEditor;
using UnityEngine;

namespace Sources.DataBaseSystem
{
    public enum CellState
    {
        Empty,
        Discovered,
        Obstacle
    }

    public class JourneyMapConfig : DataBaseConfig
    {
        public int X = 5;
        public int Y = 5;

        [Serializable]
        public class Column
        {
            public CellState[] rows;
        }

        public Column[] columns;

        public void ResizeGrid()
        {
            // Resize columns based on X
            if (columns == null || columns.Length != X)
            {
                Array.Resize(ref columns, X);
            }

            // Resize each column's rows based on Y
            for (int x = 0; x < X; x++)
            {
                if (columns[x] == null)
                    columns[x] = new Column();

                if (columns[x].rows == null || columns[x].rows.Length != Y)
                    columns[x].rows = new CellState[Y];
            }
        }
    }

    [CustomEditor(typeof(JourneyMapConfig))]
    public class CustomScriptInscpector : Editor
    {
        private JourneyMapConfig targetScript;

        void OnEnable()
        {
            targetScript = (JourneyMapConfig)target;
            targetScript.ResizeGrid();
        }

        public override void OnInspectorGUI()
        {
            // Thiết lập lại kích thước của ma trận nếu giá trị X hoặc Y thay đổi
            int newX = EditorGUILayout.IntField("X", targetScript.X);
            int newY = EditorGUILayout.IntField("Y", targetScript.Y);

            // Nếu X hoặc Y thay đổi, cập nhật lại cấu hình
            if (newX != targetScript.X || newY != targetScript.Y)
            {
                targetScript.X = newX;
                targetScript.Y = newY;

                // Khởi tạo lại mảng columns để phản ánh kích thước mới
                targetScript.columns = new JourneyMapConfig.Column[targetScript.X];
                for (int x = 0; x < targetScript.X; x++)
                {
                    targetScript.columns[x] = new JourneyMapConfig.Column();
                    targetScript.columns[x].rows = new CellState[targetScript.Y];
                }

                // Yêu cầu Unity cập nhật lại Inspector
                Repaint();
            }

            // Hiển thị ma trận
            EditorGUILayout.BeginHorizontal();
            for (int y = 0; y < targetScript.Y; y++)
            {
                EditorGUILayout.BeginVertical();
                for (int x = 0; x < targetScript.X; x++)
                {
                    // Toggle cho từng ô trong ma trận
                    targetScript.columns[x].rows[y] = (CellState)EditorGUILayout.EnumPopup(targetScript.columns[x].rows[y]);
                }
                EditorGUILayout.EndVertical();
            }
            EditorGUILayout.EndHorizontal();
        }
    }
}