using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class thermal_block_script : MonoBehaviour
{

    [SerializeField] GameObject Controls;

    private MeshRenderer meshRenderer;

    // Start is called before the first frame update
    void Start()
    {
        // get mesh renderer
        meshRenderer = GetComponent<MeshRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Controls != null && Controls.GetComponent<test_controls>().Thermal_Cam)
        {
            meshRenderer.enabled = true;
        }
        else
        {
            meshRenderer.enabled = false;
        }
    }
}

