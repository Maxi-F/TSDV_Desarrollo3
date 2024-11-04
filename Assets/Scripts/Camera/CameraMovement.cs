using System;
using System.Collections;
using Events.ScriptableObjects;
using UnityEngine;

namespace Camera
{
    public class CameraMovement : MonoBehaviour
    {
        [SerializeField] private CameraDataChannelSO onCameraMovementEvent;

        private Coroutine movementCoroutine;
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
             if(movementCoroutine != null)
                 StopCoroutine(movementCoroutine);

             movementCoroutine = StartCoroutine(HandleMovementCoroutine(cameraData));
        }

        private IEnumerator HandleMovementCoroutine(CameraSO cameraData)
        {
            float timer = 0;
            float startTime = Time.time;
            Quaternion startRotation = gameObject.transform.rotation;
            
            while (timer < cameraData.timeToRotate)
            {
                timer = Time.time - startTime;

                float rotationTime = cameraData.curve.Evaluate(timer / cameraData.timeToRotate);
                gameObject.transform.rotation = Quaternion.Slerp(startRotation, cameraData.Rotation, rotationTime);
                yield return null;
            }

            gameObject.transform.rotation = cameraData.Rotation;
        }
    }
}
