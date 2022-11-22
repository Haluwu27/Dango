using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StepHeightScript : MonoBehaviour
{
    public enum D5
    {
        D3 = 3,
        D4 = 4,
        D5 = 5,
        D6 = 6,
        D7 = 7,
    }

    [SerializeField] D5 objD5;
    private int currentStabCount;
    private Renderer renderer = new Renderer();
    PlayerData player1;

    private Vector3[] xyz = new Vector3[8];
    // Start is called before the first frame update
    void Start()
    {
        renderer = gameObject.GetComponent<Renderer>();
        InputSystemManager.Instance.onExpansionUIPerformed += OnSet;
        InputSystemManager.Instance.onExpansionUICanceled += OffSet;
        gameObject.SetActive(false);

    }
    private void OnDestroy()
    {
        InputSystemManager.Instance.onExpansionUIPerformed -= OnSet;
        InputSystemManager.Instance.onExpansionUICanceled -= OffSet;
    }

    // Update is called once per frame
    void Update()
    {
        SetCollar();
            OnSet();
    }

    private void SetCollar()
    {
        currentStabCount = player1.GetMaxDango();

        if ((int)objD5 <= currentStabCount)
            renderer.material.color = Color.green;
        else
            renderer.material.color = Color.red;
    }

    public void OnSet()
    {
        this.gameObject.SetActive(true);
    }
    public void OffSet()
    {
        this.gameObject.SetActive(false);
    }

    public void SetPlayer(GameObject playerData) => player1 = playerData.GetComponent<PlayerData>();
}
