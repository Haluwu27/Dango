using System.Collections.Generic;
using System.Linq;
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

    private void Awake()
    {
        for (int i = 0; i < floorArrays.Length; i++)
        {
            floorArrays[i].SetFloor((Floor)i);

            foreach (var injection in floorArrays[i].DangoInjections)
            {
                injection.SetFloor((Floor)i);
            }
        }
    }

    public FloorArray[] FloorArrays => floorArrays;

}