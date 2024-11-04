using System;
using System.Collections;
using Enemy;
using Enemy.Shield;
using Events;
using Events.ScriptableObjects;
using FSM;
using Health;
using UnityEngine;
using Utils;

public class WeakenedController : EnemyController
{
    [Header("Properties")]
    [SerializeField] private EnemyConfigSO enemyConfig;
    [SerializeField] private HealthPoints healthPoints;
    [SerializeField] private ShieldController shieldController;
    [SerializeField] private EnemyMovementController movementController;
    [SerializeField] private bool shieldActive;

    [Header("ShieldProperties")]
    [SerializeField] private float timeToReactivateShield = 4.0f;
    [SerializeField] private float timeToStartReactivatingShield = 2.0f;

    [Header("Health canvas")]
    [SerializeField] private GameObject healthCanvas;

    [Header("Events")]
    [SerializeField] private VoidEventChannelSO onEnemyShouldDieEvent;
    [SerializeField] private VoidEventChannelSO onEnemyDeathEvent;
    [SerializeField] private EventChannelSO<bool> onEnemyParriedEvent;
    [SerializeField] private IntEventChannelSO onEnemyDamageEvent;

    private void OnEnable()
    {
        healthPoints.SetCanTakeDamage(false);
        shieldController.SetActive(true);
        shieldController.SetActiveMaterial();

        onEnemyShouldDieEvent?.onEvent.AddListener(HandleDeath);
        onEnemyDamageEvent?.onEvent.AddListener(HandleDamage);
    }

    private void OnDisable()
    {
        onEnemyShouldDieEvent?.onEvent.RemoveListener(HandleDeath);
        onEnemyDamageEvent?.onEvent.RemoveListener(HandleDamage);
    }

    private void HandleDeath()
    {
        Sequence deathSequence = new Sequence();
        deathSequence.SetAction(BossDeath());
        animationHandler.StartDeath();
        StartCoroutine(deathSequence.Execute());
    }


    private void HandleDamage()
    {
        animationHandler.ReceiveHit();
    }

    public void HandleDefensesDown()
    {
        Sequence weakenedSequence = new Sequence();
        weakenedSequence.AddPreAction(PlayWeakenedAnimation());
        weakenedSequence.AddPreAction(ToggleShield(false));
        weakenedSequence.AddPreAction(MoveTo(enemyConfig.defaultPosition, enemyConfig.weakenedPosition, enemyConfig.weakenedMoveDuration));
        weakenedSequence.SetAction(ReactivateShield());
        weakenedSequence.AddPostAction(ToggleShield(true));

        StartCoroutine(weakenedSequence.Execute());
    }

    private IEnumerator PlayWeakenedAnimation()
    {
        animationHandler.StartWeakened();
        yield return new WaitForSeconds(enemyConfig.weakenedStartDelay);
    }

    private IEnumerator ReactivateShield()
    {
        yield return new WaitForSeconds(timeToStartReactivatingShield);
        shieldController.SetIsActivating(true);
        yield return new WaitForSeconds(timeToReactivateShield);
        shieldController.SetActiveMaterial();
    }

    private IEnumerator ToggleShield(bool isActive)
    {
        healthPoints.SetCanTakeDamage(!isActive);
        shieldController.SetActive(isActive);
        onEnemyParriedEvent?.RaiseEvent(isActive);
        if (isActive)
        {
            shieldController.ResetShield();
            animationHandler.Recover();
            yield return MoveTo(enemyConfig.weakenedPosition, enemyConfig.defaultPosition, enemyConfig.recoverMoveDuration);
            enemyAgent.ChangeStateToIdle();
        }

        yield break;
    }

    private IEnumerator MoveTo(Vector3 startingPos, Vector3 target, float duration)
    {
        yield return new WaitForSeconds(enemyConfig.weakenedStartDelay);
        float timer = 0;
        float startingTime = Time.time;

        while (timer < duration)
        {
            timer = Time.time - startingTime;
            transform.position = Vector3.Lerp(startingPos, target, timer / duration);
            yield return null;
        }

        movementController.SetOriginalY();
    }

    private IEnumerator BossDeath()
    {
        animationHandler.StartDeath();
        healthCanvas.SetActive(false);
        yield return new WaitForSeconds(enemyConfig.deathAnimationDelay);
        yield return MoveTo(transform.position, enemyConfig.deathTargetPosition, enemyConfig.deathMovementDuration);
        onEnemyDeathEvent?.RaiseEvent();
    }
}