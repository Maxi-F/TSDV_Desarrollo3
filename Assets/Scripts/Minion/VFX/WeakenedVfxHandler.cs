using UnityEngine;

public class WeakenedVfxHandler : MonoBehaviour
{
    public void ActivateParticles()
    {
        if (!gameObject.activeInHierarchy)
            gameObject.SetActive(true);
    }
}