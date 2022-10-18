using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    private GameObject Cube;
    private int currentStabCount;
    private List<MeshRenderer> cubeMesh = new List<MeshRenderer>();
    Player1 player1;

    private Vector3[] xyz = new Vector3[8];
    // Start is called before the first frame update
    void Start()
    {
        player1 = GameObject.Find("PlayerParent").transform.Find("Player1").GetComponent<Player1>();
        Cube = Resources.Load("Cube") as GameObject;
        xyz[0] = new Vector3(1, 1, 1);
        xyz[1] = new Vector3(1, 1, -1);
        xyz[2] = new Vector3(1, -1, 1);
        xyz[3] = new Vector3(-1, 1, 1);
        xyz[4] = new Vector3(-1, -1, 1);
        xyz[5] = new Vector3(1, -1, -1);
        xyz[6] = new Vector3(-1, 1, -1);
        xyz[7] = new Vector3(-1, -1, -1);
        InstantiateImage();

    }

    // Update is called once per frame
    void Update()
    {
        SetCollar();
    }

    private void SetCollar()
    {
        currentStabCount = player1.GetMaxDango();

        for (int i = 0; i < cubeMesh.Count; i++)
        {
            if ((int)objD5 <= currentStabCount)
            {
                cubeMesh[i].material.color = Color.green;
            }
            else
            {
                cubeMesh[i].material.color = Color.red;
            }
        }
    }

    private void InstantiateImage()
    {
            Vector3[] Vecs = Getpos(gameObject);

            for (int j = 0; j < Vecs.Length; j++)
            {
                GameObject obj = Instantiate(Cube, Vecs[j], Quaternion.identity);
                obj.transform.parent =gameObject.transform;
            cubeMesh.Add(obj.GetComponent<MeshRenderer>());
            }
    }

    private Vector3[] Getpos(GameObject obj)
    {
        Vector3[] vec = new Vector3[8];

        for (int i = 0; i < vec.Length; i++)
        {
            vec[i] = new Vector3(Getpos_C(obj.transform.position.x, obj.transform.localScale.x * xyz[i].x) + (0.05f * xyz[i].x), Getpos_C(obj.transform.position.y, obj.transform.localScale.y * xyz[i].y) + (0.05f * xyz[i].y), Getpos_C(obj.transform.position.z, obj.transform.localScale.z * xyz[i].z) + (0.05f * xyz[i].z));
        }
        return vec;
    }

    private float Getpos_C(float pos,float scale)
    {
        return (pos + (scale / 2f));
    }
}
