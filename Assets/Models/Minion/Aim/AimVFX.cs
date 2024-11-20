using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimVFX : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private float aimingDuration = 1;
    [SerializeField] private float alertingDuration = 1;
    [SerializeField] private float releaseDuration = 1;

    private static readonly int _aimSpeed = Animator.StringToHash("aiming_speed_multiplier");
    private static readonly int _alertSpeed = Animator.StringToHash("alert_speed_multiplier");
    private static readonly int _releaseSpeed = Animator.StringToHash("release_speed_multiplier");

    private static readonly int _aimTrigger = Animator.StringToHash("AIM");
    private static readonly int _alertTrigger = Animator.StringToHash("ALERT");
    private static readonly int _releaseTrigger = Animator.StringToHash("RELEASE");

    public float AimingDuration { get => aimingDuration; set => aimingDuration = value; }
    public float AlertDuration { get => alertingDuration; set => alertingDuration = value; }
    public float ReleaseDuration { get => releaseDuration; set => releaseDuration = value; }

    void Update()
    {
        animator.SetFloat(_aimSpeed, 1 / aimingDuration);
        animator.SetFloat(_alertSpeed, 1 / alertingDuration);
        animator.SetFloat(_releaseSpeed, 1 / releaseDuration);
    }

    public void Aim()
    {
        animator.SetTrigger(_aimTrigger);
    }

    public void Alert()
    {
        animator.SetTrigger(_alertTrigger);
    }

    public void Release()
    {
        animator.SetTrigger(_releaseTrigger);
    }
}