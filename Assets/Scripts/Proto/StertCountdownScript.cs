using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class StertCountdownScript : MonoBehaviour
{
    int i = 0;

    TextMeshProUGUI text;
    Animator animator;
    [SerializeField] string[] words;
    [SerializeField]AudioClip clip;

    private void Start()
    {
        text = gameObject.GetComponent<TextMeshProUGUI>();
        animator = GetComponent<Animator>();
        animator.SetBool("stert", true);
        text.text = "";
        PlayerData.Event = true;
    }

    public void countDown()//アニメーションから呼び出し
    {
        if(i<words.Length)
            text.text = words[i];
        if (i >= words.Length)
        {
            text.text = "";
            animator.SetBool("stert", false);
        }

        i++;
    }
    public void playSE()
    {
       if (i == words.Length)
        {
            gameObject.GetComponent<AudioSource>().clip = clip;
            gameObject.GetComponent<AudioSource>().Play();
            PlayerData.Event = false;

        }
    }
}
