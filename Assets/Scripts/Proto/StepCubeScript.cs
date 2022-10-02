using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StepCubeScript : MonoBehaviour
{
    private MeshRenderer mesh;
    public int current;
    // Start is called before the first frame update
    void Start()
    {
        mesh = GetComponent<MeshRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void green()
    {
        mesh.material.color = Color.green;
    }

    public void red()
    {
        mesh.material.color = Color.red;
    }
}
