using System.Collections;
using Events;
using UnityEngine;
using UnityEngine.Serialization;

namespace VFX.AfterImage
{
    public class CustomTrail : MonoBehaviour
    {
        [SerializeField] private float cloneDuration = 1f;
        [SerializeField] private float timeBetweenInstances = 0.1f;

        private bool _shouldSpawnImages;
        private Coroutine _startCoroutine;

        
        public void StartSpawn()
        {
            if (_startCoroutine != null)
            {
                StopCoroutine(_startCoroutine);
            }

            _shouldSpawnImages = true;
            _startCoroutine = StartCoroutine(SpawnAfterImage());
        }

        public void StopSpawn()
        {
            _shouldSpawnImages = false;
        }

        IEnumerator SpawnAfterImage()
        {
            while (_shouldSpawnImages)
            {
                yield return new WaitForSeconds(timeBetweenInstances);

                GameObject afterImage = AfterImagePool.Instance.GetPooledObject();
                afterImage.transform.SetPositionAndRotation(transform.position, transform.rotation);
                afterImage.SetActive(true);
                StartCoroutine(FadeOut(afterImage));
            }
        }

        IEnumerator FadeOut(GameObject afterImage)
        {
            Material material = afterImage.GetComponentInChildren<SkinnedMeshRenderer>().material;
            float elapsedTime = 0f;
            Color initialColor = material.color;
            while (elapsedTime < cloneDuration)
            {
                elapsedTime += Time.deltaTime;
                float alpha = Mathf.Lerp(1f, 0f, elapsedTime / cloneDuration);
                material.color = new Color(initialColor.r, initialColor.g, initialColor.b, alpha);
                yield return null;
            }

            afterImage.SetActive(false);
        }
    }
}