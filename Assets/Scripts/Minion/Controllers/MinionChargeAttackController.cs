using System;
using System.Collections;
using Events;
using Health;
using Minion.ScriptableObjects;
using UnityEngine;

namespace Minion.Controllers
{
    public class MinionChargeAttackController : MinionController
    {
        [SerializeField] private MinionSO minionConfig;
        [SerializeField] private MinionAttackController minionAttackController;
        [SerializeField] private AimVFX aimLineHandler;
        private Vector3 _dir;
        private bool _isCharging;

        private Coroutine _chargeCoroutine;

        private void OnDisable()
        {
            if (_chargeCoroutine != null)
                StopCoroutine(_chargeCoroutine);
        }

        public void Enter()
        {
            _chargeCoroutine = StartCoroutine(AttackCoroutine());
        }

        private void StartAimLine()
        {
            aimLineHandler.Aim();
        }

        private void SetNewAimPosition()
        {
            _dir = target.transform.position - transform.position;
            _dir.y = 0;
            LookAtTarget();
        }

        private IEnumerator AttackCoroutine()
        {
            float timer = 0;
            float startTime = Time.time;
            StartAimLine();

            while (timer < minionConfig.chargeAttackData.duration)
            {
                timer = Time.time - startTime;
                SetNewAimPosition();
                yield return null;
            }

            aimLineHandler.Alert();
            minionAttackController.AttackDir = _dir;
            yield return new WaitForSeconds(minionConfig.chargeAttackData.alertDuration);
            aimLineHandler.Release();
            yield return new WaitForSeconds(minionConfig.chargeAttackData.delayAfterLine);
            minionAgent.ChangeStateToAttack();
        }
    }
}