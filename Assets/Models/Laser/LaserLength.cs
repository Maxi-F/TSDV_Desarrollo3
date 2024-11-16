using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserLength : MonoBehaviour
{
    public float laserLength = 0;
    public float laserWidth = 2;
    [SerializeField] LineRenderer beamLineRenderer;
    [SerializeField] ParticleSystem startParticleSystem;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        UpdateBeamLength();
        UpdateBeamWidth();
        UpdateStartParticleEmission();
    }
    void UpdateBeamWidth()
    {
        beamLineRenderer.widthMultiplier = laserWidth;
    }
    void UpdateBeamLength() 
    {
        beamLineRenderer.SetPosition(1, new Vector3(0, 0, laserLength));
    }
    void UpdateStartParticleEmission()
    {
        var emission = startParticleSystem.emission;
        emission.rateOverTime = Mathf.Lerp(0, 60, Mathf.Clamp(laserLength, 0, 1));
    }
}
