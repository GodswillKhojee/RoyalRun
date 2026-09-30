using UnityEngine;

public class PickUps : MonoBehaviour
{
    const string playerString = "Player";

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag(playerString))
        {
            Debug.Log("that was player");
        }
    }
}
