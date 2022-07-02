using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControllerFinal : MonoBehaviour
{
    [Header("Ground variables")]
    [SerializeField] public LayerMask groundLayer;
    [SerializeField] public float distToGround = 2f;
    [SerializeField] public float playerDrag = 10f;
    [SerializeField] public bool playerIsGrounded = false;

    [Header("Slopes and steps")]
    [SerializeField] public GameObject stepUpper;
    [SerializeField] public GameObject stepLower;
    [SerializeField] public float maxStepHeight = 0.15f;
    [SerializeField] public float stepSmooth = 0.01f;

    private Rigidbody rb;
    private float yaw = 0.0f, pitch = 0.0f;    
    private float horizontalInput;
    private float verticalInput;
    
    [Header("Movement")]
    [SerializeField] float walkSpeed = 5.0f, sensitivy = 2.0f;
    private Vector3 moveDirection;
    [SerializeField] public GameObject playerCamera;

    
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        rb = GetComponent<Rigidbody>();
    }

    private void Awake()
    {
        stepUpper.transform.position = new Vector3(stepUpper.transform.position.x, maxStepHeight, stepUpper.transform.position.z);
    }

    void Update()
    {
        LookAround(); 
        GetPlayerInput();
        GetMoveDirection();
    }


    private void LookAround()
    {
        pitch -= Input.GetAxisRaw("Mouse Y") * sensitivy;
        pitch = Mathf.Clamp(pitch, -90f, 90f);
        yaw += Input.GetAxisRaw("Mouse X") * sensitivy;
        Camera.main.transform.localRotation = Quaternion.Euler(pitch, yaw, 0);
    }

    private void FixedUpdate()
    {
        // Limit the player speed;
        SpeedControl();

        // Apply drag to the player if he is on the ground
        ApplyDrag();
        if (CheckIsOnGround())
        {
            Movement();
            if (verticalInput > 0f || verticalInput < 0f)
            {
                stepClimb();
            }
        }
    }



    private void ApplyDrag()
    {
        if (CheckIsOnGround())
        {
            rb.drag = playerDrag;
        }
        rb.drag = 0f;
    }

    private void GetMoveDirection()
    {

        moveDirection = transform.forward * verticalInput + transform.right * horizontalInput;
    }

    private bool CheckIsOnGround()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, distToGround + 0.5f, groundLayer))
        {
            return true;
        }
        return false;
    }

    private void SpeedControl()
    {
        Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        if (flatVel.magnitude > walkSpeed)
        {
            Vector3 limitedVel = flatVel.normalized * walkSpeed;
            rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
        }
    }

    void stepClimb()
    {
        RaycastHit hitLower;
        if (Physics.Raycast(stepLower.transform.position, playerCamera.transform.TransformDirection(Vector3.forward), out hitLower, 0.1f))
        {
            Debug.Log("First");
            RaycastHit hitUpper;
            if (!Physics.Raycast(stepUpper.transform.position, playerCamera.transform.TransformDirection(Vector3.forward), out hitUpper, 0.2f))
            {
                Debug.Log("First");
                rb.position -= new Vector3(0f, -stepSmooth * Time.deltaTime, 0f);
            }
        }

        RaycastHit hitLower45;
        if (Physics.Raycast(stepLower.transform.position, rb.transform.TransformDirection(1.5f, 0, 1), out hitLower45, 0.1f))
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

    private void GetPlayerInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");
    }
    void Movement()
    {
        Vector2 axis = new Vector2(verticalInput, horizontalInput).normalized * walkSpeed;
        Vector3 forward = new Vector3(-Camera.main.transform.right.z, 0.0f, Camera.main.transform.right.x);
        Vector3 wishDirection = (forward * axis.x + Camera.main.transform.right * axis.y + Vector3.up * rb.velocity.y);
        rb.velocity = wishDirection;
    }
}
