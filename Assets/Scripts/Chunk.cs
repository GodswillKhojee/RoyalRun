using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

public class Chunk : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] GameObject fencePrefab;

    [SerializeField] float[] lanes = { -2.5f, 0f, 2.5f };

    private void Start()
    {
        spawnFence();
    }

    void spawnFence()
    {
        List<int> availableLanes = new List<int> { 0, 1, 2 };
        int fenceToSpawn = Random.Range(0, 3);
        for (int i = 0; i < fenceToSpawn; i++)
        {
            if (availableLanes.Count <= 0) break;

            int randomlaneIdx = Random.Range(0, availableLanes.Count);
            int selectedLane = availableLanes[randomlaneIdx];
            availableLanes.RemoveAt(randomlaneIdx);

            Vector3 spawnPosition = new Vector3(lanes[selectedLane], transform.position.y, transform.position.z);
            Instantiate(fencePrefab, spawnPosition, Quaternion.identity,this.transform);
        }
    }
}
