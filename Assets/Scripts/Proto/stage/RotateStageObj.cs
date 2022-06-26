using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateStageObj : MonoBehaviour
{
    [SerializeField, Tooltip("何か起点になるオブジェクトがあればここ")] GameObject pointObj;
    [SerializeField, Tooltip("回転の中心")] Vector3 point;
    [SerializeField, Tooltip("中心からどのくらい離れるか")] Vector3 distance;
    [SerializeField] float speed;

    // Start is called before the first frame update
    void Start()
    {
        if (pointObj != null)
            point = pointObj.transform.position;

        transform.position = point + distance;
    }

    // Update is called once per frame
    void Update()
    {
        transform.RotateAround(point.normalized, Vector3.up, speed * Time.deltaTime);
    }
}
