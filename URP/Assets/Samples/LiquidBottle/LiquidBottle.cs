using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

[ExecuteInEditMode]
public class LiquidBottle : MonoBehaviour
{
    [SerializeField] Mesh _mesh;
    [SerializeField] Material _material;
    
    [ReadOnly]
    [SerializeField] float _liquidTop;
    [ReadOnly]
    [SerializeField] float _liquidBottom;
    
    void Awake()
    {
        if (_mesh == null)
            _mesh = GetComponent<MeshFilter>().sharedMesh;
        if (_material == null)
            _material = GetComponent<Renderer>().sharedMaterial;
        if (_material != null && _mesh != null)
        {
            _liquidBottom = float.MaxValue;
            _liquidTop = float.MinValue;
            foreach (Vector3 vertex in _mesh.vertices)
            {
                if (vertex.y > _liquidTop)
                    _liquidTop = vertex.y;
                if (vertex.y < _liquidBottom)
                    _liquidBottom = vertex.y;
            }
            _material.SetFloat("_LiquidTop", _liquidTop);
            _material.SetFloat("_LiquidBottom", _liquidBottom);
        }
    }

    void OnEnable()
    {
        Awake();
    }
}
