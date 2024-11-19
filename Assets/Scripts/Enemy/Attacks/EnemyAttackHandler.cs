using System.Collections;
using System.Linq;
using Events;
using Events.ScriptableObjects;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Enemy.Attacks
{
    public class EnemyAttackHandler : MonoBehaviour
    {
        [SerializeField] private float timeBetweenAttacks;

        [Header("events")]
        [SerializeField] private VoidEventChannelSO onAttackExecutedEvent;

        private IEnemyAttack _actualAttack;
        private IEnemyAttack[] _attacks;

        void OnEnable()
        {
            _attacks ??= GetComponents<IEnemyAttack>();

            SelectRandomAttack();
        }

        public void PerformAttack()
        {
            StartCoroutine(ExecuteAttack());
        }

        private void SelectRandomAttack()
        {
            IEnemyAttack[] attacksToSearchFrom = _attacks.Where((attack) => attack.CanExecute()).ToArray();
            _actualAttack = attacksToSearchFrom[Random.Range(0, attacksToSearchFrom.Length)];
        }

        IEnumerator ExecuteAttack()
        {
            yield return null;
            SelectRandomAttack();
            yield return new WaitForSeconds(timeBetweenAttacks);
            yield return _actualAttack.Execute();
            onAttackExecutedEvent?.RaiseEvent();
        }
    }
}