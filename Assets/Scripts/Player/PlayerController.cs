using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float moveSpeed = 12f;
    Rigidbody rigidBody;
    Vector2 movement;

    [SerializeField] float xClamp = 3f;
    [SerializeField] float zClamp = 3f;


    private void Awake()
    {
        rigidBody = GetComponent<Rigidbody>(); // as the script is already attach to the player game object
        // we are getting the component rigidBody from it
    }
    public void Move(InputAction.CallbackContext context)
    {
        movement = context.ReadValue<Vector2>();
        Debug.Log(movement);
    }

    private void FixedUpdate()
    {
        HandleMovement();
    }
    void HandleMovement()
    {
        Vector3 currentPosition = rigidBody.position;
        Vector3 movePosition = new Vector3(movement.x, 0f, movement.y);
        Vector3 newPosition = currentPosition + movePosition * moveSpeed * Time.fixedDeltaTime;

        newPosition.x = Mathf.Clamp(newPosition.x, -xClamp, xClamp);
        newPosition.z = Mathf.Clamp(newPosition.z, -zClamp, zClamp);

        rigidBody.MovePosition(newPosition);
    }
}
