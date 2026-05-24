using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public float rotationSpeed = 720f;

    [Header("References")]
    public VirtualJoystick joystick; 
    private Rigidbody2D rb;
    private Animator animator;

    private Vector2 moveInput;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        
        // Ensure gravity doesn't affect the player in a top-down view
        rb.gravityScale = 0f;
    }

    void Update()
    {
        HandleInput();
    }

    void FixedUpdate()
    {
        Move();
    }

    void HandleInput()
    {
        float horizontal = joystick != null ? joystick.Horizontal : Input.GetAxisRaw("Horizontal");
        float vertical = joystick != null ? joystick.Vertical : Input.GetAxisRaw("Vertical");

        moveInput = new Vector2(horizontal, vertical);
        
        if (moveInput.magnitude > 1f)
            moveInput.Normalize();

        if (animator != null)
            animator.SetBool("isRunning", moveInput.magnitude > 0.1f);
    }

    void Move()
    {
        rb.linearVelocity = moveInput * moveSpeed;

        if (moveInput.magnitude > 0.1f)
        {
            // Rotate towards movement direction in 2D
            float targetAngle = Mathf.Atan2(moveInput.y, moveInput.x) * Mathf.Rad2Deg - 90f;
            Quaternion targetRotation = Quaternion.Euler(0, 0, targetAngle);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.fixedDeltaTime);
        }
    }

    public void UseBubbleTrap()
    {
        Debug.Log("Lumi used Bubble Trap!");
    }
}
