using UnityEngine;

namespace Roads.Walls.ScriptableObjects
{
   [CreateAssetMenu(fileName = "Wall", menuName = "Roads/Walls/Wall", order = 0)]
   public class WallSO : ScriptableObject
   {
      public GameObject wallObject;
   }
}
