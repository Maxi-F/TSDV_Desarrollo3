using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "SwingConfig", menuName = "Enemy/Attacks/SwingConfig", order = 0)]
public class SwingConfigSO : ScriptableObject
{
    [Header("Laser Grow Configuration")]
    public float growDuration = 1.4f;
    public AnimationCurve laserGrowCurve;
    public float startingSize;
    public float finishingSize;
    public Vector3 initialLaserLocalPosition;
    public Vector3 initialLaserScale;

    [Header("Swing Configuration")]
    public float swingDuration = 1.4f;
    public AnimationCurve swingCurve;

    [Range(0.0f, 1.0f)]
    public float startingValue;
    [Range(0.0f, 1.0f)]
    public float finishingValue;
    
    public bool shouldRecoverRight;
}