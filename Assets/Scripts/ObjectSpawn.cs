using UnityEngine;

public class ObjectSpawn : MonoBehaviour
{
    [SerializeField] GameObject objectPrefab;
    int obstacleSpawnes= 4;

    private void Start()
    {
        while (obstacleSpawnes > 0)
        {
            Instantiate(objectPrefab, transform.position, Quaternion.identity);
            obstacleSpawnes--;
        }
    }
}
