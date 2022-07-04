using UnityEngine;

public class DistanceJoint3D : MonoBehaviour
{

    public Transform ConnectedRigidbody;
    public Transform NextRigidbody;
    public bool DetermineDistanceOnStart = true;
    public float Distance;
    public Vector3 Direction;
    public float Spring = 0.1f;
    public float Damper = 5f;

    protected Rigidbody Rigidbody;

    void Awake()
    {
        Rigidbody = GetComponent<Rigidbody>();
    }

    void Start()
    {
        if (DetermineDistanceOnStart && ConnectedRigidbody != null)
            Distance = Vector3.Distance(Rigidbody.position, ConnectedRigidbody.position);
    }

    void Update()
    {
        if (NextRigidbody != null)
        {
            Direction = NextRigidbody.position - Rigidbody.position;
        }
    }

	void FixedUpdate()
    {
		if (NextRigidbody != null)
        {
            Rigidbody.rotation = Quaternion.LookRotation(Vector3.forward, Direction);
		}

		var connection = Rigidbody.position - ConnectedRigidbody.position;
        var distanceDiscrepancy = Distance - connection.magnitude;

        Rigidbody.position += distanceDiscrepancy * connection.normalized;

        var velocityTarget = connection + (Rigidbody.velocity + new Vector3(0, 1, 0) * Spring);
        var projectOnConnection = Vector3.Project(velocityTarget, connection);
        Rigidbody.velocity = (velocityTarget - projectOnConnection) / (1 + Damper * Time.fixedDeltaTime);
    }
}