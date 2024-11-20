using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimVFX : MonoBehaviour
{
    [SerializeField] Animator animator;
    [SerializeField] float aimingDuration = 1;
    [SerializeField] float alertingDuration = 1;
    [SerializeField] float releaseDuration = 1;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        animator.SetFloat("aiming_speed_multiplier", 1/aimingDuration);
        animator.SetFloat("alert_speed_multiplier", 1/alertingDuration);
        animator.SetFloat("release_speed_multiplier", 1/releaseDuration);

    }
    public void Aim()
    {
        animator.SetTrigger("AIM");
    }
}
