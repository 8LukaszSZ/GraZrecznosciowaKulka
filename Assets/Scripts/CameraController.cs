using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    public Transform target;
    public float rotationSpeed = 5f;
    public Vector3 offset;

    private float currentYaw = 0f;
    private float currentPitch = 15f; 
    private Vector2 lookInput;
    public float minPitch = -40f;
    public float maxPitch = 60f;

    public void OnLook(InputAction.CallbackContext context)
    {
        lookInput = context.ReadValue<Vector2>();
    }

    void LateUpdate()
    {
        if (target == null) return;

        currentYaw += lookInput.x * rotationSpeed;
        currentPitch -= lookInput.y * rotationSpeed; 
        currentPitch = Mathf.Clamp(currentPitch, minPitch, maxPitch);

        Quaternion rotation = Quaternion.Euler(currentPitch, currentYaw, 0);
        Vector3 desiredPosition = target.position + rotation * offset;
        transform.position = desiredPosition;

        transform.LookAt(target.position);
    }

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}
