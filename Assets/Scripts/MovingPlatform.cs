using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public enum MovementType
    {
        Horizontal,
        Vertical
    }

    public enum StartDirection
    {
        Right,  
        Left    
    }

    public MovementType movementType = MovementType.Horizontal;
    public StartDirection startDirection = StartDirection.Right;
    public float moveSpeed = 2f;
    public float moveDistance = 3f;
    public float playerVelocityMultiplier = 0.3f;

    private Vector3 startPosition;
    private float movementProgress = 0f;
    private Rigidbody rb;
    private Vector3 lastPosition;
    private bool isReturning = false;

    void Start()
    {
        startPosition = transform.position;
        lastPosition = startPosition;
        rb = GetComponent<Rigidbody>();
        
        if (rb == null)
        {
            rb = gameObject.AddComponent<Rigidbody>();
        }
        rb.useGravity = false;
        rb.isKinematic = true;
        rb.interpolation = RigidbodyInterpolation.Interpolate;

        if (startDirection == StartDirection.Left)
        {
            movementProgress = moveDistance;
            isReturning = true;
        }
    }

    void FixedUpdate()
    {
        lastPosition = transform.position;

        if (!isReturning)
        {
            movementProgress += Time.fixedDeltaTime * moveSpeed;
            if (movementProgress >= moveDistance)
            {
                movementProgress = moveDistance;
                isReturning = true;
            }
        }
        else
        {
            movementProgress -= Time.fixedDeltaTime * moveSpeed;
            if (movementProgress <= 0)
            {
                movementProgress = 0;
                isReturning = false;
            }
        }

        Vector3 targetPosition = startPosition + GetTargetOffset();
        rb.MovePosition(targetPosition);
    }

    private Vector3 GetTargetOffset()
    {
        float currentOffset = movementProgress;
        
        if (startDirection == StartDirection.Left)
        {
            currentOffset = moveDistance - currentOffset;
        }

        return movementType == MovementType.Horizontal 
            ? new Vector3(currentOffset, 0, 0) 
            : new Vector3(0, currentOffset, 0);
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Rigidbody playerRb = collision.gameObject.GetComponent<Rigidbody>();
            if (playerRb != null)
            {
                Vector3 platformVelocity = (transform.position - lastPosition) / Time.fixedDeltaTime;

                if (movementType == MovementType.Horizontal)
                {
                    playerRb.linearVelocity = new Vector3(
                        platformVelocity.x,
                        playerRb.linearVelocity.y,
                        playerRb.linearVelocity.z
                    );
                }
                else 
                {
                    if (platformVelocity.y > 0)
                    {
                        playerRb.linearVelocity = new Vector3(
                            playerRb.linearVelocity.x,
                            platformVelocity.y,
                            playerRb.linearVelocity.z
                        );
                    }
                }
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Rigidbody playerRb = collision.gameObject.GetComponent<Rigidbody>();
            if (playerRb != null)
            {
                playerRb.interpolation = RigidbodyInterpolation.Interpolate;
            }
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Rigidbody playerRb = collision.gameObject.GetComponent<Rigidbody>();
            if (playerRb != null)
            {
                playerRb.interpolation = RigidbodyInterpolation.None;
            }
        }
    }
} 