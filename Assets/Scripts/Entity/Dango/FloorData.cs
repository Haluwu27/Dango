using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static FloorManager;

public class FloorData : MonoBehaviour
{
    private void Awake()
    {
        CreateInvertedMeshCollider();
    }

    public void CreateInvertedMeshCollider()
    {
        RemoveExistingColliders();
        InvertMesh();

        gameObject.AddComponent<MeshCollider>();
    }

    private void RemoveExistingColliders()
    {
        Collider[] colliders = GetComponents<Collider>();
        for (int i = 0; i < colliders.Length; i++)
            DestroyImmediate(colliders[i]);
    }

    private void InvertMesh()
    {
        Mesh mesh = GetComponent<MeshFilter>().mesh;
        mesh.triangles = mesh.triangles.Reverse().ToArray();
    }


    [SerializeField] FloorManager floorManager;
    [SerializeField] Floor floor;

    //フロアに現存している団子の数
    int _dangoCount;

    private void OnTriggerEnter(Collider other)
    {
        //団子以外を弾く
        if (other.GetComponentInParent<DangoData>() == null) return;

        _dangoCount++;
        floorManager.CheckDangoIsFull(other, floor);
    }

    private void OnTriggerExit(Collider other)
    {
        //団子以外を弾く
        if (other.GetComponent<DangoData>() == null) return;

        _dangoCount--;
        floorManager.CheckDangoIsNotFull(other, floor, 1);
    }

    public void AddDangoCount() => _dangoCount++;
    public void RemoveDangoCount() => _dangoCount--;
    public int DangoCount => _dangoCount;
}

[Serializable]
public class FloorArray
{
    [SerializeField, Tooltip("エリアの定義")] FloorData[] floorDatas;
    [SerializeField, Tooltip("エリアに存在する団子射出装置")] DangoInjection[] dangoInjections;
    [SerializeField, Tooltip("エリアに存在できる最大の団子の数"), Min(0)] int maxDangoCount;

    public FloorData[] FloorDatas => floorDatas;
    public DangoInjection[] DangoInjections => dangoInjections;
    public int MaxDangoCount => maxDangoCount;
}