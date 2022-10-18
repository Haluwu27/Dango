using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EatCamCubeScript : MonoBehaviour
{
    [SerializeField] LayerMask Mask;
    public List<GameObject> objects = new List<GameObject>();
    private void OnTriggerEnter(Collider col)
    {
            objects.Add(col.gameObject);
        
    }
}
