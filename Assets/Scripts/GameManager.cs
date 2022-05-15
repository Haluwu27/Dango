using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    static private int PlayerNum = 1;
    static public Plater1[] player = new Plater1[2];

    public static void SetPlayer(Plater1 obj)
    {
        for (int i = 0; i < player.Length; i++) {
            if (player[i] == null)
            {
                player[i] = obj;
                break;
            }
        }
    }

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
