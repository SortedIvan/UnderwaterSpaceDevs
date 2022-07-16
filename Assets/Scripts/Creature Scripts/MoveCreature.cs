using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCreature : MonoBehaviour
{
    [SerializeField] Transform[] Positions;
    [SerializeField] float objectSpeed;
    [SerializeField] List<string> objectsThatScareFish;
    [SerializeField] public GameObject fishEyes;
    [SerializeField] public GameObject fishBody;

    [SerializeField] public Material fishEyeM;
    [SerializeField] public Material fishBodyM;

    private float speedWhenScared = 3f;
    private float normalSpeed = 1f;

    private bool fishIsScared = false;
    private float timeLeft;
    int nextPosIndex;
    Transform nextPos;

    Material fishEyesInstance;
    Material fishBodyInstance;

    private SkinnedMeshRenderer skinRendererEyes;
    private SkinnedMeshRenderer skinRenderedBody;


    // Start is called before the first frame update
    void Start()
    {
        fishEyesInstance = new Material(fishEyeM);
        fishBodyInstance = new Material(fishBodyM);

        skinRendererEyes = fishEyes.GetComponent<SkinnedMeshRenderer>();
        skinRenderedBody = fishBody.GetComponent<SkinnedMeshRenderer>();

        skinRendererEyes.sharedMaterial = fishEyesInstance;
        skinRenderedBody.sharedMaterial = fishBodyInstance;

        nextPos = GetRandomPosition();
    }

    // Update is called once per frame
    void Update()
    {
        MoveGameObject();

        if (fishIsScared)
        {
            ScareFish();
        }
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

    private void ScareFish()
    {
        timeLeft -= Time.deltaTime;
        if (timeLeft <= 0)
        {
            SetFishSettings(false);
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
                SetFishSettings(true);
            }
        }
    }

    private void SetFishSettings(bool isScared)
    {
        if (isScared)
        {
            fishIsScared = true;
            timeLeft = 3f;
            objectSpeed = speedWhenScared;
            fishEyesInstance.SetFloat("WaveSpeed", 6);
            fishBodyInstance.SetFloat("WaveSpeed", 6);
            fishEyesInstance.SetFloat("Amplitude", 16);
            fishBodyInstance.SetFloat("Amplitude", 16);
            

        }
        else
        {
            fishIsScared = false;
            objectSpeed = normalSpeed;
            fishEyesInstance.SetFloat("WaveSpeed", 3);
            fishBodyInstance.SetFloat("WaveSpeed", 3);
            fishEyesInstance.SetFloat("Amplitude", 12);
            fishBodyInstance.SetFloat("Amplitude", 12);
        }
    }

    
}
