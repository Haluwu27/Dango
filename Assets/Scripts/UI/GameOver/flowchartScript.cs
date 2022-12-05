using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class flowchartScript : MonoBehaviour
{
    [SerializeField] LineRenderer lineRenderer;
    public List<Vector3> poss;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Vector3[] points = new Vector3[poss.Count];
        for (int i = 0; i < points.Length; i++)
            points[i] = poss[i];

        lineRenderer.SetPositions(points);
    }

    public void posSet(Vector3 pos)
    {
        poss.Add(pos);
    }
}
