using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class thermal_script : MonoBehaviour
{

    [SerializeField] GameObject Controls;
    [SerializeField] Material material;

    private Material materialInstance;
    private SkinnedMeshRenderer skinRenderer;
    private MeshRenderer meshRenderer;

    public float Distance;
    private Transform transformCam;

    // Start is called before the first frame update
    void Start()
    {
        // get mesh or skin renderer
        skinRenderer = GetComponent<SkinnedMeshRenderer>();
        meshRenderer = GetComponent<MeshRenderer>();
        
        // create new material instance
        materialInstance = new Material(material);
        materialInstance.name = material.name + " instance";

        // put new material to use
        if (skinRenderer) skinRenderer.sharedMaterial = materialInstance;
        if (meshRenderer) meshRenderer.sharedMaterial = materialInstance;

        if (GameObject.Find("Main Camera") != null)
        {
            transformCam = GameObject.Find("Main Camera").GetComponent<Transform>();
        }
    }

    void Update()
    {
        if(Controls != null && Controls.GetComponent<test_controls>().Thermal_Cam)
		{
            if (skinRenderer) skinRenderer.enabled = true;
            if (meshRenderer) meshRenderer.enabled = true;

            Distance = Vector3.Distance(transform.position, transformCam.position);
            materialInstance.SetFloat("_Distance", Distance);

        } else
		{
            if (skinRenderer) skinRenderer.enabled = false;
            if (meshRenderer) meshRenderer.enabled = false;
        }
    }
}
