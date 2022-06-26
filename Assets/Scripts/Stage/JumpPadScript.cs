using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpPadScript : MonoBehaviour
{
    [SerializeField] float jumpPower = 20f;

    private void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            if (col.GetComponent<Player1>().IsGround)
                col.GetComponent<Rigidbody>().AddForce(Vector3.up * jumpPower, ForceMode.Impulse);
        }
    }
}
