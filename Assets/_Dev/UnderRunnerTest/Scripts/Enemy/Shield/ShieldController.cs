using System;
using System.Collections;
using UnityEngine;

namespace _Dev.UnderRunnerTest.Scripts.Enemy.Shield
{
    public class ShieldController : MonoBehaviour
    {
        [SerializeField] private Material activatingShieldMaterial;
        [SerializeField] private Material translucentMaterial;
        [SerializeField] private Material activeMaterial;
        [SerializeField] private float twinkleSeconds = 0.5f;
        [SerializeField] private GameObject shieldModel;
        
        private bool _isActivating = false;
        private bool _isActive = true;
        private bool _isInCoroutine = false;
        private MeshRenderer _meshRenderer;
        
        private void OnEnable()
        {
            _isActivating = false;
            _isActive = true;
            _isInCoroutine = false;
            
            _meshRenderer ??= shieldModel.GetComponent<MeshRenderer>();
        }

        private void Update()
        {
            if (_isActivating && !_isInCoroutine)
            {
                _isInCoroutine = true;
                StartCoroutine(ChangeMaterials());
            }
        }

        private IEnumerator ChangeMaterials()
        {
            bool isTraslucentOn = true;
            while (_isActivating)
            {
                _meshRenderer.material = isTraslucentOn
                    ? activatingShieldMaterial
                    : translucentMaterial;

                isTraslucentOn = !isTraslucentOn;
                
                yield return new WaitForSeconds(twinkleSeconds);
            }

            _isActivating = false;
            _isInCoroutine = false;
        }
        
        public void SetIsActivating(bool isActivating)
        {
            _isActivating = isActivating;
        }

        public void SetActiveMaterial()
        {
            _isActivating = false;
            _meshRenderer.material = activeMaterial;
        }

        public bool IsActive()
        {
            return _isActive;
        }

        public void SetActive(bool isActive)
        {
            _isActive = isActive;

            if (!_isActive) _meshRenderer.material = translucentMaterial;
        }
    }
}
