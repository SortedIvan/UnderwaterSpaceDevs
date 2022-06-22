using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spaceshipMovementScript : MonoBehaviour
{
    [Header("Movement Variables")]
    public float moveSpeed;

    [Header("Ship GameObject")]
    public GameObject spaceshipGameObject;

    public bool isMoving { get; set; } = false;
    public bool isMovingBackwards { get; set; } = false;
    public bool isRotating { get; set; } = false;

    // Private variables
    #region Ship rotation variables
    private Quaternion startRotation;
    private Quaternion endRotation;
    private float rotationProgress = -1;
    #endregion

    private float shipLifting;
    private bool canLiftShip = false;


    void Start()
    {
        //spaceshipGameObject = gameObject.GetComponent<GameObject>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            Debug.Log(GetHeadingAngle());
            //this.isMoving = true;
        }
        else if (Input.GetKeyDown(KeyCode.E))
        {
            //this.isMoving = false;
        }

        if (isMoving)
        {
            transform.position += transform.forward * Time.deltaTime * moveSpeed;
        }
        if (isMovingBackwards)
        {
            transform.position -= transform.forward * Time.deltaTime * moveSpeed;
        }
        if (rotationProgress < 1 && rotationProgress >= 0)
        {
            rotationProgress += Time.deltaTime * 0.08f;
            transform.rotation = Quaternion.Lerp(startRotation, endRotation, rotationProgress);
        }
        if (canLiftShip)
        {
            if (transform.position.y < shipLifting)
            {
                // Lifting code to be code here
            }
        }
    }

    private void LateUpdate()
    {
        
    }


    public void LiftUpShip(float degree)
    {
       shipLifting = transform.position.y + degree;
       canLiftShip = true; 
    }

    // Call this to start the rotation
    public void StartRotating(float yPosition)
    {
        startRotation = transform.rotation;
        endRotation = Quaternion.Euler(transform.rotation.eulerAngles.x, yPosition, transform.rotation.eulerAngles.z);
        rotationProgress = 0;
    }


    public void AddSpeed(float amount)
    {
        moveSpeed = amount;
    }

    public float GetHeadingAngle()
    {
        Vector3 forward = transform.forward;
        // Zero out the y component of your forward vector to only get the direction in the X,Z plane
        forward.y = 0;
        float headingAngle = Quaternion.LookRotation(forward).eulerAngles.y;
        return headingAngle;
    }

    public void MoveSpaceship(bool isActive)
    {
        if (isActive)
        {
            transform.position += transform.forward * Time.deltaTime * moveSpeed;
        }
    }
}
