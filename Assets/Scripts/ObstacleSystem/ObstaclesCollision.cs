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
        [SerializeField] private GameObjectEventChannelSO onObstacleTriggeredEvent;

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                other.TryGetComponent<ITakeDamage>(out ITakeDamage playerHealth);
                if (playerHealth.TryTakeDamage(collisionDamage))
                    onObstacleTriggeredEvent.RaiseEvent(gameObject);
            }
        }
    }
}