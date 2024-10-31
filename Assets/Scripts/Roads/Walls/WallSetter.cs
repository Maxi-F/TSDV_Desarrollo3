using System;
using System.Collections;
using System.Collections.Generic;
using Roads.Walls;
using Unity.Mathematics;
using UnityEngine;

public class WallSetter : MonoBehaviour
{
    [SerializeField] private GameObject wallRoot;
    [SerializeField] private WallTypes wallType;

    private List<GameObject> currentWalls;

    private void OnEnable()
    {
        StartCoroutine(InitCoroutine());
    }

    private void OnDisable()
    {
        if (currentWalls == null || currentWalls.Count == 0)
            return;

        currentWalls[1].transform.localScale = new Vector3(currentWalls[1].transform.localScale.x * -1, currentWalls[1].transform.localScale.y, currentWalls[1].transform.localScale.z);

        for (int i = 0; i < currentWalls.Count; i++)
        {
            currentWalls[i].transform.parent = null;
            currentWalls[i].SetActive(false);
        }

        currentWalls.Clear();
    }

    private IEnumerator InitCoroutine()
    {
        yield return null;
        currentWalls = RoadWallsManager.Instance.GetRandomWalls(wallType);

        while (currentWalls.Count < 2)
        {
            currentWalls.Clear();
            currentWalls = RoadWallsManager.Instance.GetRandomWalls(wallType);
        }

        for (int i = 0; i < currentWalls.Count; i++)
        {
            currentWalls[i].SetActive(true);
            currentWalls[i].transform.parent = wallRoot.transform;
            currentWalls[i].transform.localRotation = quaternion.identity;
            currentWalls[i].transform.localPosition = Vector3.zero;
        }

        currentWalls[1].transform.localScale = new Vector3(currentWalls[1].transform.localScale.x * -1, currentWalls[1].transform.localScale.y, currentWalls[1].transform.localScale.z);
    }
}