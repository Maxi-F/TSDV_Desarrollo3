using System;
using Events.ScriptableObjects;
using Input;
using UnityEngine;

namespace Player
{
    public class PlayerYMovementStateRaiser : MonoBehaviour
    {
        [SerializeField] private EventChannelSO<YMovementState> onMovementStateChange;
        [SerializeField] private InputHandlerSO inputHandler;
        
        private YMovementState _yMovementState;

        private void OnEnable()
        {
            inputHandler?.onPlayerMove.AddListener(HandleMovement);
        }

        private void OnDisable()
        {
            inputHandler?.onPlayerMove.RemoveListener(HandleMovement);
        }

        private void HandleMovement(Vector2 movement)
        {
            if (movement.y < 0)
            {
                if(_yMovementState != YMovementState.Downwards)
                    onMovementStateChange?.RaiseEvent(YMovementState.Downwards);
                _yMovementState = YMovementState.Downwards;
            } else if (movement.y > 0)
            {
                if(_yMovementState != YMovementState.Upwards)
                    onMovementStateChange?.RaiseEvent(YMovementState.Upwards);
                _yMovementState = YMovementState.Upwards;
            }
            else
            {
                _yMovementState = YMovementState.Normal;
                onMovementStateChange?.RaiseEvent(_yMovementState);
            }
        }
    }
}
