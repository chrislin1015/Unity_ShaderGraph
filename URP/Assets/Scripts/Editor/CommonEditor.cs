using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace LINK.Editor
{
    public class CommonEditor : EditorWindow
    {
        public virtual void DrawBlockGUI(string label, SerializedProperty serializedProperty)
        {
            EditorGUILayout.BeginHorizontal("Box");
            EditorGUILayout.LabelField(label, GUILayout.Width(50));
            EditorGUILayout.PropertyField(serializedProperty, GUIContent.none);
            EditorGUILayout.EndHorizontal();
        }
    }
}