using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AICreatureScript : MonoBehaviour
{
    [SerializeField] public GameObject creature;
    [SerializeField] public GameObject spawnPoint;
    public float moveSpeed = 5f;
    private Vector3 direction = Vector3.zero;

    void Start()
    {
        direction = Random.insideUnitSphere;
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

    void Update()
    {
        creature.transform.position += direction * moveSpeed * Time.deltaTime;
    }
}
