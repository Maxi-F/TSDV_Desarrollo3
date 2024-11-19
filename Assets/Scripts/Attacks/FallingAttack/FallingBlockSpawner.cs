using System;
using System.Collections;
using System.Collections.Generic;
using Events;
using LevelManagement;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Attacks.FallingAttack
{
    public class FallingBlockSpawner : MonoBehaviour
    {
        [Header("Events")] 
        [SerializeField] private Vector3EventChannelSO onPlayerPositionChanged;
        [SerializeField] private VoidEventChannelSO onHandleAttack;
        [SerializeField] private VoidEventChannelSO onFinishSpawningBlocks;
        [SerializeField] private GameObjectEventChannelSO onFallingBlockDisabledEvent;
        [SerializeField] private VoidEventChannelSO onGameplayEndedEvent;
        
        private Vector3 _playerPosition;
        private bool _isSpawning;
        private List<GameObject> _fallingBlocks;
        private FallingAttackData _fallingAttackData;
        
        private void OnEnable()
        {
            onGameplayEndedEvent?.onEvent.AddListener(HandleGameplayEnd);
            onPlayerPositionChanged?.onVectorEvent.AddListener(HandleNewPlayerPosition);
            onHandleAttack?.onEvent.AddListener(HandleSpawnBlocks);
            onFallingBlockDisabledEvent?.onGameObjectEvent.AddListener(HandleFallingBlockDisabled);
        }
        
        private void OnDisable()
        {
            onGameplayEndedEvent?.onEvent.RemoveListener(HandleGameplayEnd);
            onPlayerPositionChanged?.onVectorEvent.RemoveListener(HandleNewPlayerPosition);
            onHandleAttack?.onEvent.RemoveListener(HandleSpawnBlocks);
            onFallingBlockDisabledEvent?.onGameObjectEvent.RemoveListener(HandleFallingBlockDisabled);
        }

        public void SetFallingAttackData(FallingAttackData fallingAttackData)
        {
            _fallingAttackData = fallingAttackData;
        }

        private void HandleSpawnBlocks()
        {
            if (_fallingAttackData == null) return;
            _fallingBlocks = new List<GameObject>();
            StartCoroutine(SpawnBlocks(_fallingAttackData.spawnQuantity));
        }
        
        private void HandleGameplayEnd()
        {
            if (_fallingBlocks == null || _fallingBlocks.Count == 0) return;
            foreach (var fallingBlock in _fallingBlocks)
            {
                FallingBlockObjectPool.Instance?.ReturnToPool(fallingBlock);
            }
        }

        private void HandleNewPlayerPosition(Vector3 playerPosition)
        {
            _playerPosition = playerPosition;
        }

        private IEnumerator SpawnBlocks(int quantity)
        {            
            _isSpawning = true;
            for (int i = 0; i < quantity; i++)
            {
                GameObject fallingBlockInstance = FallingBlockObjectPool.Instance?.GetPooledObject();
                
                if (fallingBlockInstance == null)
                {
                    Debug.LogError("Falling block instance null");
                    yield return new WaitForSeconds(_fallingAttackData.timeBetweenSpawns);
                    continue;
                }
                
                Vector2 distance = CalculateRandomDistance();
                
                Vector3 fallingBlockPosition = new Vector3(
                    distance.x,
                    fallingBlockInstance.transform.position.y,
                    distance.y
                );

                fallingBlockInstance.transform.position = fallingBlockPosition;

                FallingBlock.FallingAttack fallingAttackScript =
                    fallingBlockInstance.GetComponentInChildren<FallingBlock.FallingAttack>();
                
                fallingAttackScript.SetAcceleration(_fallingAttackData.acceleration);
                
                fallingBlockInstance.SetActive(true);
                _fallingBlocks.Add(fallingBlockInstance);
                yield return new WaitForSeconds(_fallingAttackData.timeBetweenSpawns);
            }
            _isSpawning = false;
        }

        private void HandleFallingBlockDisabled(GameObject fallingBlock)
        {
            _fallingBlocks.Remove(fallingBlock);

            if (_fallingBlocks.Count > 0 || _isSpawning)
            {
                return;
            }

            onFinishSpawningBlocks?.RaiseEvent();
        }

        private Vector2 CalculateRandomDistance()
        {
            float xDistance = Random.Range(_playerPosition.x - _fallingAttackData.spawnRadiusFromPlayer, 
                _playerPosition.x + _fallingAttackData.spawnRadiusFromPlayer);
            float zDistance = Random.Range(_playerPosition.z - _fallingAttackData.spawnRadiusFromPlayer,
                _playerPosition.z + _fallingAttackData.spawnRadiusFromPlayer);

            return new Vector2(xDistance, zDistance);
        }
    }
}
