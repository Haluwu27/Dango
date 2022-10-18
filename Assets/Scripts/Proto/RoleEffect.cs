using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoleEffect : MonoBehaviour
{
    [SerializeField] GameObject[] Effects;
    private GameObject EffectObj;
    private ParticleSystem particle;
    void Start()
    {
        
    }

    void SetEffect()
    {
        switch (DangoRoleUI.CurrentRoleName)
        {
            case "単色役":
                SetEffect(Effects[0]);
                break;
            case "線対称":
                SetEffect(Effects[1]);
                break;
            case "ループ":
                SetEffect(Effects[2]);
                break;
            case "二分割":
                SetEffect(Effects[3]);
                break;
            case "三分割":
                SetEffect(Effects[4]);
                break;
        }
}
    private void Update()
    {
        if(particle!=null)
        if (particle.isStopped) //パーティクルが終了したか判別
        {
            Destroy(EffectObj);//パーティクル用ゲームオブジェクトを削除
        }
    }
    private void SetEffect(GameObject effect)
    {
        //エフェクトの表示
        EffectObj = Instantiate(effect, this.transform.position, Quaternion.identity);
        particle =EffectObj.GetComponent<ParticleSystem>();

    }
}
