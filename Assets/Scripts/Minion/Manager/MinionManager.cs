using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Events;
using Events.ScriptableObjects;
using Minion.Controllers;
using Minion.ScriptableObjects;
using UnityEngine;
using UnityEngine.Serialization;

namespace Minion.Manager
{
    public class MinionManager : MonoBehaviour
    {
        [SerializeField] private MinionSpawnerSO minionSpawnerConfig;
        [SerializeField] private MinionsManagerSO minionManagerConfig;
        [SerializeField] private GameObject player;

        [Header("Events")]
        [SerializeField] private MinionAgentEventChannelSO onMinionDeletedEvent;
        [SerializeField] private VoidEventChannelSO onAllMinionsDestroyedEvent;
        [SerializeField] private VoidEventChannelSO onGameplayEndEvent;
        [SerializeField] private MinionAgentEventChannelSO onMinionWantsToAttackEvent;
        [SerializeField] private MinionAgentEventChannelSO onMinionAttackedEvent;

        private List<MinionAgent> _minions;
        private bool _isSpawning;
        private Coroutine _spawnCoroutine;
        private List<MinionAgent> _attackQueue;
        private List<MinionAgent> _attackingMinions;

        protected void OnEnable()
        {
            _spawnCoroutine = StartCoroutine(SpawnMinions());
            onMinionDeletedEvent?.onTypedEvent.AddListener(HandleDeletedEvent);
            onGameplayEndEvent?.onEvent.AddListener(RemoveAllMinions);
            onMinionWantsToAttackEvent?.onTypedEvent.AddListener(AddAttackingMinion);
            onMinionAttackedEvent?.onTypedEvent.AddListener(RemoveAttackingMinion);
        }

        protected void OnDisable()
        {
            onMinionDeletedEvent?.onTypedEvent.RemoveListener(HandleDeletedEvent);
            onGameplayEndEvent?.onEvent.RemoveListener(RemoveAllMinions);
            onMinionWantsToAttackEvent?.onTypedEvent.RemoveListener(AddAttackingMinion);
            onMinionAttackedEvent?.onTypedEvent.RemoveListener(RemoveAttackingMinion);
            StopCoroutine(_spawnCoroutine);
        }

        private void AddAttackingMinion(MinionAgent minion)
        {
            _attackQueue.Add(minion);
            if (CanMinionAttack())
                HandleMinionOrder();
        }

        private void RemoveAttackingMinion(MinionAgent minion)
        {
            if (_attackingMinions.Contains(minion))
            {
                _attackingMinions.Remove(minion);
                if (CanMinionAttack() && _attackQueue.Count > 0)
                    HandleMinionOrder();
            }
        }

        private void RemoveAllMinions()
        {
            if (_minions == null) return;
            foreach (var minion in _minions.ToList())
            {
                _minions.Remove(minion);
                MinionObjectPool.Instance?.ReturnToPool(minion.gameObject);
            }

            _attackQueue.Clear();
            _attackingMinions.Clear();
        }

        private void HandleMinionOrder()
        {
            MinionAgent minion = _attackQueue[0];
            _attackQueue.RemoveAt(0);
            MinionIdleController minionIdleController = minion.GetComponent<MinionIdleController>();
            minionIdleController.SetCanAttack(true);
            _attackingMinions.Add(minion);
        }

        private void HandleDeletedEvent(MinionAgent deletedMinion)
        {
            MinionObjectPool.Instance?.ReturnToPool(deletedMinion.gameObject);

            _minions.Remove(deletedMinion);
            _attackingMinions.RemoveAll(aMinion => aMinion == deletedMinion);
            _attackQueue.Remove(deletedMinion);

            if (_minions.Count == 0 && !_isSpawning)
            {
                onAllMinionsDestroyedEvent?.RaiseEvent();
            }
        }

        private bool CanMinionAttack()
        {
            return _attackingMinions.Count < minionManagerConfig.maxMinionsAttackingAtSameTime;
        }

        private IEnumerator SpawnMinions()
        {
            _isSpawning = true;
            _minions = new List<MinionAgent>();
            _attackQueue = new List<MinionAgent>();
            _attackingMinions = new List<MinionAgent>();

            int minionsSpawned = 0;
            while (minionsSpawned < minionSpawnerConfig.minionsToSpawn)
            {
                if (minionsSpawned != 0) yield return new WaitForSeconds(minionSpawnerConfig.timeBetweenSpawns);
                if (_minions.Count >= minionManagerConfig.maxMinionsAtSameTime) continue;

                GameObject minion = MinionObjectPool.Instance?.GetPooledObject();
                if (minion == null)
                {
                    Debug.LogError("Minion was null");
                    break;
                }

                MinionAgent minionAgent = minion.GetComponent<MinionAgent>();
                minionAgent.SetPlayer(player);

                minion.transform.position = minionSpawnerConfig.GetSpawnPoint();
                minion.SetActive(true);

                _minions.Add(minionAgent);
                minionsSpawned++;
            }

            _isSpawning = false;
            yield return null;
        }

        public void Clear()
        {
            RemoveAllMinions();
        }
    }
}