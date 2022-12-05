using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoleEffect : MonoBehaviour
{
    [SerializeField] GameObject onlyEffects;
    [SerializeField] GameObject repetitionEffects;
    [SerializeField] GameObject loopEffects;
    [SerializeField] GameObject mirrorEffects;
    [SerializeField] GameObject mirrorThreeEffects;
    private GameObject EffectObj;
    private ParticleSystem particle;
    public Color color =Color.white;
    void Start()
    {
        
    }

    public void RoleSetEffect(DangoColor color)
    {

        switch (DangoRoleUI.CurrentRoleName)
        {
            case "「一統団結」":
                onlyEffects.SetActive(true);
                SetEffect(onlyEffects);
                ChangeColor(color);
                particle.Play();
                break;
            case "「全天鏡面」":
                repetitionEffects.SetActive(true);
                SetEffect(repetitionEffects);
                ChangeColor(DangoColor.None);
                particle.Play();
                break;
            case "「輪廻転生」":
                loopEffects.SetActive(true);
                SetEffect(loopEffects);
                ChangeColor(DangoColor.None);
                particle.Play();
                break;
            case "「隣色鏡面」":
                mirrorEffects.SetActive(true);
                SetEffect(mirrorEffects);
                ChangeColor(DangoColor.None);
                particle.Play();
                break;
            case "「三面華鏡」":
                mirrorThreeEffects.SetActive(true);
                SetEffect(mirrorThreeEffects);
                ChangeColor(DangoColor.None);
                particle.Play();
                break;
        }
}
    private void Update()
    {
        if (particle != null)
        {
            var main = particle.main;
            main.startColor = color;
        }
    }
    private void SetEffect(GameObject effect)
    {
        //エフェクトの表示
        particle =effect.GetComponent<ParticleSystem>();

    }
    private void ChangeColor(DangoColor _color)
    {
        var main = particle.main;
        color = _color switch
        {
            DangoColor.An => new Color(0.58f, 0.145f, 0.227f),
            DangoColor.Beni => new Color(1, 0.784f, 0.784f),
            DangoColor.Mitarashi => new Color(0.745f, 0.431f, 0.258f),
            DangoColor.Nori => new Color(1, 0.651f, 0.612f),
            DangoColor.Shiratama => new Color(1,1,1),
            DangoColor.Yomogi => new Color(0.706f, 0.831f, 0.372f),
            DangoColor.None => Color.white,
        };
    }
    }
