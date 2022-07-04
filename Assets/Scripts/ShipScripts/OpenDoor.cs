using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenDoor : MonoBehaviour
{
    [SerializeField] public GameObject door;
    private bool PlayerCanOpenThisDoor = false;
    private Vector3 originalDoorPosition;
    //private spaceshipMovementScript spaceshipScript;

    private void Awake()
    {
        this.originalDoorPosition = door.transform.position;
    }

    void Start()
    {
        door = GetComponent<GameObject>();
        //this.spaceshipScript = GetComponent<spaceshipMovementScript>();
    }

    void Update()
    {
        //if (!spaceshipScript.isMoving)
        //{
        //    if (Input.GetKeyDown(KeyCode.E) && PlayerCanOpenThisDoor)
        //    {
        //        door.transform.position = new Vector3(originalDoorPosition.x, 
        //            originalDoorPosition.y, originalDoorPosition.z + 0.5f);
        //    }
        //}

        if (Input.GetKeyDown(KeyCode.E) && PlayerCanOpenThisDoor)
        {
            door.transform.position = new Vector3(originalDoorPosition.x,
                originalDoorPosition.y, originalDoorPosition.z + 0.5f);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            this.originalDoorPosition = door.transform.position;
            PlayerCanOpenThisDoor = true;
            Debug.Log("Player can open the door");
        }
    }

    private void OnTriggerExit(Collider other)
    {
       if (other.CompareTag("Player"))
       {
            PlayerCanOpenThisDoor = false;

            Debug.Log("Player cannot");
       }
        door.transform.position = originalDoorPosition;
    }
}
