using ObjectPool.Runtime;
using UnityEngine;

namespace VFX.AfterImage
{
    public class AfterImageFactory : IFactory<AfterImageSO>
    {
        AfterImageSO config;

        public void SetConfig(AfterImageSO config)
        {
            this.config = config;
        }

        public GameObject CreateObject()
        {
            GameObject afterImageInstance = GameObject.Instantiate(config.defaultMesh);
            foreach (SkinnedMeshRenderer meshRenderer in afterImageInstance.GetComponentsInChildren<SkinnedMeshRenderer>())
            {
                meshRenderer.material = config.defaultMaterial;
            }
            return afterImageInstance;
        }
    }
}