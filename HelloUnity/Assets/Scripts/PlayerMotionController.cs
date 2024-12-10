using UnityEngine;

public class PlayerMotionController : MonoBehaviour
{
    public bool isGrounded;           // Tracks if the player is grounded
    public float groundCheckDistance;
    public LayerMask groundMask;
    public float gravity = -9.81f;   // Gravity value for downward force

    public float walkSpeed = 10.0f;         // Walking speed
    public float runSpeed = 20.0f;         // Running speed
    public float turnSpeed = 200f;         // Rotation speed in degrees per second
    public float moveSpeed = 0.0f;         // Current movement speed
    public float jumpHeight;
    public float mouseSense; 
    public Animator animator;              // Animator for controlling animations

    private CharacterController controller; // Reference to the CharacterController
    private Vector3 moveDirection;
    private Vector3 velocity;               // keep track of gravity and jumping
    

    void Start()
    {
        // Get or add a CharacterController component
        controller = GetComponent<CharacterController>();
        if (controller == null)
        {
            controller = gameObject.AddComponent<CharacterController>();
        }

        // Ensure Animator is assigned
        if (animator == null)
        {
            animator = GetComponent<Animator>();
        }
    }

    void Update()
    {
        Move();
        Rotate();
    }

    private void Move(){
        isGrounded = Physics.CheckSphere(transform.position, groundCheckDistance, groundMask);

        // stop applying gravity when grounded
        if(isGrounded && velocity.y < 0){
            velocity.y = -2f;
        }

        // W and S movements
        float moveZ = Input.GetAxis("Vertical");
        moveDirection = new Vector3(0, 0, moveZ);
        moveDirection = transform.TransformDirection(moveDirection); // use player forward

        if(isGrounded){
            if(moveDirection != Vector3.zero && !Input.GetKey(KeyCode.LeftShift)){
                // walk
                moveSpeed = walkSpeed;
                animator.SetFloat("speed", 0.5f);
                animator.SetBool("isGrounded", isGrounded);
            }
            else if (moveDirection != Vector3.zero && Input.GetKey(KeyCode.LeftShift)){
                // run
                moveSpeed = runSpeed;
                animator.SetFloat("speed", 1);
                animator.SetBool("isGrounded", isGrounded);
            }
            else if (moveDirection == Vector3.zero){
                // idle
                moveSpeed = 0;
                animator.SetFloat("speed", 0);
                animator.SetBool("isGrounded", isGrounded);
            }

            // apply movement
            moveDirection *= moveSpeed;
            
            if(Input.GetKeyDown(KeyCode.Space)){
                velocity.y = Mathf.Sqrt(jumpHeight * -2 * gravity);
                animator.SetTrigger("jump");
            }
        }

        controller.Move(moveDirection * Time.deltaTime);

        // apply gravity
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

    }

    private void Rotate(){
        float mouseX = Input.GetAxis("Mouse X") * mouseSense * Time.deltaTime;
        transform.Rotate(Vector3.up, mouseX);
    }

}