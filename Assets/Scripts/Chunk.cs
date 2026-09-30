using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

public class Chunk : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] GameObject fencePrefab;
    [SerializeField] GameObject applePrefab;
    [SerializeField] GameObject coinPrefab;

    [SerializeField] float coinSeparationLength = 2f;

    [SerializeField] float appleChance = .3f;
    [SerializeField] float coinChance = .5f;

    [SerializeField] float[] lanes = { -2.5f, 0f, 2.5f };

    List<int> availableLanes = new List<int> { 0, 1, 2 };
    private void Start()
    {
        spawnFences();
        spawnApple();
        SpawnCoin();
    }

    void spawnFences()
    {
        int fenceToSpawn = Random.Range(0, 3);
        for (int i = 0; i < fenceToSpawn; i++)
        {
            if (availableLanes.Count <= 0) break;
            int selectedLane = SelectLane();

            Vector3 spawnPosition = new Vector3(lanes[selectedLane], transform.position.y, transform.position.z);
            Instantiate(fencePrefab, spawnPosition, Quaternion.identity, this.transform);
        }
    }

    void spawnApple()
    {
        if (Random.value > appleChance ||  availableLanes.Count <= 0) return;
        int selectedLane = SelectLane();

        Vector3 spawnPosition = new Vector3(lanes[selectedLane], transform.position.y, transform.position.z);
        Instantiate(applePrefab, spawnPosition, Quaternion.identity, this.transform);
    }

    void SpawnCoin()
    {
        if (Random.value > coinChance || availableLanes.Count <= 0) return;
        int selectedLane = SelectLane();

        int maxCoinRange = 6;
        int coinsToSpawn = Random.Range(1,maxCoinRange);

        float topOfChunk = transform.position.z + (coinSeparationLength * 2f);
        for(int i = 0; i < coinsToSpawn;i++)
        {
            float spawnPositionZ = topOfChunk - (coinSeparationLength * i);
            Vector3 spawnPosition = new Vector3(lanes[selectedLane], transform.position.y, spawnPositionZ);
            Instantiate(coinPrefab, spawnPosition, Quaternion.identity, this.transform);
        }
    }

    int SelectLane()
    {
        int randomlaneIdx = Random.Range(0, availableLanes.Count);
        int selectedLane = availableLanes[randomlaneIdx];
        availableLanes.RemoveAt(randomlaneIdx);
        return selectedLane;
    }

}
