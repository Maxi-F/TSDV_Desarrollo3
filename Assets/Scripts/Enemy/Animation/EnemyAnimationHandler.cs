using UnityEngine;

[RequireComponent(typeof(Animator))]
public class EnemyAnimationHandler : MonoBehaviour
{
    private Animator _animator;

    private static readonly int bombThrow = Animator.StringToHash("BombThrow");
    private static readonly int attackUp = Animator.StringToHash("AttackUp");
    private static readonly int leftRecover = Animator.StringToHash("LeftRecovery");
    private static readonly int rightRecover = Animator.StringToHash("RightRecovery");
    private static readonly int laserBlend = Animator.StringToHash("BlendLaser");
    private static readonly int startLaser = Animator.StringToHash("StartLaser");
    private static readonly int weakened = Animator.StringToHash("BossExplosion");
    private static readonly int recover = Animator.StringToHash("Recover");
    private static readonly int receiveHit = Animator.StringToHash("GetHit");
    private static readonly int death = Animator.StringToHash("Death");

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    public void StartAttackUpAnimation()
    {
        _animator.SetTrigger(attackUp);
    }

    public void StartBombThrowAnimation()
    {
        _animator.SetTrigger(bombThrow);
    }

    public void StartLaserAnimation()
    {
        _animator.SetTrigger(startLaser);
    }

    public void StartRecoverAnimation(bool isRight)
    {
        if (isRight)
            _animator.SetTrigger(rightRecover);
        else
            _animator.SetTrigger(leftRecover);
    }

    public void SetLaserBlend(float value)
    {
        _animator.SetFloat(laserBlend, value);
    }

    public void StartWeakened()
    {
        _animator.SetTrigger(weakened);
    }

    public void Recover()
    {
        _animator.SetTrigger(recover);
    }

    public void ReceiveHit()
    {
        if (_animator.GetCurrentAnimatorClipInfo(0)[0].clip.name != "hitVulnerable")
            _animator.SetTrigger(receiveHit);
    }

    public void StartDeath()
    {
        _animator.SetTrigger(death);
    }
}