using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    [SerializeField] GameObject chunckPrefab;
    [SerializeField] int startingChunksAmount = 12;
    [SerializeField] Transform chunkParent;

    [SerializeField] float chunkLength = 10f;
    void Start()
    {
        for (int i = 0; i < startingChunksAmount; i++)
        {
            float spawnPositionZ;
            if (i == 0) spawnPositionZ = transform.position.z ;
            else spawnPositionZ = transform.position.y  + (i*chunkLength);

            Vector3 chunkSpawnPos = new Vector3(transform.position.x, transform.position.y, spawnPositionZ);
            Instantiate(chunckPrefab,chunkSpawnPos, Quaternion.identity, chunkParent);
        }
    }
}
