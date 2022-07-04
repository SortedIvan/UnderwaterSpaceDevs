using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RopeRoot : MonoBehaviour
{

    public float RigidbodyMass = 1f;
    public float ColliderRadius = 0.1f;
    public float JointSpring = 0.1f;
    public float JointDamper = 5f;
    public Vector3 RotationOffset;
    public Vector3 PositionOffset;

    protected List<Transform> CopySource;
    protected List<Transform> CopyDestination;
    protected static GameObject RigidBodyContainer;

    void Awake()
    {
        if (RigidBodyContainer == null)
            RigidBodyContainer = new GameObject("RopeRigidbodyContainer");

        CopySource = new List<Transform>();
        CopyDestination = new List<Transform>();

        //add children
        AddChildren(transform);
    }

    private void AddChildren(Transform parent)
    {
        for (int i = 0; i < parent.childCount; i++)
        {
            var child = parent.GetChild(i);
            var representative = new GameObject(child.gameObject.name);
            //set parameters from child object (from actual joint)
            representative.transform.parent = RigidBodyContainer.transform;
            representative.transform.position = child.transform.position;
            representative.transform.rotation = child.transform.rotation;
            //rigidbody
            var childRigidbody = representative.gameObject.AddComponent<Rigidbody>();
            childRigidbody.useGravity = false;
            childRigidbody.isKinematic = false;
            childRigidbody.freezeRotation = false;
            childRigidbody.mass = RigidbodyMass;

            //collider
            var collider = representative.gameObject.AddComponent<SphereCollider>();
            collider.center = Vector3.zero;
            collider.radius = ColliderRadius;

            //DistanceJoint
            var joint = representative.gameObject.AddComponent<DistanceJoint3D>();
            joint.ConnectedRigidbody = parent;
            joint.DetermineDistanceOnStart = true;
            joint.Spring = JointSpring;
            joint.Damper = JointDamper;
            joint.DetermineDistanceOnStart = false;
            joint.Distance = Vector3.Distance(parent.position, child.position);

            //add copy source
            CopySource.Add(representative.transform); // copy generated joints
            CopyDestination.Add(child);               // to actual joints

            AddChildren(child);
        }
    }

    public void Update()
    {
        for (int i = 0; i < CopySource.Count; i++)
        {
            CopyDestination[i].position = CopySource[i].position + PositionOffset;
            CopyDestination[i].rotation = CopySource[i].rotation * Quaternion.Euler(RotationOffset);
        }

        for (int i = 0; i < CopySource.Count - 1; i++)
        {
            CopySource[i].gameObject.GetComponent<DistanceJoint3D>().NextRigidbody = CopySource[i + 1];
        }
    }
}
