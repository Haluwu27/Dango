using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveStageObjScript : MonoBehaviour
{
    [SerializeField, Tooltip("移動量")] Vector3[] amountMove;
    [SerializeField, Tooltip("スピード")] float moveSpeed;
    [SerializeField, Tooltip("停止時間")] float delayTime;

    //移動する目標地点座標
    Vector3[] DestPos;

    //地点
    int point = 0;

    bool isStay = false;

    // Start is called before the first frame update
    void Start()
    {
        DestPos = new Vector3[amountMove.Length + 1];
        DestPos[0] = transform.position;

        for (int i = 0; i < amountMove.Length; i++)
        {
            DestPos[i + 1] = amountMove[i] + DestPos[i];//行きの目標地点
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (OnPositionComparison(DestPos[point + 1]))
        {
            StartCoroutine(DelayMove());
        }

        Move(amountMove[point]);
    }

    private void Move(Vector3 point)//移動
    {
        if (isStay) return;

        transform.Translate(moveSpeed * Time.deltaTime * point.normalized);
    }

    private bool OnPositionComparison(Vector3 point)//自分の位置と目標地点のposを比較
    {
        return Vector3.Distance(transform.position, point) <= 0.1f;
    }

    private IEnumerator DelayMove()
    {
        isStay = true;

        point = (point + 1) % amountMove.Length;
        transform.position = DestPos[point];

        yield return new WaitForSeconds(delayTime);

        isStay = false;
    }
}
