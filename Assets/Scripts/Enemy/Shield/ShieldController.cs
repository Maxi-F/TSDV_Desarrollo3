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
        [SerializeField] private MeshRenderer meshRenderer;

        private IEnumerator ShieldBlink(float duration)
        {
            float timer = 0;
            float startTime = Time.time;
            while (timer < duration)
            {
                timer = Time.time - startTime;
                meshRenderer.enabled = !meshRenderer.enabled;
                float blinkDuration = blinkCurve.Evaluate(Mathf.Lerp(0, 1, timer / duration));
                yield return new WaitForSeconds(blinkDuration);
            }

            meshRenderer.enabled = true;
        }

        public void SetIsActivating(float activationDuration)
        {
            StartCoroutine(ShieldBlink(activationDuration));
        }

        public void SetActiveMaterial()
        {
            if (!meshRenderer) return;
            meshRenderer.material = activeMaterial;
        }

        public void SetActive(bool isActive)
        {
            meshRenderer.enabled = isActive;
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