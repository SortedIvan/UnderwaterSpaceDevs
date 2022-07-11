using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AICreatureScript : MonoBehaviour
{
    [SerializeField] public GameObject creature;
    [SerializeField] public GameObject spawnPoint;
    [SerializeField] List<GameObject> waypoints;
    public float moveSpeed = 5f;
    private Vector3 direction = Vector3.zero;
    private float counterToChangeDirection;
    float speed = 0.01f;
    Vector3 initialPosition;
    Quaternion rotationBeforeDirSwitch;
    Quaternion rotation;

    void Start()
    {
        direction = Random.insideUnitSphere;
        initialPosition = creature.transform.position;
    }

    private Vector3 GetRandomDirection()
    {
        Vector3 randomPosition = new Vector3(
            Random.Range(-spawnPoint.transform.position.x, spawnPoint.transform.position.x),
            Random.Range(-spawnPoint.transform.position.y, spawnPoint.transform.position.y),
            Random.Range(-spawnPoint.transform.position.z, spawnPoint.transform.position.z)
            );
        return randomPosition;
    }

    private Vector3 GetRandomWaypoint()
    {
        return waypoints[Random.Range(0, 2)].transform.position;
    }

    void Update()
    {
        //creature.transform.position += direction * moveSpeed * Time.deltaTime;
        
        if (Input.GetKeyDown(KeyCode.U))
        {
            rotationBeforeDirSwitch = creature.transform.rotation;
            initialPosition = creature.transform.position;
            direction = GetRandomWaypoint();
            //MoveFunction();
        }

        MoveCreatureSlowly();
        //creature.transform.position = Vector3.MoveTowards(initialPosition, direction, moveSpeed * Time.deltaTime);
        //creature.transform.position += direction * moveSpeed * Time.deltaTime;
        rotation = Quaternion.LookRotation(direction - creature.transform.position, creature.transform.up);
        transform.rotation = Quaternion.Lerp(rotationBeforeDirSwitch, rotation, 5 * speed);
    }

    private void MoveCreatureSlowly()
    {
        creature.transform.position = Vector3.MoveTowards(initialPosition, direction, moveSpeed * Time.deltaTime);
    }
    IEnumerator MoveFunction()
    {
        float timeSinceStarted = 0f;
        while (true)
        {
            timeSinceStarted += Time.deltaTime;
            creature.transform.position = Vector3.Lerp(creature.transform.position, direction, timeSinceStarted);
            creature.transform.position = direction;
            // If the object has arrived, stop the coroutine
            if (creature.transform.position == direction)
            {
                yield break;
            }

            // Otherwise, continue next frame
            yield return null;
        }
    }
}
