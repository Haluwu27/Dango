using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ゲームの進行に関わるもののみ定義
/// </summary>
internal class GameManager : MonoBehaviour
{
    public static float GameScore { get; set; } = 0;

    //static private int PlayerNum = 1;
    //static public Player1[] player { get; set; } = new Player1[2];

    QuestManager _questManager = new();

    private void Awake()
    {
        //マウスカーソルのやつ。
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        //最初のクエストを仮置き。
        _questManager.ChangeQuest(_questManager.Creater.CreateQuestCreateRole(DangoRole.POSROLE_DIVIDED_INTO_TWO, 1, "役「二分割」を1個作れ！"),
                               _questManager.Creater.CreateQuestIncludeColor(DangoColor.Red, 3, "赤色を含めて役を3個作れ！"));
    }

    //複数プレイヤーの処理（棄却されたもの）
    //public static void SetPlayer(Player1 obj)
    //{

    //    for (int i = 0; i < player.Length; i++)
    //    {
    //        if (player[i] == null)
    //        {
    //            player[i] = obj;
    //            break;
    //        }
    //    }
    //}

    //public void AddPlayer(GameObject obj)
    //{
    //    //Camera cam = obj.GetComponentInChildren<Camera>();
    //    //if (PlayerNum == 1)
    //    //{
    //    //    cam.rect = new Rect(0, 0, 1f, 0.5f);
    //    //    Logger.Log("cam.rectを変更");

    //    //}
    //    //else if (PlayerNum == 2)
    //    //{
    //    //    cam.rect = new Rect(0, 0.5f, 1f, 0.5f);
    //    //    Logger.Log("cam.rectを変更");
    //    //    return;
    //    //}
    //    //    PlayerNum++;
    //}
}
