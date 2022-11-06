using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class StertCountdownScript : MonoBehaviour
{
    int a = 3;
    int i = 0;

    TextMeshProUGUI text;
    Animator animator;
    [SerializeField] string[] words;

    private void Start()
    {
        text = gameObject.GetComponent<TextMeshProUGUI>();
        animator = GetComponent<Animator>();
        animator.SetBool("stert", true);
        text.text = "3";
    }

    public void countDown()//アニメーションから呼び出し
    {
        i++;
        text.text = (a - i).ToString("0");

        if (i == a)
            text.text = "始め！";
        if (i > a)
        {
            text.text = "";
            animator.SetBool("stert", false);
        }
    }
}
