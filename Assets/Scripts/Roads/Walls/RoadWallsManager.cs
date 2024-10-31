using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace Roads.Walls
{
    public enum WallTypes
    {
        Corner,
        Middle
    }

    public class RoadWallsManager : MonoBehaviour
    {
        public static RoadWallsManager Instance;

        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(this.gameObject);
                return;
            }

            Instance = this;
        }

        public List<GameObject> GetRandomWalls(WallTypes wallType)
        {
            List<GameObject> wallsToReturn = new List<GameObject>();
            GameObject wall = null;
            Wall_ID auxWallId = null;
            switch (wallType)
            {
                case WallTypes.Corner:
                    wall = CornerWallsObjectPool.Instance.GetRandomPooledObject();
                    auxWallId = wall.GetComponent<Wall_ID>();
                    wall.SetActive(true);
                    wallsToReturn.Add(wall);
                    do
                    {
                        wall = CornerWallsObjectPool.Instance.GetRandomPooledObject();
                    } while (auxWallId.wallName == wall.GetComponent<Wall_ID>().wallName);

                    wallsToReturn.Add(wall);
                    break;
                case WallTypes.Middle:
                    wall = MiddleWallsObjectPool.Instance.GetRandomPooledObject();
                    auxWallId = wall.GetComponent<Wall_ID>();
                    wall.SetActive(true);
                    wallsToReturn.Add(wall);
                    do
                    {
                        wall = MiddleWallsObjectPool.Instance.GetRandomPooledObject();
                    } while (auxWallId.wallName == wall.GetComponent<Wall_ID>().wallName);

                    wallsToReturn.Add(wall);
                    break;
            }

            return wallsToReturn;
        }
    }
}