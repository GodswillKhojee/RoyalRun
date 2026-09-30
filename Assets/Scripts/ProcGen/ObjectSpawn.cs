using UnityEngine;
using System.Collections;

public class ObjectSpawn : MonoBehaviour
{
    [SerializeField] GameObject[] objectPrefab;
    [SerializeField] float obstacleSpawnTime = 1f;
    [SerializeField] Transform obstacleParent;
    [SerializeField] float spawnWidth = 4f;

    private void Start()
    {
        StartCoroutine(spawnObstacleRoutine());
    }

    IEnumerator spawnObstacleRoutine()
    {
        while (true)
        {
            GameObject noOfObject = objectPrefab[Random.Range(0, objectPrefab.Length)];
            Vector3 spawnPosition = new Vector3(Random.Range(-spawnWidth, spawnWidth), transform.position.y, transform.position.z);
            yield return new WaitForSeconds(obstacleSpawnTime);
            Instantiate(noOfObject, spawnPosition, Random.rotation,obstacleParent);
        }
    }
}
