using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using TM.Entity.Player;
using UnityEngine;

public class PlayerSpitColliderManager : MonoBehaviour
{
    PlayerAttackAction _playerAttack;
    [SerializeField] ImageUIData rangeUI;

    //突き刺し用のCollider。Player本体のコライダーはモデルについている
    private void OnTriggerEnter(Collider other)
    {
        _playerAttack.OnTriggerEnter(other);
    }

    //突き刺し用のCollider。Player本体のコライダーはモデルについている
    private void OnTriggerExit(Collider other)
    {
        _playerAttack.OnTriggerExit(other);
    }

    public void SetRangeUIEnable(bool isGroundOrEvent)
    {
        float alpha = isGroundOrEvent ? 155f / 255f : 0;

        rangeUI.ImageData.SetAlpha(alpha);
    }

    public void SetPlayerAttack(PlayerAttackAction playerAttackAction)
    {
        _playerAttack = playerAttackAction;
    }
}
