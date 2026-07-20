using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float moveSpeed = 12f;
    Rigidbody rigidBody;
    Vector2 movement;

    private void Awake()
    {
        rigidBody = GetComponent<Rigidbody>(); // as the script is already attach to the player game object
        // we are getting the component rigidBody from it
    }
    private void FixedUpdate()
    {
        HandleMovement();
    }
    public void Move(InputAction.CallbackContext context)
    {
        movement = context.ReadValue<Vector2>();
        Debug.Log(movement);
    }

    void HandleMovement()
    {
        Vector3 currentPosition = rigidBody.position;
        Vector3 movePosition = new Vector3(movement.x, 0f, movement.y);
        Vector3 newPosition = currentPosition + movePosition * moveSpeed * Time.fixedDeltaTime;
        rigidBody.MovePosition(newPosition);
    }
}
