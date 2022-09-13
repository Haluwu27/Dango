using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class fadeInTextScript : MonoBehaviour
{
    [SerializeField] float fadeSpeed = 0.001f;
    float red, green, blue, alfa;

    public bool isFadeOut = true;

    [SerializeField]TextMeshProUGUI fadeText;
    // Start is called before the first frame update
    void Start()
    {
        fadeText = GetComponent<TextMeshProUGUI>();
        red = fadeText.color.r;
        green = fadeText.color.g;
        blue = fadeText.color.b;
    }

    private void OnEnable()
    {
        alfa = 0;
        red = fadeText.color.r;
        green = fadeText.color.g;
        blue = fadeText.color.b;
        fadeText.color = new Color(red, green, blue, alfa);
        isFadeOut = true;
    }
    void Update()
    {

        if (isFadeOut)
        {
            alfa += fadeSpeed;
            SetAlpha();
            if (alfa >= 1)
            {
                isFadeOut = false;
            }
        }
    }
    void SetAlpha()
    {
        fadeText.color = new Color(red, green, blue, alfa);
    }
}
