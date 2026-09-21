using UnityEngine;
using System.Collections;

public class ObjectSpawn : MonoBehaviour
{
    [SerializeField] GameObject objectPrefab;
    [SerializeField] float obstacleSpawnTime = 1f;

    private void Start()
    {
        StartCoroutine(spawnObstacleRoutine());
    }

    IEnumerator spawnObstacleRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(obstacleSpawnTime);
            Instantiate(objectPrefab, transform.position, Random.rotation);
        }
    }
}
