using UnityEngine;
using UnityEngine.InputSystem;

public class BallController : MonoBehaviour
{
    public float moveSpeed = 8f;
    public float jumpForce = 7f;
    public Transform cameraTransform; 
    private Rigidbody rb;
    private Vector2 moveInput;
    private bool isGrounded;

    [Header("Ground Check")]
    public LayerMask groundLayer; 
    public float groundCheckDistance = 0.6f;
    public Vector3 groundCheckOffset = new Vector3(0, 0, 0);

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (cameraTransform == null)
        {
            cameraTransform = FindObjectOfType<CameraController>().transform;
        }

        rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
        rb.interpolation = RigidbodyInterpolation.Interpolate;
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.performed && isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }

    void Update()
    {
        isGrounded = Physics.Raycast(transform.position + groundCheckOffset, Vector3.down, groundCheckDistance, groundLayer);
    }

    void FixedUpdate()
    {
        Vector3 camForward = cameraTransform.forward;
        Vector3 camRight = cameraTransform.right;
        camForward.y = 0;
        camRight.y = 0;
        camForward.Normalize();
        camRight.Normalize();

        Vector3 movement = (camForward * moveInput.y + camRight * moveInput.x);
        
        if (movement.magnitude > 0.1f)
        {
            Vector3 horizontalVelocity = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z);
            if (horizontalVelocity.magnitude < 10f) 
            {
                rb.AddForce(movement * moveSpeed, ForceMode.Acceleration);
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Platform"))
        {
            FindObjectOfType<GameManager>().OnFirstPlatformTouch(transform.position.y);
        }
    }
}
