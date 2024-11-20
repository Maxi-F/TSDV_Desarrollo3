using System;
using System.Collections;
using UnityEngine;

namespace UI.Gameplay
{
    public class FallingTarget : MonoBehaviour
    {
        [SerializeField] private float tiltingSeconds;
        [SerializeField] private SpriteRenderer spriteRenderer;
        
        private Coroutine _tiltingCoroutine;

        private bool _isTilting;
        private void OnEnable()
        {
            _isTilting = true;
            
            if(_tiltingCoroutine != null)
                StopCoroutine(_tiltingCoroutine);

            _tiltingCoroutine = StartCoroutine(Tilting());
        }

        private void OnDisable()
        {
            _isTilting = false;
            
            if(_tiltingCoroutine != null)
                StopCoroutine(_tiltingCoroutine);
        }

        private IEnumerator Tilting()
        {
            bool isOn = true;
            
            while (_isTilting)
            {
                var color = spriteRenderer.color;
                color.a = isOn ? 200 : 0;
                spriteRenderer.color = color;

                yield return new WaitForSeconds(tiltingSeconds);
                isOn = !isOn;
            }
        }
    }
}
