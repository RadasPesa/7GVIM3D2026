using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class PlayerMovement : MonoBehaviour
{
    private PlayerInput playerInput;
    private InputAction movementAction;
    private Vector2 inputDirection;

    [SerializeField] private float playerSpeed = 5f;
    [SerializeField] private float jumpForce = 5f;
    private Vector3 moveDirection; // x = left/right, z = forward/backward, y = jump/gravity
    private Rigidbody rb;
    private Vector3 horizontalVelocity;

    private CharacterController charController;
    private Vector2 currentInputDirection;
    private Vector2 smoothInputVelocity;
    [SerializeField] private float smoothInputSpeed = 0.2f; // time to reach maximum

    private bool jump = false;

    private Transform cameraTransform;
    [SerializeField] private Transform head;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        movementAction = playerInput.actions["Movement"];
        rb = GetComponent<Rigidbody>();
        
        charController = GetComponent<CharacterController>();
        cameraTransform = Camera.main.transform;
    }

    // Update is called once per frame
    void Update()
    {
        inputDirection = movementAction.ReadValue<Vector2>();
        currentInputDirection = Vector2.SmoothDamp(currentInputDirection, inputDirection,
            ref smoothInputVelocity, smoothInputSpeed);
        moveDirection = new Vector3(currentInputDirection.x, 0f, currentInputDirection.y);
        moveDirection = cameraTransform.forward * moveDirection.z + cameraTransform.right * moveDirection.x;
        moveDirection.y = 0f;

        MovePlayerCC();
    }

    private void MovePlayer()
    {
        Vector3 moveVector = transform.TransformDirection(moveDirection) * playerSpeed;
        rb.linearVelocity = new Vector3(moveVector.x, rb.linearVelocity.y, moveVector.z);
    }

    private void MovePlayerCC()
    {
        if (charController.isGrounded)
        {
            horizontalVelocity.y = -1f;

            if (jump)
            {
                horizontalVelocity.y = jumpForce;
                jump = false;
            }
        }
        else
        {
            horizontalVelocity.y += Physics.gravity.y * 2f * Time.deltaTime;
        }
        charController.Move(moveDirection * (playerSpeed * Time.deltaTime));
        charController.Move(horizontalVelocity * Time.deltaTime);
        
        //var eulerAngles = cameraTransform.eulerAngles;
        //transform.rotation = Quaternion.Euler(0f, eulerAngles.y, 0f);
        //head.rotation = Quaternion.Euler(eulerAngles.x, eulerAngles.y, 0f);
    }

    public void Jump(InputAction.CallbackContext ctx)
    {
        if (!ctx.performed)
            return;
        if (charController.isGrounded)
        {
            jump = true;
        }
    }
}
