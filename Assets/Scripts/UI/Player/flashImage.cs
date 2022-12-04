using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class flashImage : MonoBehaviour
{
    ImageUIData uIData;
    Image myImage;
    [SerializeField]float time=5;
    [SerializeField] float speed = 3;
    [SerializeField] float Nomal = 1;
    [SerializeField] bool infinity;
    float alpha;
    float _time = 0;
    bool isFlash;
    // Start is called before the first frame update
    void Start()
    {
        uIData = GetComponent<ImageUIData>();
        myImage = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        if (myImage.sprite != null)
        {
            _time += Time.deltaTime;

            if (isFlash)
                alpha += Time.deltaTime * speed;
            else
                alpha -= Time.deltaTime * speed;

            if (alpha > Nomal && isFlash)
                isFlash = false;
            else if (alpha < 0 && !isFlash)
                isFlash = true;
            uIData.ImageData.SetAlpha(alpha);
        }
        else
            _time = 0;

        if (infinity)
            return;
        else if (_time >= time)
        {
            uIData.ImageData.SetSprite(null);
            uIData.ImageData.SetAlpha(0);
        }
    }
}
