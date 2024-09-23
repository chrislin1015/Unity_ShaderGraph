using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace LINK.Editor
{
    public class DotProductEditor : EditorWindow
    {
        public Vector3 _p0;
        public Vector3 _p1;
        public Vector3 _pC;

        SerializedObject _obj;
        SerializedProperty _p0Property;
        SerializedProperty _p1Property;
        SerializedProperty _pCProperty;
        GUIStyle _guiStyle = new GUIStyle();
        
        [MenuItem("LINK/Dot Product Window")]
        public static void DotProductWindow()
        {
            DotProductEditor window = GetWindow<DotProductEditor>("Dot Product Editor");
            window.Show();
        }
        
        void OnEnable()
        {
            if (_p1 == Vector3.zero && _p0 == Vector3.zero)
            {
                _p0 = Vector3.up;
                _p1 = Vector3.right;
                _pC = Vector3.zero;
            }

            _obj = new SerializedObject(this);
            _p0Property = _obj.FindProperty("_p0");
            _p1Property = _obj.FindProperty("_p1");
            _pCProperty = _obj.FindProperty("_pC");

            _guiStyle.fontSize = 25;
            _guiStyle.fontStyle = FontStyle.Bold;
            _guiStyle.normal.textColor = Color.white;
            
            SceneView.duringSceneGui += DuringSceneGui;
        }
        
        void OnDisable()
        {
            SceneView.duringSceneGui -= DuringSceneGui;
        }

        void OnGUI()
        {
            _obj.Update();

            DrawBlockGUI("p0", _p0Property);
            DrawBlockGUI("p1", _p1Property);
            DrawBlockGUI("pC", _pCProperty);

            if (_obj.ApplyModifiedProperties())
            {
                SceneView.RepaintAll();
            }
        }

        void DuringSceneGui(SceneView sceneView)
        {
            Handles.color = Color.red;
            Vector3 p0 = SetMovePoint(_p0);
            Handles.color = Color.green;
            Vector3 p1 = SetMovePoint(_p1);
            Handles.color = Color.white;
            Vector3 pC = SetMovePoint(_pC);

            if (_p0 != p0 || _p1 != p1 || _pC != pC)
            {
                _p0 = p0;
                _p1 = p1;
                _pC = pC;
                
                Repaint();
            }
            
            DrawLabel(_p0, _p1, _pC);
        }

        Vector3 SetMovePoint(Vector3 pos)
        {
            float size = HandleUtility.GetHandleSize(Vector3.zero) * 0.15f;
            return Handles.FreeMoveHandle(pos, size, Vector3.zero, Handles.SphereHandleCap);
        }

        void DrawBlockGUI(string label, SerializedProperty serializedProperty)
        {
            EditorGUILayout.BeginHorizontal("Box");
            EditorGUILayout.LabelField(label, GUILayout.Width(50));
            EditorGUILayout.PropertyField(serializedProperty, GUIContent.none);
            EditorGUILayout.EndHorizontal();
        }

        float DotProduct(Vector3 p0, Vector3 p1, Vector3 c)
        {
            Vector3 a = (p0 - c).normalized;
            Vector3 b = (p1 - c).normalized;

            return (a.x * b.x) + (a.y * b.y) + (a.z * b.z);
        }

        void DrawLabel(Vector3 p0, Vector3 p1, Vector3 c)
        {
            Handles.Label(c, DotProduct(p0, p1, c).ToString("F1"), _guiStyle);
            Handles.color = Color.black;

            Vector3 cl = WorldRotation(p0, c, Vector3.up);
            Vector3 cr = WorldRotation(p0, c, Vector3.down);
            
            Handles.DrawAAPolyLine(3f, p0, c);
            Handles.DrawAAPolyLine(3f, p1, c);
            Handles.DrawAAPolyLine(3f, c, cl);
            Handles.DrawAAPolyLine(3f, c, cr);
        }

        Vector3 WorldRotation(Vector3 p, Vector3 c, Vector3 pos)
        {
            Vector3 dir = (p - c).normalized;
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            Quaternion rot = Quaternion.AngleAxis(angle, Vector3.forward);
            return c + rot * pos;
        }
    }
}