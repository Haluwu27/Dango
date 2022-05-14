using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    static private int PlayerNum = 1;

    public void AddPlayer(GameObject obj)
    {
        Camera cam = obj.GetComponentInChildren<Camera>();
        if (PlayerNum == 1)
        {
            cam.rect = new Rect(0, 0, 1f, 0.5f);
            Logger.Log("cam.rect‚ğ•ÏX");

        }
        else if (PlayerNum == 2)
        {
            cam.rect = new Rect(0, 0.5f, 1f, 0.5f);
            Logger.Log("cam.rect‚ğ•ÏX");
            return;
        }
            PlayerNum++;
    }
}
