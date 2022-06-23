using UnityEngine;

public class PlayerMovementScript : MonoBehaviour
{

    [Header("Player Attributes")]
    [SerializeField] public Rigidbody rb;
    [SerializeField] public CharacterController cc;

    [Header("Player movement variables")]
    [SerializeField] public float move_speed = 10f;
    [SerializeField] public float movement_multiplier = 10f;
    private float verticalInput;
    private float horizontalInput;
    private Vector3 moveDirection;
    
    [Header("Ground logic")]
    [SerializeField] public float distToGround = 2f;
    [SerializeField] public LayerMask groundLayer;
    [SerializeField] public bool isOnGround;
    [SerializeField] public float playerGroundDrag = 6f;
    private bool player_is_moving = false;
    RaycastHit hit;

    private GroundSoundsController soundsController;


    public AudioClip clip;
    public AudioSource source;
    

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
        ApplyDrag();
        CheckIsMoving();

        if (Physics.Raycast(CheckIfOnMetal(), out RaycastHit hit, distToGround))
        {
            if (hit.collider.tag == "Metal")
            {
                PlaySounds();
            }
        }
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
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");
        moveDirection = transform.forward * verticalInput + transform.right * horizontalInput;
    }

    private void ApplyDrag()
    {
        rb.drag = playerGroundDrag;
    }

    private void MovePlayer()
    {
        // Normalized makes it so that it isn't bad on diagonals
        rb.AddForce(moveDirection.normalized * move_speed * movement_multiplier, ForceMode.Acceleration); 
    }

    private void CheckIsOnGround()
    {
        if (Physics.Raycast(transform.position, Vector3.down, out hit, distToGround + 0.1f, groundLayer))
        {
            isOnGround = true;
        }
        else
        {
            isOnGround = false;
        }
    }

    private Ray CheckIfOnMetal()
    {
        return new Ray(transform.position, transform.TransformDirection(Vector3.down * distToGround));
    }

    private void CheckIsMoving()
    {
        if (horizontalInput > 0f)
        {
            player_is_moving = true;
        }
        else player_is_moving = false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Vector3 direction = transform.TransformDirection(Vector3.down) * 5;
        Gizmos.DrawRay(transform.position, direction);
    }

    public void PlaySounds()
    {
            if (player_is_moving)
            {
            Debug.Log("SOUND SHOULD BE PLAYING NOW");
                soundsController.PlayMetalWalkingSound(true);
            }
            else soundsController.PlayMetalWalkingSound(false);
    }
}
