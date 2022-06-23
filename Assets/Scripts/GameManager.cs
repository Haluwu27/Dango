using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

internal class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject _dango = default!;
    public static ObjectPool<DangoManager> DangoPool { get; private set; }
    private static int _poolCount = 0;

    public static string NowRoleList = "";

    static public float gameScore = 0;

    //static private int PlayerNum = 1;
    static public Player1[] player = new Player1[2];

    QuestManager _questManager = new();
    static List<QuestData> quests = new();
    public static List<QuestData> Quests => quests;

    private void Awake()
    {
        DangoPool = new ObjectPool<DangoManager>(
            OnCreateDango,
            OnTakeFromPool,
            OnReturnedToPool,
            OnDestroyPoolObject,
            true,
            7 * 30,
            7 * 150
            );

    }

    private void Start()
    {

        quests.Add(_questManager.CreateQuestCreateRole(DangoRole.POSROLE_DIVIDED_INTO_TWO, 1, "役「二分割」を1個作れ"));
        quests.Add(_questManager.CreateQuestIncludeColor(DangoColor.Red, 3, "赤色を含めて役を3個作れ！"));
        foreach (var quest in quests)
        {
            Logger.Log(quest.QuestName);
        }

    }

    private DangoManager OnCreateDango()
    {
        //団子を取得
        var gameObject = Instantiate(_dango);

        //取得した団子からDangoManagerを取得
        var dango = gameObject.GetComponent<DangoManager>()
                    ?? gameObject.AddComponent<DangoManager>();

        //取得した段階で団子の色を設定
        dango.SetDangoType((DangoColor)_poolCount + 1);
        _poolCount++;
        if (_poolCount > 6) _poolCount = 0;

        dango.GetComponent<Renderer>().material.color = dango.GetDangoColor() switch
        {
            DangoColor.Red => Color.red,
            DangoColor.Orange => new Color32(255, 155, 0, 255),
            DangoColor.Yellow => Color.yellow,
            DangoColor.Green => Color.green,
            DangoColor.Cyan => Color.cyan,
            DangoColor.Blue => Color.blue,
            DangoColor.Purple => new Color32(200, 0, 255, 255),
            DangoColor.Other => Color.gray,
            _ => Color.white,
        };

        return dango;
    }

    private void OnTakeFromPool(DangoManager dango)
    {
        dango.gameObject.SetActive(true);
    }

    private void OnReturnedToPool(DangoManager dango)
    {
        dango.gameObject.SetActive(false);
        dango.gameObject.transform.SetPositionAndRotation(Vector3.zero, Quaternion.identity);
        dango.gameObject.transform.localScale = Vector3.one;
        dango.SetDangoType(DangoColor.None);
    }

    void OnDestroyPoolObject(DangoManager dango)
    {
        Destroy(dango.gameObject);
    }

    public static void SetPlayer(Player1 obj)
    {

        for (int i = 0; i < player.Length; i++)
        {
            if (player[i] == null)
            {
                player[i] = obj;
                break;
            }
        }
    }

    public void AddPlayer(GameObject obj)
    {
        //Camera cam = obj.GetComponentInChildren<Camera>();
        //if (PlayerNum == 1)
        //{
        //    cam.rect = new Rect(0, 0, 1f, 0.5f);
        //    Logger.Log("cam.rectを変更");

        //}
        //else if (PlayerNum == 2)
        //{
        //    cam.rect = new Rect(0, 0.5f, 1f, 0.5f);
        //    Logger.Log("cam.rectを変更");
        //    return;
        //}
        //    PlayerNum++;
    }
}
