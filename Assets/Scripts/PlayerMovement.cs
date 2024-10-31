using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    //Components
    private CharacterController characterController;
    private Transform cameraTransform;

    //movement and jumo configuration parameters
    [SerializeField] private float speed = 5f;
    [SerializeField] private float multiplier = 2f;
    [SerializeField] private float jumpForce = 1.5f;
    [SerializeField] private float gravity = Physics.gravity.y;

    //Input fields for movements and look actions
    private Vector2 moveInput;
    private Vector2 lookInput;

    //Velocity and rotation variables
    private Vector2 velocity;
    private float verticalVelocity;
    private float verticalRotation=0;

    //Is Sprinting state
    private bool isSprinting;

    //Camera look sensitivity and max angle to limit vertical rotationç
    [SerializeField] private float lookSensitivity = 1f;
    private float maxLookAngle = 80f; //max vertical


    private void Start()
    {
        characterController = GetComponent<CharacterController>();
        cameraTransform = Camera.main.transform;

        //Hide cursor
        Cursor.lockState = CursorLockMode.Locked;
    }


    private void Update()
    {
        //Manage Player Movement
        MovePlayer();
        //Manage Camera Rotation
        LookAround();
    }

    public void Move(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
        Debug.Log(moveInput);
    }
    

    //Receive look input from the Input System
    public void Look(InputAction.CallbackContext context)
    {
        lookInput = context.ReadValue<Vector2>();
    }

    //Receive jump input from the Input System and triggers jump if grounded
    public void Jump(InputAction.CallbackContext context) 
    {
        //if Player is touching ground
        if (characterController.isGrounded) 
        {
            //Calculate the require velocity for a jump
            velocity.y = Mathf.Sqrt(jumpForce * -2f * gravity);
        }
    }

    //Receive Sprint input from Input System and change isSprinting state
    public void Sprint(InputAction.CallbackContext context)
    {
        //when action started or mantained
        isSprinting = context.started || context.performed;
    }

    
    //Handles player movement and jump based on input and applies gravity
    private void MovePlayer() 
    { 
        //Falling Down
        if (characterController.isGrounded)
        {
            //Restart vertical velocity when touch ground
            verticalVelocity = 0f;
        }
        else
        {
            //when is falling down increment velocity with gravity and time
            verticalVelocity += gravity * Time.deltaTime;
        }

        Vector3 move = new Vector3(0, verticalVelocity, 0);
        characterController.Move(move * Time.deltaTime);

        //Movement
        Vector3 moveDirection = new Vector3(moveInput.x, 0, moveInput.y);
        moveDirection = transform.TransformDirection(moveDirection);
        float targetSpeed = isSprinting ? speed * multiplier : speed;

        characterController.Move(moveDirection * targetSpeed * Time.deltaTime);

        //Apply gravity constantly to posibility Jump
        velocity.y += gravity * Time.deltaTime;
        characterController.Move(velocity * Time.deltaTime);

    }

    //Handles camera rotation based on Look Input
    private void LookAround()
    {
        //Horizontal rotation (Y-axis) based on sensitivity and input
        float horizontalRotation = lookInput.x * lookSensitivity;
        transform.Rotate(Vector3.up * horizontalRotation);

        //Vertical rotation (X-axis) with clamping to prevent over-rotation
        verticalRotation -= lookInput.y * lookSensitivity;
        verticalRotation = Mathf.Clamp(verticalRotation, -maxLookAngle, maxLookAngle);
        cameraTransform.localRotation = Quaternion.Euler(verticalRotation, 0f, 0f);
    }

}
