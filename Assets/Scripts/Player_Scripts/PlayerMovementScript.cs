using UnityEngine;
using UnityEngine.Audio;

public class PlayerMovementScript : MonoBehaviour
{

    [Header("Player Attributes")]
    [SerializeField] public Rigidbody rb;
    private float playerHeight;

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

    [Header("Sound Control Player")]
    private GroundSoundsController soundsController;

    [Header("Player steps and ledges")]
    [SerializeField] GameObject stepUpper;
    [SerializeField] GameObject stepLower;
    [SerializeField] public float stepHeight = 0.15f;
    [SerializeField] public float stepSmooth = 0.01f;

    [Header("Player slopes")]
    public float maxSlopeAngle;
    private RaycastHit slopeHit;
    


    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        soundsController = GetComponent<GroundSoundsController>();
        this.playerHeight = 0.3f;
    }

    private void Awake()
    {
        stepUpper.transform.position = new Vector3(stepUpper.transform.position.x, stepHeight, stepUpper.transform.position.z);
    }

    // Update is called once per frame
    void Update()
    {
        CheckIsOnGround();
        GetPlayerInput();
        CheckIsMoving();

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
            stepClimb();



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

        //if (IsOnSlope())
        //{
        //    rb.AddForce(GetSlopeMoveDirection() * move_speed * 10f, ForceMode.Force);
        //}
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


    void stepClimb()
    {
        RaycastHit hitLower;
        if (Physics.Raycast(stepLower.transform.position, transform.TransformDirection(Vector3.forward), out hitLower, 0.1f))
        {
            Debug.Log("First");
            RaycastHit hitUpper;
            if (!Physics.Raycast(stepUpper.transform.position, transform.TransformDirection(Vector3.forward), out hitUpper, 0.2f))
            {
                Debug.Log("First");
                rb.position -= new Vector3(0f, -stepSmooth * Time.deltaTime, 0f);
            }
        }

        RaycastHit hitLower45;
        if (Physics.Raycast(stepLower.transform.position, transform.TransformDirection(1.5f, 0, 1), out hitLower45, 0.1f))
        {
            Debug.Log("First");
            RaycastHit hitUpper45;
            if (!Physics.Raycast(stepUpper.transform.position, transform.TransformDirection(1.5f, 0, 1), out hitUpper45, 0.2f))
            {
                Debug.Log("First");
                rb.position -= new Vector3(0f, -stepSmooth * Time.deltaTime, 0f);
            }
        }

        RaycastHit hitLowerMinus45;
        if (Physics.Raycast(stepLower.transform.position, transform.TransformDirection(-1.5f, 0, 1), out hitLowerMinus45, 0.1f))
        {
            Debug.Log("First");
            RaycastHit hitUpperMinus45;
            if (!Physics.Raycast(stepUpper.transform.position, transform.TransformDirection(-1.5f, 0, 1), out hitUpperMinus45, 0.2f))
            {
                Debug.Log("First");
                rb.position -= new Vector3(0f, -stepSmooth * Time.deltaTime, 0f);
            }
        }
    }



}
