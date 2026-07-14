using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] Rigidbody playerRb;
    Vector2 movement;
    public void Move(InputAction.CallbackContext context)
    {
        movement = context.ReadValue<Vector2>();
        Debug.Log(movement);
    }
}
