using UnityEngine;

public class PlayerMotionController : MonoBehaviour
{
    public Animator animator; 
    public float walkSpeed = 5.0f;
    public float runSpeed = 10.0f;
    public float gravity = -9.81f; 
    public float gravityMultiplier = 2.0f; 

    private CharacterController characterController; 
    private Vector3 movement;
    private float verticalVelocity; 

    void Start()
    {
        if (animator == null)
        {
            animator = GetComponent<Animator>();
        }

        characterController = GetComponent<CharacterController>();
    }

    void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        movement = new Vector3(horizontal, 0, vertical).normalized;

        // update animator params
        float speed = movement.magnitude; 
        bool isMoving = speed > 0.1f; 

        animator.SetFloat("speed", speed);
        animator.SetBool("isMoving", isMoving);

        // do gravity if not grounded
        if (!characterController.isGrounded)
        {
            verticalVelocity += gravity * gravityMultiplier * Time.deltaTime;
        }
        else
        {
            verticalVelocity = 0f; // reset
        }

        movement.y = verticalVelocity;
    }

    void FixedUpdate()
    {
        if (movement.magnitude >= 0.1f || verticalVelocity != 0)
        {
            // identify walking or running speed
            float currentSpeed = Input.GetKey(KeyCode.LeftShift) ? runSpeed : walkSpeed;
            Vector3 moveVelocity = movement * currentSpeed;

            // move
            characterController.Move(moveVelocity * Time.fixedDeltaTime);
        }
    }
}
