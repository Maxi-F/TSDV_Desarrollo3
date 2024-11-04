using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utils;

namespace Attacks.Swing
{
    public class Swing : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private GameObject laserPivot;
        [SerializeField] private GameObject animationPivot;
        [SerializeField] private GameObject laserObject;
        [SerializeField] private List<SwingConfigSO> swingConfigSo = new List<SwingConfigSO>();
        [Header("Anim Config")]
        [SerializeField] private EnemyAnimationHandler animationHandler;

        private float _startingValue;
        private float _finishingValue;
        private bool _shouldRecoverRight;

        private Sequence _swingSequence;
        private Coroutine _swingCoroutine;

        private int _randomIndex;

        [ContextMenu("Swing")]
        private void TestRunSwing()
        {
            SetSequence();
            if (_swingCoroutine != null)
                StopCoroutine(_swingCoroutine);

            _swingCoroutine = StartCoroutine(_swingSequence.Execute());
        }

        public IEnumerator RunSwing()
        {
            SetSequence();
            if (_swingCoroutine != null)
                StopCoroutine(_swingCoroutine);

            yield return _swingSequence.Execute();
        }

        private void SetSequence()
        {
            _swingSequence = new Sequence();
            _swingSequence.AddPreAction(SwingSetup());
            _swingSequence.AddPreAction(LaserGrowCoroutine());
            _swingSequence.SetAction(SwingingCoroutine());
            _swingSequence.AddPostAction(SwingEnd());
        }

        private IEnumerator SwingSetup()
        {
            _randomIndex = Random.Range(0, swingConfigSo.Count);
            laserPivot.SetActive(true);
            _startingValue = swingConfigSo[_randomIndex].startingValue;
            _finishingValue = swingConfigSo[_randomIndex].finishingValue;
            _shouldRecoverRight = swingConfigSo[_randomIndex].shouldRecoverRight;
            animationHandler.SetLaserBlend(_startingValue);

            laserPivot.transform.right = animationPivot.transform.forward;
            laserPivot.transform.position = new Vector3(laserPivot.transform.position.x, animationPivot.transform.position.y, laserPivot.transform.position.z);
            laserObject.transform.localScale = swingConfigSo[_randomIndex].initialLaserScale;
            laserObject.transform.localPosition = swingConfigSo[_randomIndex].initialLaserLocalPosition;
            yield break;
        }

        private IEnumerator LaserGrowCoroutine()
        {
            float timer = 0;
            float startingTime = Time.time;

            while (timer < swingConfigSo[_randomIndex].growDuration)
            {
                timer = Time.time - startingTime;

                float scaleUpValue = Mathf.Lerp(swingConfigSo[_randomIndex].startingSize, swingConfigSo[_randomIndex].finishingSize, swingConfigSo[_randomIndex].laserGrowCurve.Evaluate(timer / swingConfigSo[_randomIndex].growDuration));

                Vector3 scale = new Vector3(scaleUpValue, 1, 1);

                laserPivot.transform.right = animationPivot.transform.forward;
                laserPivot.transform.position = new Vector3(laserPivot.transform.position.x, animationPivot.transform.position.y, laserPivot.transform.position.z);
                laserObject.transform.localPosition = new Vector3(swingConfigSo[_randomIndex].initialLaserLocalPosition.x + scaleUpValue / 2, 0, 0);
                laserObject.transform.localScale = scale;
                yield return null;
            }
        }

        private IEnumerator SwingingCoroutine()
        {
            float timer = 0;
            float startingTime = Time.time;

            while (timer < swingConfigSo[_randomIndex].swingDuration)
            {
                timer = Time.time - startingTime;
                float swingValue = swingConfigSo[_randomIndex].swingCurve.Evaluate(timer / swingConfigSo[_randomIndex].swingDuration);
                animationHandler.SetLaserBlend(Mathf.Lerp(_startingValue, _finishingValue, swingValue));
                laserPivot.transform.right = animationPivot.transform.forward;
                laserPivot.transform.position = new Vector3(laserPivot.transform.position.x, animationPivot.transform.position.y, laserPivot.transform.position.z);
                yield return null;
            }
        }

        private IEnumerator SwingEnd()
        {
            laserPivot.SetActive(false);
            animationHandler.StartRecoverAnimation(_shouldRecoverRight);
            yield break;
        }
    }
}