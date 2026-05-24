using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public float rotationSpeed = 10f;

    [Header("References")]
    public DynamicJoystick joystick; // Assuming using Joystick Pack for mobile
    private CharacterController controller;
    private Animator animator;

    private Vector3 moveDirection;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        HandleMovement();
    }

    void HandleMovement()
    {
        float horizontal = joystick != null ? joystick.Horizontal : Input.GetAxis("Horizontal");
        float vertical = joystick != null ? joystick.Vertical : Input.GetAxis("Vertical");

        moveDirection = new Vector3(horizontal, 0, vertical).normalized;

        if (moveDirection.magnitude >= 0.1f)
        {
            // Rotate towards movement direction
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

            // Move
            controller.Move(moveDirection * moveSpeed * Time.deltaTime);
            
            if (animator != null)
                animator.SetBool("isRunning", true);
        }
        else
        {
            if (animator != null)
                animator.SetBool("isRunning", false);
        }
    }

    // Power Trigger Methods
    public void UseBubbleTrap()
    {
        Debug.Log("Lumi used Bubble Trap!");
        // Instantiate bubble trap logic here
    }

    public void UseRainbowBlast()
    {
        Debug.Log("Lumi used Rainbow Blast!");
    }
}
