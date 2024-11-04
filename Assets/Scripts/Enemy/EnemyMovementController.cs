using System;
using System.Collections;
using Events;
using Health;
using UnityEngine;

namespace Enemy
{
    public class EnemyMovementController : EnemyController
    {
        [SerializeField] private EnemyConfigSO enemyConfig;
        [SerializeField] private VoidEventChannelSO onEnemyLeftEvent;
        [SerializeField] private float movementDelay;

        private Coroutine _moveCoroutine;

        public void SetInitialPosition()
        {
            transform.position = enemyConfig.offScreenPosition;
        }

        public void MoveIntoScreen()
        {
            if (_moveCoroutine != null)
                StopCoroutine(_moveCoroutine);

            _moveCoroutine = StartCoroutine(MoveIntoScreenCoroutine());
        }

        public void MoveOutOfScreen()
        {
            if (_moveCoroutine != null)
                StopCoroutine(_moveCoroutine);

            _moveCoroutine = StartCoroutine(MoveOutOfScreenCoroutine());
        }

        private IEnumerator MoveIntoScreenCoroutine()
        {
            yield return MoveTo(enemyConfig.offScreenPosition, enemyConfig.defaultPosition, enemyConfig.appearingDuration);
            enemyAgent.ChangeStateToIdle();
        }

        private IEnumerator MoveOutOfScreenCoroutine()
        {
            animationHandler.Recover();
            yield return new WaitForSeconds(movementDelay);
            yield return MoveTo(enemyConfig.weakenedPosition, enemyConfig.offScreenPosition, enemyConfig.leavingDuration);
            onEnemyLeftEvent?.RaiseEvent();
            enemyAgent.ChangeStateToIdle();
        }

        public IEnumerator MoveTo(Vector3 startingPos, Vector3 target, float duration)
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
        }
    }
}