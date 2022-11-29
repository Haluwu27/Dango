using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class DontEatScript : MonoBehaviour
{
    ImageUIData uIData;
    Image myImage;
    [SerializeField]float time=5;
    [SerializeField] float speed = 3;
    float alpha;
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
            time += Time.deltaTime;

            if (isFlash)
                alpha += Time.deltaTime * speed;
            else
                alpha -= Time.deltaTime * speed;

            if (alpha > 1f && isFlash)
                isFlash = false;
            else if (alpha < 0 && !isFlash)
                isFlash = true;
            uIData.ImageData.SetAlpha(alpha);
        }
        else
            time = 0;

        if (time >= 5)
        {
            uIData.ImageData.SetSprite(null);
            uIData.ImageData.SetAlpha(0);
        }
    }
}
