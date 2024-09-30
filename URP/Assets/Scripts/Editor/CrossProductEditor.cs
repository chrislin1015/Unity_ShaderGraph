using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace LINK.Editor
{
    public class CrossProductEditor : CommonEditor, IUpdateSceneGUI
    {
        public Vector3 pointOne;
        public Vector3 pointTwo;
        public Vector3 pointCross;
        SerializedObject _obj;
        SerializedProperty _pointOneProperty;
        SerializedProperty _pointTwoProperty;
        SerializedProperty _pointCrossProperty;
        GUIStyle _guiStyle = new();
        bool _isMatrixMethod = false;
        
        [MenuItem("LINK/Cross Product Window")]
        public static void CrossProductWindow()
        {
            CrossProductEditor window = GetWindow<CrossProductEditor>("Cross Product Editor");
            window.Show();
        }
        
        //implement IUpdateSceneGUI
        void IUpdateSceneGUI.SceneGUI(SceneView sceneView)
        {
            SceneGUI(sceneView);
        }

        void SceneGUI(SceneView sceneView)
        {
            Vector3 p = Handles.PositionHandle(pointOne, Quaternion.identity);
            Vector3 q = Handles.PositionHandle(pointTwo, Quaternion.identity);

            Handles.color = Color.blue;
            Vector3 pxq = _isMatrixMethod ? CrossProductMatrix(p, q) : CrossProduct(p, q);
            Handles.DrawSolidDisc(pxq, Vector3.forward, 0.05f);
            
            if (pointOne != p || pointTwo != q)
            {
                Undo.RecordObject(this, "Tool Move");
                
                pointOne = p;
                pointTwo = q;
                pointCross = pxq;
                
                RepaintOnGUI();
            }
            
            DrawLineGUI(p, "P", Color.red);
            DrawLineGUI(q, "Q", Color.green);
            DrawLineGUI(pxq, "PxQ", Color.blue);
        }

        void SetDefaultValues()
        {
            pointOne = Vector3.up;
            pointTwo = Vector3.right;
            pointCross = Vector3.Cross(pointOne, pointTwo);
        }

        void OnEnable()
        {
            if (pointOne == Vector3.zero && pointTwo == Vector3.zero)
            {
                SetDefaultValues();
            }

            _obj = new SerializedObject(this);
            _pointOneProperty = _obj.FindProperty("pointOne");
            _pointTwoProperty = _obj.FindProperty("pointTwo");
            _pointCrossProperty = _obj.FindProperty("pointCross");
            
            _guiStyle.fontSize = 25;
            _guiStyle.fontStyle = FontStyle.Bold;
            _guiStyle.normal.textColor = Color.white;

            SceneView.duringSceneGui += SceneGUI;
            Undo.undoRedoPerformed += RepaintOnGUI;
        }

        void OnDisable()
        {
            SceneView.duringSceneGui -= SceneGUI;
            Undo.undoRedoPerformed -= RepaintOnGUI;
        }

        void OnGUI()
        {
            _obj.Update();
            
            DrawBlockGUI("P", _pointOneProperty);
            DrawBlockGUI("Q", _pointTwoProperty);
            DrawBlockGUI("PxQ", _pointCrossProperty);

            if (_obj.ApplyModifiedProperties())
            {
                SceneView.RepaintAll();
            }

            if (GUILayout.Button(("Reset Value")))
            {
                SetDefaultValues();
            }

            _isMatrixMethod = GUILayout.Toggle(_isMatrixMethod, "Is Matrix Method");
        }

        void DrawLineGUI(Vector3 pos, string text, Color color)
        {
            Handles.color = color;
            Handles.Label(pos, text, _guiStyle);
            Handles.DrawAAPolyLine(3f, pos, Vector3.zero);
        }

        void RepaintOnGUI()
        {
            Repaint();
        }

        Vector3 CrossProduct(Vector3 p, Vector3 q)
        {
            float x = p.y * q.z - p.z * q.y;
            float y = p.z * q.x - p.x * q.z;
            float z = p.x * q.y - p.y * q.x;
            return new Vector3(x, y, z);
        }

        Vector3 CrossProductMatrix(Vector3 p, Vector3 q)
        {
            Matrix4x4 m = new Matrix4x4();

            // 0   -Qz   Qy
            // Qz   0   -Qx
            //-Qy   Qx   0
            m.SetColumn(0, new Vector4(0, -q.z, q.y, 0));
            m.SetColumn(1, new Vector4(q.z, 0, -q.x, 0));
            m.SetColumn(2, new Vector4(-q.y, q.x, 0, 0));
            
            // m[0, 0] = 0;
            // m[0, 1] = q.z;
            // m[0, 2] = -q.y;
            //
            // m[1, 0] = -q.z;
            // m[1, 1] = 0;
            // m[1, 2] = q.x;
            //
            // m[2, 0] = q.y;
            // m[2, 1] = -q.x;
            // m[2, 2] = 0;

            return m * p;
        }
    }
}