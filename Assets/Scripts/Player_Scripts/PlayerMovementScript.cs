using UnityEngine;

public class PlayerMovementScript : MonoBehaviour
{

    [Header("Player Attributes")]
    [SerializeField] public Rigidbody rb;
    [SerializeField] public CharacterController cc;

    [Header("Player movement variables")]
    [SerializeField] public float move_speed = 10f;
    [SerializeField] public float player_acceleration = 3f;
    private float verticalInput;
    private float horizontalInput;
    
    [Header("Ground logic")]
    [SerializeField] public float distToGround = 2f;
    [SerializeField] public LayerMask groundLayer;
    [SerializeField] public bool isOnGround;
    RaycastHit hit;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        CheckIsOnGround();

        //if (isOnGround)
        //{
        //    Vector3 direction = new Vector3(horizontalInput, 0, 0);
        //    Vector3 player_velocity = direction * move_speed;
        //    cc.Move(player_velocity * Time.deltaTime);
        //}
    }

    void FixedUpdate()
    {
        horizontalInput = Input.GetAxis("Horizontal");
    }

    public void CheckIsOnGround()
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


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Vector3 direction = transform.TransformDirection(Vector3.down) * 5;
        Gizmos.DrawRay(transform.position, direction);
    }
}
