using System;
using System.Collections;
using Events.ScriptableObjects;
using UnityEngine;

namespace Camera
{
    public class CameraMovement : MonoBehaviour
    {
        [SerializeField] private CameraDataChannelSO onCameraMovementEvent;

        private Coroutine _movementCoroutine;
        private void OnEnable()
        {
            onCameraMovementEvent?.onTypedEvent.AddListener(HandleCameraMovement);
        }

        private void OnDisable()
        {
            onCameraMovementEvent?.onTypedEvent.RemoveListener(HandleCameraMovement);
        }

        private void HandleCameraMovement(CameraSO cameraData)
        {
             if(_movementCoroutine != null)
                 StopCoroutine(_movementCoroutine);

             _movementCoroutine = StartCoroutine(HandleMovementCoroutine(cameraData));
        }

        private IEnumerator HandleMovementCoroutine(CameraSO cameraData)
        {
            float timer = 0;
            float startTime = Time.time;
            Quaternion startRotation = gameObject.transform.rotation;
            Quaternion endRotation = Quaternion.Euler(cameraData.eulerRotation);
            
            while (timer < cameraData.timeToRotate)
            {
                timer = Time.time - startTime;

                float rotationTime = cameraData.curve.Evaluate(timer / cameraData.timeToRotate);
                gameObject.transform.rotation = Quaternion.Slerp(startRotation, endRotation, rotationTime);
                yield return null;
            }

            gameObject.transform.rotation = endRotation;
        }
    }
}
