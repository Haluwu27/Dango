using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveStageObjScript : MonoBehaviour
{
    [SerializeField, TooltipAttribute("移動量")] Vector3[] amountMove;
    [SerializeField] float moveSpeed;
    [SerializeField, TooltipAttribute("停止時間")] float delayTime;
    private Vector3[] move;
    private Vector3[] DestPos;
    int i=0,j=0;
    // Start is called before the first frame update
    void Start()
    {
        DestPos = new Vector3[amountMove.Length+1];
        move = new Vector3[amountMove.Length];
        DestPos[0] = transform.position ;

        foreach(Vector3 vector in amountMove)
        {
            move[j] = amountMove[j];//移動のためのベクトル
            DestPos[j+1] = move[j] + DestPos[j];//行きの目標地点
            j++;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (OnPositionComparison(DestPos[i+1]))
            StartCoroutine(DelayMove());


            Move(move[i]);
    }

    private void Move(Vector3 point)//移動
    {
        transform.Translate(point.normalized * moveSpeed * Time.deltaTime);
    }
    
    private bool OnPositionComparison(Vector3 point)//自分の位置と目標地点のposを比較
    {
        Vector3 pos = transform.position;
        float distance;
        distance = Vector3.Distance(pos, point);
        //差が1以下ならmoveベクトルの変更
        if (distance <= 1f) 
            return true;
        else
            return false;
    }

    private IEnumerator DelayMove()
    {
        i++;
        if (i >= move.Length)
        {
            i = 0;
            transform.position = DestPos[0];
        }
        else
            transform.position = DestPos[i];
        Vector3 temp = move[i];
        move[i] = Vector3.zero;
        yield return new WaitForSeconds(delayTime);
        move[i] = temp;
    }
}
