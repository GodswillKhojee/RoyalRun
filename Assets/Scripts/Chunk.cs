using UnityEngine;

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
        int randomlaneIdx = Random.Range(0, lanes.Length);
        Vector3 spawnPosition = new Vector3(lanes[randomlaneIdx], transform.position.y, transform.position.z);
        Instantiate(fencePrefab, spawnPosition, Quaternion.identity,this.transform);
    }
}
