using System;
using Enemy.Attacks;
using Events;
using Health;
using UnityEngine;

namespace ObstacleSystem
{
    public class ObstaclesCollision : MonoBehaviour
    {
        [SerializeField] private int collisionDamage;
        [SerializeField] private ObstacleDestroy obstacleDestroyer;

        private bool _canTrigger;

        private void OnEnable()
        {
            _canTrigger = true;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!_canTrigger) return;
            
            if (other.CompareTag("Player"))
            {
                other.TryGetComponent<ITakeDamage>(out ITakeDamage playerHealth);
                if (playerHealth.TryTakeDamage(collisionDamage))
                    obstacleDestroyer.DestroyObstacle();
            }
        }

        public void SetCanTrigger(bool value)
        {
            _canTrigger = value;
        }
    }
}