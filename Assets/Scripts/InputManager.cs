using System;
using System.Threading;
using TMPro;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance { get; private set; }

    private string[] joysticks;
    private int joysticksCount, joysticksCountOld;
    private bool searchForNewInputDevices;
    private bool inputDevicesChanged;

    private string playerOneHorizontalAxis, playerOneVerticalAxis, playerOneActionButton;
    private string playerTwoHorizontalAxis, playerTwoVerticalAxis, playerTwoActionButton;

    public TMP_Text statusPlayerOne, statusPlayerTwo;
    
    private Thread searchForInputDevices;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
        
        searchForInputDevices = new Thread(InputChecker);
        searchForNewInputDevices = true;
        joysticksCountOld = 99;
    }

    private void Start()
    {
        searchForInputDevices.Start();
    }
    
    private void Update()
    {
        // TODO No Joystick found: Use 2 Keyboards for control
        // Player controls: WASD + SPACE / ArrowKeys + CTRL left,SHIFT left

        if (searchForNewInputDevices)
        {
            searchForNewInputDevices = false;
            joysticksCount = 0;
            
            // Search for input devices
            joysticks = Input.GetJoystickNames();
            
            // Is there a new input device?
            foreach (var inputDevice in joysticks)
            {
                joysticksCount += !string.IsNullOrEmpty(inputDevice) ? 1 : 0;
            }

            if (joysticksCount != joysticksCountOld)
            {
                // If true, notify in Update
                inputDevicesChanged = true;
                joysticksCountOld = joysticksCount;
            }
        }
        if (inputDevicesChanged)
        {
            inputDevicesChanged = false;

            switch (joysticksCount)
            {
                case 0:
                    playerOneHorizontalAxis = "KeyboardOnlyAD";
                    playerOneVerticalAxis   = "KeyboardOnlyWS";
                    playerOneActionButton   = "KeyboardOnlyAction1";

                    playerTwoHorizontalAxis = "KeyboardOnlyUpDown";
                    playerTwoVerticalAxis   = "KeyboardOnlyLeftRight";
                    playerTwoActionButton   = "KeyboardOnlyAction2";
                    
                    print("Only keyboard found. Use two keyboards to control players");
                    statusPlayerOne.text = "Player 1: Keyboard (1)";
                    statusPlayerTwo.text = "Player 2: Keyboard (2)";
                    break;
                
                case 1:
                    playerOneHorizontalAxis = "KeyboardOnlyAD";
                    playerOneVerticalAxis   = "KeyboardOnlyWS";
                    playerOneActionButton   = "KeyboardOnlyAction1";

                    playerTwoHorizontalAxis = "Player2 Joystick Horizontal";
                    playerTwoVerticalAxis   = "Player2 Joystick Vertical";
                    playerTwoActionButton   = "Joystick2 Action";
                    
                    print("One joystick found. Player one uses keyboard, player two joystick");
                    statusPlayerOne.text = "Player 1: Keyboard";
                    statusPlayerTwo.text = "Player 2: Gamepad";
                    break;
                
                case 2:
                    playerOneHorizontalAxis = "Player1 Joystick Horizontal";
                    playerOneVerticalAxis   = "Player1 Joystick Vertical";
                    playerOneActionButton   = "Joystick1 Action";

                    playerTwoHorizontalAxis = "Player2 Joystick Horizontal";
                    playerTwoVerticalAxis   = "Player2 Joystick Vertical";
                    playerTwoActionButton   = "Joystick2 Action";
                    
                    print("Both players use joysticks, assigned by joystick number");
                    statusPlayerOne.text = "Player 1: Gamepad (1)";
                    statusPlayerTwo.text = "Player 2: Gamepad (2)";
                    break;
                
                default:
                    playerOneHorizontalAxis = "Player1 Joystick Horizontal";
                    playerOneVerticalAxis   = "Player1 Joystick Vertical";
                    playerOneActionButton   = "Joystick1 Action";

                    playerTwoHorizontalAxis = "Player2 Joystick Horizontal";
                    playerTwoVerticalAxis   = "Player2 Joystick Vertical";
                    playerTwoActionButton   = "Joystick2 Action";
                    
                    print("More than one joystick found. Same procedure like case 2");
                    statusPlayerOne.text = "Player 1: Gamepad (1)";
                    statusPlayerTwo.text = "Player 2: Gamepad (2)";
                    break;
            }
        }
    }

    public string GetInputForPlayer(int playerNumber, string input)
    {
        if (input == "Horizontal")
        {
            return playerNumber == 1 ? playerOneHorizontalAxis : playerTwoHorizontalAxis;
        }
        else if (input == "Vertical")
        {
            return playerNumber == 1 ? playerOneVerticalAxis : playerTwoVerticalAxis;
        }
        else if (input == "Action")
        {
            return playerNumber == 1 ? playerOneActionButton : playerTwoActionButton;
        }
        return "Error";
    }

    private void InputChecker()
    {
        while (true)
        {
            Thread.Sleep(0);
            searchForNewInputDevices = true;
            Thread.Sleep(1000);
        }
    }
    
    
}
