using System.Collections.Generic;
using UnityEngine;

public class FloorManager : MonoBehaviour
{
    public enum Floor
    {
        floor1,
        floor2,
        floor3,
        floor4,
        floor5,
        floor6,
        floor7,
        floor8,
        floor9,
        floor10,
        floor11,
        floor12,
        floor13,
        floor14,
        floor15,
        floor16,
        floor17,
        floor18,
        floor19,
        floor20,
        floor21,
        floor22,
        floor23,
        floor24,
        floor25,
        floor26,
        floor27,
        floor28,
        floor29,
        floor30,
        floor31,
        floor32,
        floor33,
        floor34,

        [InspectorName("")]
        Max,
    }

    //二次元配列はシリアライズできないため、別クラスを用いて仮想的に二次元配列にしている。
    [SerializeField] FloorArray[] floorArrays = new FloorArray[(int)Floor.Max];

    //重複しない乱数取得用
    List<int> _nums = new();

    public void CheckDangoIsFull(Collider other, Floor floor)
    {
        //団子以外を弾く
        if (other.GetComponent<DangoData>() == null) return;

        //団子の数がフロアの最大値に到達していなかったら弾く
        if (!CheckFloorDangoIsFull((int)floor)) return;

        //最大数ならすべて射出禁止にする
        foreach (var dI in floorArrays[(int)floor].DangoInjections)
        {
            dI.SetCanShot(false);
        }
    }

    public void CheckDangoIsNotFull(Collider other, Floor floor, int shotValue)
    {
        //団子以外を弾く
        if (other.GetComponent<DangoData>() == null) return;

        //団子の数がフロアの最大値に到達していたら弾く
        if (CheckFloorDangoIsFull((int)floor)) return;

        //一旦すべて射出禁止にして
        foreach (var dI in floorArrays[(int)floor].DangoInjections)
        {
            dI.SetCanShot(false);
        }

        //乱数用のインデックス番号を取得
        for(int i = 0; i < floorArrays[(int)floor].DangoInjections.Length; i++)
        {
            _nums.Add(i);
        }

        //ランダムな装置を選択する
        for (int i = 0; i < shotValue; i++)
        {
            //インデックス番号を取得
            int index = Random.Range(0, _nums.Count);

            //重複しないランダムな発射装置の発射フラグを立てる
            floorArrays[(int)floor].DangoInjections[_nums[index]].SetCanShot(true);
            
            //今回取得した番号を選択肢から排除
            _nums.RemoveAt(index);
        }

        //次回用にクリアする
        _nums.Clear();
    }

    bool CheckFloorDangoIsFull(int index)
    {
        int count = 0;

        //ワンフロアに複数Dataが存在する場合合算する
        for (int i = 0; i < floorArrays[index].FloorDatas.Length; i++)
        {
            count += floorArrays[index].FloorDatas[i].DangoCount;
        }

        return count >= floorArrays[index].MaxDangoCount;
    }
}
