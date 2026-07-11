using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    [SerializeField] GameObject chunckPrefab;

    void Start()
    {
        Instantiate(chunckPrefab, transform.position, Quaternion.identity);
    }
}
