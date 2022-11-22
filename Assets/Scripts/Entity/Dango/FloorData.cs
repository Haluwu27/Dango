using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static FloorManager;

public class FloorData : MonoBehaviour
{
    [SerializeField] FloorManager floorManager;
    [SerializeField] Floor floor;

    Mesh _mesh;

    private void Awake()
    {
        CreateInvertedMeshCollider();
    }

    private void OnTriggerEnter(Collider other)
    {
        PlayerTriggerEnter(other);
    }

    private void OnTriggerExit(Collider other)
    {
        PlayerTriggerExit(other);
    }

    private void PlayerTriggerEnter(Collider other)
    {
        if (!other.gameObject.CompareTag("Player")) return;

        QuestManager.Instance.SucceedChecker.CheckQuestDestinationSucceed(floor, true);
    }

    private void PlayerTriggerExit(Collider other)
    {
        if (other.GetComponentInParent<PlayerData>() == null) return;

        QuestManager.Instance.SucceedChecker.CheckQuestDestinationSucceed(floor, false);
    }

    private void CreateInvertedMeshCollider()
    {
        //RemoveExistingColliders();
        InvertMesh();

        GameObject obj = new();
        obj.transform.parent = transform;
        obj.AddComponent<MeshCollider>().sharedMesh = _mesh;
        obj.layer = 8;
        obj.transform.localPosition = Vector3.zero;
        obj.transform.localRotation = Quaternion.identity;
    }

    private void RemoveExistingColliders()
    {
        Collider[] colliders = GetComponents<Collider>();
        for (int i = 0; i < colliders.Length; i++)
            DestroyImmediate(colliders[i]);
    }

    private void InvertMesh()
    {
        _mesh = GetComponent<MeshFilter>().mesh;
        _mesh.triangles = _mesh.triangles.Reverse().ToArray();
    }

    public Floor GetFllor => floor;
}

[Serializable]
public class FloorArray
{
    [SerializeField, Tooltip("エリアの定義")] FloorData[] floorDatas;
    [SerializeField, Tooltip("エリアに存在する団子射出装置")] DangoInjection[] dangoInjections;
    [SerializeField, Tooltip("エリアに存在できる最大の団子の数"), Min(0)] int maxDangoCount;

    int dangoCount;
    Floor _floor;

    //重複しない乱数取得用
    List<int> _nums = new();

    public void AddDangoCount()
    {
        dangoCount++;

        if (dangoCount < maxDangoCount) return;

        foreach (var dango in dangoInjections)
        {
            dango.SetCanShot(false);
        }
    }

    public void RemoveDangoCount(int shotValue)
    {
        dangoCount--;
        if (_floor == Floor.floor1)
            Logger.Log(dangoCount);

        if (dangoCount >= maxDangoCount) return;

        //乱数用のインデックス番号を取得
        for (int i = 0; i < DangoInjections.Length; i++)
        {
            _nums.Add(i);
        }

        //ランダムな装置を選択する
        for (int i = 0; i < shotValue; i++)
        {
            //インデックス番号を取得
            int index = UnityEngine.Random.Range(0, _nums.Count);

            //重複しないランダムな発射装置の発射フラグを立てる
            DangoInjections[_nums[index]].SetCanShot(true);

            //今回取得した番号を選択肢から排除
            _nums.RemoveAt(index);
        }

        //次回用にクリアする
        _nums.Clear();
    }

    public void SetFloor(Floor floor) => _floor = floor;
    public Floor Floor => _floor;
    public FloorData[] FloorDatas => floorDatas;
    public DangoInjection[] DangoInjections => dangoInjections;
}