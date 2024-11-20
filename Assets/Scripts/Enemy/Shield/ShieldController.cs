using System;
using System.Collections;
using Events;
using Health;
using UnityEngine;
using UnityEngine.Serialization;

namespace Enemy.Shield
{
    public class ShieldController : MonoBehaviour
    {
        [SerializeField] private HealthPoints _healthPoints;

        [SerializeField] private Material activeMaterial;
        [SerializeField] private AnimationCurve blinkCurve;
        [SerializeField] private GameObject shieldModel;
        private bool _isActive = true;
        private MeshRenderer _meshRenderer;

        private void OnEnable()
        {
            _isActive = true;

            _meshRenderer ??= shieldModel.GetComponent<MeshRenderer>();
        }

        private IEnumerator ShieldBlink(float duration)
        {
            float timer = 0;
            float startTime = Time.time;
            while (timer < duration)
            {
                timer = Time.time - startTime;
                _meshRenderer.enabled = !_meshRenderer.enabled;
                float blinkDuration = blinkCurve.Evaluate(Mathf.Lerp(0, 1, timer / duration));
                yield return new WaitForSeconds(blinkDuration);
            }

            _meshRenderer.enabled = true;
        }

        public void SetIsActivating(float activationDuration)
        {
            StartCoroutine(ShieldBlink(activationDuration));
        }

        public void SetActiveMaterial()
        {
            if (!_meshRenderer) return;
            _meshRenderer.material = activeMaterial;
        }

        public void SetActive(bool isActive)
        {
            _isActive = isActive;

            if (!_isActive) _meshRenderer.enabled = false;
        }

        public bool TryDestroyShield(int parryDamage)
        {
            _healthPoints.TryTakeDamage(parryDamage);

            if (_healthPoints.IsDead())
            {
                _healthPoints.SetCanTakeDamage(false);
                return true;
            }

            return false;
        }

        public void ResetShield()
        {
            _healthPoints.SetCanTakeDamage(true);
            _healthPoints.ResetHitPoints();
        }
    }
}