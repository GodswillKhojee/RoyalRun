using UnityEngine;

public abstract class PickUps : MonoBehaviour
{
    const string playerString = "Player";
    [SerializeField] float rotationSpeed = 100f;

    private void Update()
    {
        transform.Rotate(0, rotationSpeed * Time.deltaTime,0);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag(playerString))
        {
            PickUp();
            Destroy(gameObject);
        }
    }

    protected abstract void PickUp();
}
