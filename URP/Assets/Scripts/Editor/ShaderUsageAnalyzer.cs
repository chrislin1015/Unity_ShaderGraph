using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class ShaderUsageAnalyzer : EditorWindow
{
    Dictionary<Shader, List<Material>> shaderUsage = new Dictionary<Shader, List<Material>>();
    Dictionary<Shader, bool> shaderFoldOut = new Dictionary<Shader, bool>();
    Vector2 scrollPosition = Vector2.zero;
    
    [MenuItem("LINK/Shader Usage Analyzer")]
    public static void ShowWindow()
    {
        ShaderUsageAnalyzer window = GetWindow<ShaderUsageAnalyzer>("Shader Usage Analyzer");
        window.Show();
    }

    void OnGUI()
    {
        if (GUILayout.Button("Analyze Shader Usage"))
        {
            AnalyzeShaderUsage();
        }

        scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);
        foreach (KeyValuePair<Shader, List<Material>> kvp in shaderUsage)
        {
            shaderFoldOut[kvp.Key] = EditorGUILayout.Foldout(shaderFoldOut[kvp.Key], kvp.Key.name);
            if (shaderFoldOut[kvp.Key])
            {
                foreach (Material material in kvp.Value)
                {
                    EditorGUILayout.ObjectField(material, typeof(Material));
                }
            }
        }
        EditorGUILayout.EndScrollView();
    }

    void AnalyzeShaderUsage()
    {
        shaderUsage.Clear();
        shaderFoldOut.Clear();
        
        string[] shaderGuids = AssetDatabase.FindAssets("t:shader");
        string[] shaderGraphGuids = AssetDatabase.FindAssets("t:shadergraph");
        string[] allShaderGuids = shaderGuids.Concat(shaderGraphGuids).ToArray();

        foreach (string guid in allShaderGuids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            Shader shader = AssetDatabase.LoadAssetAtPath<Shader>(path);
            if (shader == null) continue;
            shaderUsage.Add(shader, new List<Material>());
            shaderFoldOut.Add(shader, false);
        }
        
        string[] materialGuids = AssetDatabase.FindAssets("t:material");
        foreach (string guid in materialGuids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            Material material = AssetDatabase.LoadAssetAtPath<Material>(path);
            if (material == null) continue;
            if (!shaderUsage.ContainsKey(material.shader)) continue;
            if (material.hideFlags == HideFlags.NotEditable) continue;
            shaderUsage[material.shader].Add(material);
        }
        
        // foreach (string guid in shaderGuids)
        // {
        //     string path = AssetDatabase.GUIDToAssetPath(guid);
        //     Shader shader = AssetDatabase.LoadAssetAtPath<Shader>(path);
        //     if (shader == null) continue;
        //
        //     Material[] materials = shader.GetDependency()<Material>();
        //     if (shaderUsage.ContainsKey(shader))
        //     {
        //         foreach (Material material in materials)
        //         {
        //             shaderUsage[shader].Add(material);
        //         }
        //     }
        //     else
        //     {
        //         List<Material> materialList = new();
        //         foreach (Material material in materials)
        //         {
        //             materialList.Add(material);
        //         }
        //         shaderUsage.Add(shader, materialList);
        //     }
        // }
    }
}
