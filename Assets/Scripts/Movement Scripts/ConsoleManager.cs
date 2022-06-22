using System.Collections;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System;

public class ConsoleManager : MonoBehaviour
{
    [Header("Command set up")]
    public List<string> commandEntry = new List<string>();
    public bool canDeleteEntries;
    public float deleteDelay;
    public KeyCode consoleKey;

    [Header("Prefabs")]
    public GameObject logPrefab;
    public GameObject cmdListPrefab;
    spaceshipMovementScript spaceshipMovementScript;
    public GameObject spaceship;


    //Variables not shown
    private InputField consoleInput;
    private Transform log;
    private GameObject console;

    //Command regex

    private Regex engine_enhance = new Regex("engine_thrust\\([1-9]\\)", RegexOptions.IgnoreCase);
    private Regex ship_height = new Regex("ship_height\\([1-9]\\)", RegexOptions.IgnoreCase);
    private Regex light_control = new Regex("lights_control\\([0-1]\\)", RegexOptions.IgnoreCase);
    private Regex intNumbers = new Regex(@"\d+"); // INTEGER NUMBERS
    private Regex ship_rotation = new Regex("ship_rotation\\(([+-]?(?=\\.\\d|\\d)(?:\\d+)?(?:\\.?\\d*))(?:[eE]([+-]?\\d+))?\\)", RegexOptions.IgnoreCase);
    private Regex rotation_control = new Regex("rotation_control\\([0-1]\\)", RegexOptions.IgnoreCase);
    private void Awake()
    {
        console = transform.Find("Console").gameObject;
        consoleInput = transform.Find("Console/CmdPanel/cmdinput").GetComponent<InputField>();
        log = transform.Find("Console/CmdPanel/cmdlogs");
        spaceshipMovementScript = spaceship.GetComponent<spaceshipMovementScript>();
        
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(consoleKey))
        {
            console.SetActive(!console.activeInHierarchy);
            consoleInput.Select();
            consoleInput.ActivateInputField();
        }

        if (Input.GetKeyDown(KeyCode.Return))
        {
            CommandParser(consoleInput.text);

            consoleInput.text = "";
            consoleInput.Select();
            consoleInput.ActivateInputField();
        }
    }

    void SpeedUpEngines()
    {

    }

    void CommandParser(string cmd)
    {
        cmd = cmd.ToLower();

        if (engine_enhance.IsMatch(cmd))
        {
            int speedEnhancement = Int32.Parse(string.Join("", cmd.ToCharArray().Where(Char.IsDigit)));
            this.spaceshipMovementScript.AddSpeed(speedEnhancement);
            Debug.Log(speedEnhancement);
        }
        if (light_control.IsMatch(cmd))
        {
            // TO DO LIGHTS HERE;
            int speedEnhancement = Int32.Parse(string.Join("", cmd.ToCharArray().Where(Char.IsDigit)));
            Debug.Log(speedEnhancement);
        }
        if (ship_rotation.IsMatch(cmd))
        {
            float shipAngle = float.Parse(string.Join("", cmd.ToCharArray().Where(Char.IsDigit)));
            this.spaceshipMovementScript.StartRotating(shipAngle);
            Debug.Log(shipAngle);
        }
        if(cmd == "move")
        {

            this.spaceshipMovementScript.isMoving = true;
        }
        if (rotation_control.IsMatch(cmd))
        {
            int rotationControl = Int32.Parse(string.Join("", cmd.ToCharArray().Where(Char.IsDigit)));
            switch (rotationControl)
            {
                case 1:
                    this.spaceshipMovementScript.isRotating = true;
                    break;
                case 2:
                    this.spaceshipMovementScript.isRotating = false;
                    break;
            }
        }
        if (ship_height.IsMatch(cmd))
        {
            float liftUp = float.Parse(string.Join("", cmd.ToCharArray().Where(Char.IsDigit)));
            this.spaceshipMovementScript.LiftUpShip(liftUp);
        }



        

    }
}
