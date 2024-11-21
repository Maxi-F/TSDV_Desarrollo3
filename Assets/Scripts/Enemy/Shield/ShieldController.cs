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
        [FormerlySerializedAs("_healthPoints")] [SerializeField] private HealthPoints healthPoints;

        [SerializeField] private Material activeMaterial;
        [SerializeField] private AnimationCurve blinkCurve;
        [SerializeField] private GameObject shieldModel;
        private MeshRenderer _meshRenderer;

        private void OnEnable()
        {
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
            _meshRenderer.enabled = isActive;
        }

        public bool TryDestroyShield(int parryDamage)
        {
            healthPoints.TryTakeDamage(parryDamage);

            if (healthPoints.IsDead())
            {
                healthPoints.SetCanTakeDamage(false);
                return true;
            }

            return false;
        }

        public void ResetShield()
        {
            healthPoints.SetCanTakeDamage(true);
            healthPoints.ResetHitPoints();
        }
    }
}