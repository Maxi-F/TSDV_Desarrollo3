using UnityEngine;
using UnityEngine.Serialization;

namespace Enemy
{
    [CreateAssetMenu(fileName = "EnemyConfig", menuName = "Enemy/Config", order = 0)]
    public class EnemyConfigSO : ScriptableObject
    {
        public Vector3 offScreenPosition;
        public Vector3 defaultPosition;
        public Vector3 weakenedPosition;

        [Header("Enemy Incoming Sequence")]
        public float appearingDuration;
        [Header("Enemy Leaving Sequence")]
        public float leavingDuration;
        
        [Header("Movement Properties")]
        public float weakenedStartDelay;
        public float weakenedMoveDuration;
        public float recoverMoveDuration;
        
        [Header("Death Properties")]
        public float deathAnimationDelay;
        public float deathMovementDuration;
        public Vector3 deathTargetPosition;
    }
}