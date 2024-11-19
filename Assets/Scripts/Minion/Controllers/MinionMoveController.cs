using System;
using System.Collections;
using Events;
using Minion.ScriptableObjects;
using UnityEngine;

namespace Minion.Controllers
{
    public class MinionMoveController : MinionController
    {
        [SerializeField] private MinionSO minionConfig;
        [SerializeField] private Vector3EventChannelSO onPlayerMovedEvent;
        [SerializeField] private Vector3EventChannelSO onDashMovementEvent;
        [SerializeField] private float initialDelay;

        private Vector3 _moveDir;
        private Coroutine _moveCoroutine;

        public void OnDisable()
        {
            if (_moveCoroutine != null)
                StopCoroutine(_moveCoroutine);
        }

        public void Enter()
        {
            _moveCoroutine = StartCoroutine(HandleChangeState());
        }

        private IEnumerator HandleChangeState()
        {
            yield return new WaitForSeconds(initialDelay);
            float prevTime = Time.time;
            float delta = 0;
            while (Vector3.Distance(transform.position, target.transform.position) > minionConfig.moveData.minDistance)
            {
                delta = Time.time - prevTime;
                _moveDir = target.transform.position - transform.position;
                _moveDir.y = 0;
                transform.Translate(_moveDir * (minionConfig.moveData.speed * delta));
                prevTime = Time.time;
                yield return null;
            }

            minionAgent.ChangeStateToChargeAttack();
        }
    }
}