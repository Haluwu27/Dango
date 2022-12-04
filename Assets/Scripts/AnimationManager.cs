using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationManager
{
    public enum E_Animation
    {
        An1_Idle,
        An2_Run,
        An3_FreeFall,
        An4A_EatCharge,
        An4B_Eat,
        An5_Thrust,
        An6_Jump,
        An7A_FallAction,
        An7B_FallLanding,
        An8A_DangoRemoveCharge,
        An8B_DangoRemove,
        An9_Landing,
        An10_Walk,
        An11A_JumpCharge_Walk,
        An11B_JumpCharge,
        An11Start,
        AN3Start,

        Max,
    }

    Animator _anim;

    readonly List<int> _animHash = new();

    E_Animation _currentAnimation;

    static readonly List<string> names = new() { "AN1", "AN2", "AN3", "AN4A", "AN4B", "AN5", "AN6", "AN7A", "AN7B", "AN8A", "AN8B", "AN9", "AN10", "AN11A", "AN11B","AN11Start","AN3Start" };

    public AnimationManager(Animator anim)
    {
        _anim = anim;

        foreach (var name in names)
        {
            _animHash.Add(Animator.StringToHash(name));
        }
    }

    public void ChangeAnimation(E_Animation anim, float fadeTime)
    {
        if (_currentAnimation == anim) return;
        if (PlayerData.Event) return;

        ChangeAnimationEnforcement(anim, fadeTime);
    }

    public void ChangeAnimationEnforcement(E_Animation anim, float fadeTime)
    {
        if (PlayerData.Event) return;

        _currentAnimation = anim;

        _anim.CrossFadeInFixedTime(_animHash[(int)anim], fadeTime);
    }
}