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

    public void RoleSetEffect(DangoColor color)
    {
        switch (DangoRoleUI.CurrentRoleName)
        {
            case "単色役":
                SetEffect(Effects[0]);
                ChangeColor(particle,color);
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
    private void ChangeColor(ParticleSystem particleSystem,DangoColor color)
    {
        particleSystem.startColor = color switch
        {
            DangoColor.An => new Color(153, 37,58),
            DangoColor.Beni => new Color(255, 200, 200),
            DangoColor.Mitarashi => new Color(191, 110,66),
            DangoColor.Nori => new Color(253, 166, 156),
            DangoColor.Shiratama => new Color(252,252,252),
            DangoColor.Yomogi => new Color(180, 212, 95),
            _ => new Color(0,0,0),
        };
    }
    }
