using UnityEngine;

namespace VFX.AfterImage
{
    [CreateAssetMenu(fileName = "PlayerAfterImageConfig", menuName = "VFX/PlayerAfterImage", order = 0)]
    public class AfterImageSO : ScriptableObject
    {
        public GameObject defaultMesh;
        public Material defaultMaterial;
    }
}