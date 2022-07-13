using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCreature : MonoBehaviour
{
    [SerializeField] Transform[] Positions;
    [SerializeField] float objectSpeed;
    [SerializeField] List<string> objectsThatScareFish;

    int nextPosIndex;
    Transform nextPos;
    // Start is called before the first frame update
    void Start()
    {
        nextPos = GetRandomPosition();
    }

    // Update is called once per frame
    void Update()
    {
        MoveGameObject();
    }

    private Transform GetRandomPosition()
    {
        return Positions[Random.Range(0, Positions.Length)];
    }
    void MoveGameObject()
    {
        if (transform.position == nextPos.position)
        {
            nextPos = GetRandomPosition();
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, nextPos.position, objectSpeed * Time.deltaTime);
            transform.rotation = Quaternion.LookRotation(nextPos.position - transform.position, transform.up);
        }
    }


    private Transform FindFurthestObject()
    {
        float FurthestDistance = 0;
        Transform furthestWaypoint = null;
        foreach (Transform waypoint in Positions)
        {
            float ObjectDistance = Vector3.Distance(transform.position, waypoint.position);
            if (ObjectDistance > FurthestDistance)
            {
                furthestWaypoint = waypoint;
                FurthestDistance = ObjectDistance;
            }
        }
        return furthestWaypoint;
    }

    private void OnTriggerEnter(Collider other)
    {
        for (int i = 0; i < objectsThatScareFish.Count; i++)
        {
            if (objectsThatScareFish[i] == other.tag)
            {
                //transform.position = nextPos.position;
                nextPos = FindFurthestObject();
            }
        }
    }
}
