using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MAXDangoUIScript : MonoBehaviour
{
    [SerializeField] TextUIData textUIData;
    [SerializeField] ImageUIData imageUIData;
    [SerializeField] float speed = 1;
    public float alpha = 0;
    private bool fade = false;
    public void Start()
    {
        gameObject.SetActive(false);
    }
    public void Update()
    {
        IsFade();
    }

    private void IsFade()
    {
        if (fade)
            alpha += Time.deltaTime * speed;
        else
            alpha -= Time.deltaTime * speed;

        if (alpha > 1f && fade)
            fade = false;
        else if (alpha < 0 && !fade)
            fade = true;
        textUIData.TextData.SetAlpha(alpha);
        imageUIData.ImageData.SetAlpha(alpha);
    }
}
