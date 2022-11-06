using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EatCamCubeScript : MonoBehaviour
{
    [SerializeField] LayerMask Mask;

    private List<Renderer> _rend = new();

    private void OnDisable()
    {
        SetRendEnable(true);
    }

    private void OnTriggerEnter(Collider col)
    {
        if (col.TryGetComponent(out Renderer rend))
        {
            _rend.Add(rend);
            rend.enabled = false;
        }
    }

    private void SetRendEnable(bool enable)
    {
        foreach (Renderer rend in _rend)
        {
            rend.enabled = enable;
        }
        if (enable) _rend.Clear();
    }
}
