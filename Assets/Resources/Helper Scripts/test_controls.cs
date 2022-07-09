using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test_controls : MonoBehaviour
{

    public bool Thermal_Cam = false;
    public Camera MainCam;
    public Camera ThermalCam;

    // Start is called before the first frame update
    void Start()
    {
        MainCam = GameObject.Find("Main Camera").GetComponent<Camera>();
        ThermalCam = GameObject.Find("Thermal Camera").GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.T))
		{
            MainCam.enabled = !MainCam.enabled;
            ThermalCam.enabled = !ThermalCam.enabled;
            Thermal_Cam = !Thermal_Cam;
		}
    }
}
