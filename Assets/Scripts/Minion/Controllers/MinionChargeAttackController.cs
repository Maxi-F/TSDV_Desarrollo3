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
        private LineRenderer _aimLine;
        private Vector3 _dir;
        private bool _isCharging;

        private Coroutine _chargeCoroutine;

        protected override void OnEnable()
        {
            _aimLine ??= gameObject.transform.Find("AimLine").gameObject.GetComponent<LineRenderer>();
            _aimLine.enabled = false;

            base.OnEnable();
        }

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

        private void SetNewAimPosition(float timer)
        {
            _dir = target.transform.position - transform.position;
            _dir.y = 0;

            float barProgress = minionConfig.chargeAttackData.chargeCurve.Evaluate(timer / minionConfig.chargeAttackData.duration);
            Vector3 aimPosition = Vector3.Lerp(transform.position, transform.position + _dir.normalized * minionConfig.chargeAttackData.length, barProgress);
            //_aimLine.SetPosition(1, aimPosition);
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
                SetNewAimPosition(timer);
                yield return null;
            }

            aimLineHandler.Alert();
            minionAttackController.AttackDir = _dir;
            yield return new WaitForSeconds(minionConfig.chargeAttackData.alertDuration);
            aimLineHandler.Release();
            yield return new WaitForSeconds(minionConfig.chargeAttackData.delayAfterLine);
            _aimLine.enabled = false;
            minionAgent.ChangeStateToAttack();
        }
    }
}