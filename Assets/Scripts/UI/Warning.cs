using System;
using System.Collections;
using System.Timers;
using Events.ScriptableObjects;
using TMPro;
using UnityEngine;

namespace UI
{
    public class Warning : MonoBehaviour
    {
        [SerializeField] private EventChannelSO<WarningTextSO> onWarning;
        [SerializeField] private GameObject warningTexts;
        [SerializeField] private AnimationCurve fadeIn;
        [SerializeField] private AnimationCurve fadeOut;
        [SerializeField] private float fadeDuration;
        
        private Coroutine _warningCoroutine;

        private void OnEnable()
        {
            onWarning?.onTypedEvent.AddListener(HandleWarning);
        }

        private void OnDisable()
        {
            onWarning?.onTypedEvent.RemoveListener(HandleWarning);
        }

        private void HandleWarning(WarningTextSO warningData)
        {
            if(_warningCoroutine != null)
                StopCoroutine(_warningCoroutine);

            Debug.Log("Hi?");
            _warningCoroutine = StartCoroutine(HandleWarningCoroutine(warningData));
        }

        private IEnumerator HandleWarningCoroutine(WarningTextSO warningData)
        {
            float startTime = Time.time;
            float timer = 0;
            bool isEnabled = true;
            while (timer < warningData.duration)
            {
                timer = Time.time - startTime;
                warningTexts.SetActive(isEnabled);
                yield return new WaitForSeconds(warningData.intervalSeconds);
                isEnabled = !isEnabled;
            }
            
            warningTexts.SetActive(false);
        }
    }
}
