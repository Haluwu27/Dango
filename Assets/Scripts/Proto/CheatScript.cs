using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CheatScript : MonoBehaviour
{
    [SerializeField] PlayerData player;
    [SerializeField] DangoUIScript uIScript;
    [SerializeField] TMP_Dropdown D5_dropdown;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void QuestClear()
    {
        List<QuestData> quest = QuestManager.Instance.GetQuests;
        List<QuestData> nextQuest = new();
        for (int i = 0; i < quest[0].NextQuestId.Count; i++)
        {
            nextQuest.Add(Stage001Data.Instance.QuestData[quest[0].NextQuestId[i]]);
        }
        QuestManager.Instance.ChangeQuest(nextQuest);
        quest[0].SetKeyQuest(true);
    }
    public void D5Set()
    {
        player.ResetDangos();
        player.SetMaxDango(D5_dropdown.value + 3);
        uIScript.GetComponent<DangoUIScript>().DangoUISet(player.GetDangos());
    }

    public void Dangoset(DangoColor c)
    {
        if (player.GetDangos().Count < player.GetMaxDango())
        {
            player.AddDangos(c);
            uIScript.GetComponent<DangoUIScript>().DangoUISet(player.GetDangos());
        }
    }

    public void DangoClear()
    {
        player.ResetDangos();
        uIScript.GetComponent<DangoUIScript>().DangoUISet(player.GetDangos());
    }
}
