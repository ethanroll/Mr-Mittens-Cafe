using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public static PlayerMovement Instance;

    public bool canMove = true;
    [SerializeField] private int speed = 5; // can be overwritten in unity inspector but not saved if change in play mode
    private Vector2 movement;
    private Rigidbody2D rb;

    private void Awake()
    {
        Instance = this;
        rb = GetComponent<Rigidbody2D>();
    }

    public void OnMovement(InputAction.CallbackContext context) // movement logic
    {
        movement = context.ReadValue<Vector2>();
    }

    private void FixedUpdate()
    {
        if(canMove)
            rb.MovePosition(rb.position + movement * speed * Time.fixedDeltaTime);
    } 
}

