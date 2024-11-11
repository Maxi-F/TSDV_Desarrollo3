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

        private List<Collider> _hittedEnemies = new List<Collider>();

        private void OnDisable()
        {
            ResetHittedEnemiesBuffer();
        }

        private void OnTriggerEnter(Collider other)
        {
            if(other.gameObject.name.Contains("Minion"))
                Debug.Log($"MeleeWeapon. ENABLED: {this.enabled}, HITTED ENEMIES: {_hittedEnemies.Contains(other)}, OTHER: {other.gameObject.name}");
            
            if (!this.enabled || _hittedEnemies.Contains(other) || other.CompareTag("Player"))
                return;
            
            if (other.transform.TryGetComponent<IDeflectable>(out IDeflectable deflectableInterface))
            {
                deflectableInterface.Deflect(enemy);
                _hittedEnemies.Add(other);
            }
            
            if (other.transform.TryGetComponent<ITakeDamage>(out ITakeDamage takeDamageInterface))
            {
                takeDamageInterface.TryTakeDamage(damage);
                _hittedEnemies.Add(other);
            }
        }
        
        public void SetIsInteractive(bool value)
        {
            gameObject.SetActive(value);
        }

        public void ResetHittedEnemiesBuffer()
        {
            _hittedEnemies.Clear();
        }
    }
}