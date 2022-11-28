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
        obj.layer = LayerMask.NameToLayer("MapCollider");
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
}

[Serializable]
public class FloorArray
{
    [Flags]
    enum E_D5
    {
        None = 0,

        a3D5 = 1 << 1,
        a4D5 = 1 << 2,
        a5D5 = 1 << 3,
        a6D5 = 1 << 4,
        a7D5 = 1 << 5,

        [InspectorName("")]
        Max
    }

    [SerializeField, Tooltip("エリアの定義")] FloorData[] floorDatas;
    [SerializeField, Tooltip("エリアに存在する団子射出装置")] DangoInjection[] dangoInjections;
    [SerializeField, Tooltip("エリアに存在できる最大の団子の数"), Min(0)] int maxDangoCount;
    [SerializeField, Tooltip("救済供給可のうD5")] E_D5 salvageableD5;
    [SerializeField, Tooltip("はじめて侵入可のうになるD5")] E_D5 intrudableD5;

    int[] dangoCounts = new int[(int)DangoColor.Other - 1];
    int dangoCount;

    DangoData _salvationDango;

    //重複しない乱数取得用
    List<int> _nums = new();

    public void AddDangoCount(DangoColor color)
    {
        //if (HasFlagIntrudableD5(1))
        //    Logger.Log(floorDatas[0].name +":"+ color);

        dangoCount++;
        dangoCounts[(int)color - 1]++;

        if (dangoCount < maxDangoCount) return;

        foreach (var dango in dangoInjections)
        {
            dango.SetCanShot(false);
        }
    }

    public void RemoveDangoCount(int shotValue, DangoColor color)
    {
        dangoCount--;
        dangoCounts[(int)color - 1]--;

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
            if (dangoCount < maxDangoCount) DangoInjections[_nums[index]].SetCanShot(true);

            //今回取得した番号を選択肢から排除
            _nums.RemoveAt(index);
        }

        //次回用にクリアする
        _nums.Clear();
    }

    public int[] DangoCounts => dangoCounts;
    public FloorData[] FloorDatas => floorDatas;
    public DangoInjection[] DangoInjections => dangoInjections;
    public bool HasFlagSalvageableD5(int d5)
    {
        Logger.Assert(d5 is > 0 and <= 5);
        return salvageableD5.HasFlag((E_D5)(1 << d5));
    }
    public bool HasFlagIntrudableD5(int d5)
    {
        Logger.Assert(d5 is > 0 and <= 5);

        //判定しようとしている以下のフロアで侵入可能になっていたらそのフラグが立っていると同義
        //ex.(4d5を判定・・・3d5で侵入可能フロアにも侵入できる）
        for (int i = d5; i > 0; i--)
        {
            if (intrudableD5.HasFlag((E_D5)(1 << i))) return true;
        }

        return false;
    }
    public bool SetSalvationDango(DangoData dango)
    {
        if (dango == null)
        {
            _salvationDango = null;
            return true;
        }
        if (_salvationDango is not null) return false;

        _salvationDango = dango;
        return true;
    }
    public bool AlreadyExistSavlationDango()
    {
        return _salvationDango is not null;
    }
}