using UnityEngine;

public class PlayerMovementScript : MonoBehaviour
{

    [Header("Player Attributes")]
    [SerializeField] public Rigidbody rb;

    [Header("Player movement variables")]
    [SerializeField] public float move_speed = 7f;
    [SerializeField] public float movement_multiplier = 10f;
    public Transform orientation;
    private float verticalInput;
    private float horizontalInput;
    private Vector3 moveDirection;
    
    [Header("Ground logic")]
    [SerializeField] public float distToGround = 2f;
    [SerializeField] public LayerMask groundLayer;
    [SerializeField] public bool isOnGround;
    [SerializeField] public float playerGroundDrag = 17f;
    private bool player_is_moving = false;
    RaycastHit hit;

    private GroundSoundsController soundsController;


    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        soundsController = GetComponent<GroundSoundsController>();
    }

    // Update is called once per frame
    void Update()
    {
        CheckIsOnGround();
        GetPlayerInput();
        SpeedControl();
        if (isOnGround)
        {
            ApplyGroundDrag(true);
        }
        else
        {
            ApplyGroundDrag(false);
        }
        
        CheckIsMoving();
    }

    void FixedUpdate()
    {
        if (isOnGround)
        {
           MovePlayer();
        }
    }

    private void GetPlayerInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");
    }

    private void ApplyGroundDrag(bool isGrounded)
    {
        if (isGrounded)
        {
            rb.drag = playerGroundDrag;
        }
        else
        {
            rb.drag = 0f;
        }
    }

    private void MovePlayer()
    {
        // Normalized makes it so that it isn't bad on diagonals
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;
        rb.AddForce(moveDirection.normalized * move_speed * movement_multiplier, ForceMode.Force); 
    }

    private void CheckIsOnGround()
    {
        if (Physics.Raycast(transform.position, Vector3.down, out hit, distToGround + 0.5f, groundLayer))
        {
            isOnGround = true;
        }
        else
        {
            isOnGround = false;
        }
    }

    private void CheckIsMoving()
    {
        if (horizontalInput > 0f || horizontalInput < 0f || verticalInput > 0f || verticalInput < 0f)
        {
            player_is_moving = true;
        }
        else player_is_moving = false;
    }

    private void SpeedControl()
    {
        Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        if(flatVel.magnitude > move_speed)
        {
            Vector3 limitedVel = flatVel.normalized * move_speed;
            rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
        }
    }

}
