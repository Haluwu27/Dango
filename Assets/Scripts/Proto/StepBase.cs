using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StepBase : MonoBehaviour
{
    PlayerData player1;
    Animator anima;
    StepHeightScript[] chaildSteps;
    int temp;
    // Start is called before the first frame update
    void Start()
    {
        player1 = GameObject.Find("PlayerParent").transform.Find("Player1").GetComponent<PlayerData>();

        anima = player1.GetAnimator();
        chaildSteps = new StepHeightScript[transform.childCount];
        for(int i=0; i < transform.childCount; i++)
        {
            chaildSteps[i] = this.gameObject.transform.GetChild(i).GetComponent<StepHeightScript>();
            chaildSteps[i].SetPlayer(player1.gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (anima.GetCurrentAnimatorStateInfo(0).IsName("AN11B"))
        {
            for (int i = 0; i < chaildSteps.Length; i++)
                chaildSteps[i].OnSet();
            temp = 0;
        }
        else if (temp == 0)
        {
            for (int i = 0; i < chaildSteps.Length; i++)
                chaildSteps[i].OffSet();
            temp = 1;
        }
    }
}
