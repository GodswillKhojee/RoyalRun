using UnityEngine;

public class ObjectDestroyer : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("destroy it: "+ other.gameObject.name);
        Destroy(other.gameObject);
    }
}
