using System.Security;
using UnityEngine;
using UnityEngine.UIElements;

public class FirstPersonController : MonoBehaviour
{
    public float walkSpeed = 5f;
    public float jumpForce = 5f;
    public float groundCheckDistance = 1.5f;
    public float lookSensitivityX = 1f;
    public float lookSensitivityY = 1f;
    public float minYLookAngle = -90f;
    public float maxYLookAngle = 90f;
    public float gravity = -9.8f;
    public Transform playerCamera;
    private Vector3 velocity;
    private float verticalRotation = 0f;
    private CharacterController characterController;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        UnityEngine.Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        float horizontalMovement = Input.GetAxisRaw("Horizontal");
        float verticalMovement = Input.GetAxisRaw("Vertical");

        Vector3 moveDirection = transform.forward * verticalMovement + transform.right * horizontalMovement;
        moveDirection.Normalize();

        characterController.Move(moveDirection*walkSpeed*Time.deltaTime);
        if (IsGrounded() && velocity.y < 0)
        {
            velocity.y=-2f;
        }
        if (Input.GetKeyDown(KeyCode.Space)&&IsGrounded())
        {
            velocity.y = jumpForce;
        }
        else
        {
            velocity.y+=gravity*Time.deltaTime;
        }
        characterController.Move(velocity*Time.deltaTime);

        if(playerCamera != null)
        {
            float mouseX = Input.GetAxis("Mouse X") * lookSensitivityX;
            float mouseY = Input.GetAxis("Mouse Y") * lookSensitivityY;

            verticalRotation -= mouseY;
            verticalRotation = Mathf.Clamp(verticalRotation, minYLookAngle, maxYLookAngle);

            playerCamera.localRotation = Quaternion.Euler(verticalRotation, 0f, 0f);
            transform.Rotate(Vector3.up * mouseX);
        }
    }

    bool IsGrounded()
    {
        RaycastHit hit;
        if(Physics.Raycast(transform.position, Vector3.down, out hit, groundCheckDistance))
        {
            return true;
        }
        return false;
    }
}
