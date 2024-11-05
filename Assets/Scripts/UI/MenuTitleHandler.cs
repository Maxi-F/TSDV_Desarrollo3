using System;
using UnityEngine;

namespace UI
{
    public class MenuTitleHandler : MonoBehaviour
    {
        [SerializeField] private GameObject titleImageContainer;
        private bool _isFirstMenu;

        private void Awake()
        {
            _isFirstMenu = true;
        }

        public void OnEnable()
        {
            titleImageContainer.gameObject.SetActive(!_isFirstMenu);
        }

        public void SetIsFirstMenu(bool value)
        {
            _isFirstMenu = value;
        }
    }
}
