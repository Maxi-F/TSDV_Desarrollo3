using System;
using UnityEngine;

namespace Camera
{
    [CreateAssetMenu(menuName = "Camera data", fileName = "CameraConfig", order = 0)]
    public class CameraSO : ScriptableObject
    {
        public Vector3 eulerRotation;
        public AnimationCurve curve;
        public float timeToRotate;
        public Quaternion Rotation { get; private set; }

        private void OnValidate()
        {
            Rotation = Quaternion.Euler(eulerRotation);
        }
    }
}
