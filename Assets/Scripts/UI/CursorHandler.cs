using System;
using Events;
using Events.ScriptableObjects;
using UnityEngine;

namespace UI
{
    public class CursorHandler : MonoBehaviour
    {
        [SerializeField] private VoidEventChannelSO onGamepadUsed;
        [SerializeField] private VoidEventChannelSO onKeyboardUsed;
        
        private void OnEnable()
        {
            onGamepadUsed?.onEvent.AddListener(HandleGamepadUsed);
            onKeyboardUsed?.onEvent.AddListener(HandleKeyboardUsed);
        }

        private void OnDisable()
        {
            onGamepadUsed?.onEvent.RemoveListener(HandleGamepadUsed);
            onKeyboardUsed?.onEvent.RemoveListener(HandleKeyboardUsed);
        }

        private void HandleGamepadUsed()
        {
            Cursor.visible = false;
        }

        private void HandleKeyboardUsed()
        {
            Cursor.visible = true;
        }
    }
}
