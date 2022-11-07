using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFootSE : MonoBehaviour
{
    //[SerializeField] Transform footTransform;
    //[SerializeField] float raycastDistance = 1f;
    //[SerializeField] LayerMask groundLayers = 0;
    //[SerializeField] float minFootstepInterval = 0.5f;
    //bool timerIsActive = false;
    //WaitForSeconds footstepWait;
    //void Start()
    //{
    //    footstepWait = new WaitForSeconds(minFootstepInterval);
    //}
    //void Update()
    //{
    //    CheckGroundStatus();
    //}
    //void CheckGroundStatus()
    //{
    //    if (timerIsActive) return;

    //    Debug.DrawRay(footTransform.position, Vector3.down, Color.blue, raycastDistance);

    //    bool isGrounded = Physics.Raycast(footTransform.position, Vector3.down, raycastDistance, groundLayers, QueryTriggerInteraction.Ignore);
    //    if (isGrounded && InputSystemManager.Instance.MoveAxis.magnitude > 0)
    //    {
    //        PlayFootSE();
    //    }
    //    StartCoroutine(nameof(FootstepTimer));
    //}
    public void PlayFootSE()
    {
        SoundManager.Instance.PlaySE(SoundSource.SE1_FOOTSTEPS_HARD);
    }

    //IEnumerator FootstepTimer()
    //{
    //    timerIsActive = true;
    //    yield return footstepWait;
    //    timerIsActive = false;
    //}
}