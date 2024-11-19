using System;
using System.Collections.Generic;
using Health;
using ParryProjectile;
using UnityEngine;

namespace Player.Weapon
{
    public class MeleeWeapon : MonoBehaviour
    {
        [Header("Enemy")]
        [SerializeField] private GameObject enemy;

        [SerializeField] private int damage;

        private List<Collider> hittedEnemies = new List<Collider>();

        private void OnDisable()
        {
            ResetHittedEnemiesBuffer();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!this.enabled || hittedEnemies.Contains(other))
                return;

            if (other.transform.CompareTag("Deflectable"))
            {
                if (other.transform.TryGetComponent<IDeflectable>(out IDeflectable deflectableInterface))
                {
                    deflectableInterface.Deflect(enemy);
                }

                hittedEnemies.Add(other);
            }

            if (other.transform.CompareTag("Enemy"))
            {
                if (other.transform.TryGetComponent<ITakeDamage>(out ITakeDamage takeDamageInterface))
                {
                    takeDamageInterface.TryTakeDamage(damage);
                }

                hittedEnemies.Add(other);
            }
        }

        public void ResetHittedEnemiesBuffer()
        {
            hittedEnemies.Clear();
        }
    }
}