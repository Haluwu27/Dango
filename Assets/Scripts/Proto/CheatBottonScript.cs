using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheatBottonScript : MonoBehaviour
{
    [SerializeField] CheatScript BaseSc;
    public DangoColor dangoColor;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void OnClickedButton()
    {
        BaseSc.Dangoset(dangoColor);

    }
}
