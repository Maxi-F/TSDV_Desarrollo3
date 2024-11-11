using System;
using System.Collections;
using System.Collections.Generic;
using Events;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class PlayerAnimationHandler : MonoBehaviour
{
    [SerializeField] private PlayerAnimationConfigSO animationConfig;
    
    private Animator _animator;
    
    private static readonly int VelocityZ = Animator.StringToHash("velocity_z");
    private static readonly int VelocityX = Animator.StringToHash("velocity_x");
    private static readonly int Attack = Animator.StringToHash("Attack");
    private static readonly int AttackProgress = Animator.StringToHash("attackProgress");
    private static readonly int Hit = Animator.StringToHash("GetHit");
    private static readonly int Death = Animator.StringToHash("Death");

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    public void HandleResetAnimator()
    {
        _animator.Rebind();
        _animator.Update(0f);
    }

    public void SetPlayerDirection(Vector2 normalizedDir)
    {
        _animator.SetFloat(VelocityX, normalizedDir.x, animationConfig.moveDampTime, Time.deltaTime);
        _animator.SetFloat(VelocityZ, normalizedDir.y, animationConfig.moveDampTime, Time.deltaTime);
    }

    public void GetHit()
    {
        _animator.SetTrigger(Hit);
    }

    public void StartDeathAnimation()
    {
        _animator.SetTrigger(Death);
    }
    
    public void StartAttackAnimation()
    {
        _animator.SetBool(Attack, true);
    }
    
    public void RestartAttackAnimation()
    {
        _animator.CrossFade(Attack, 0.25f);
    }

    public void SetAttackProgress(float progress)
    {
        _animator.SetFloat(AttackProgress, progress);
    }
    
    public void EndAttackAnimation()
    {
        _animator.SetBool(Attack, false);
    }
}