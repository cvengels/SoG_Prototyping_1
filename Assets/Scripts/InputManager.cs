using System.Threading;
using TMPro;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    private string[] joysticks;
    private int joysticksCount, joysticksCountOld;
    private bool searchForNewInputDevices;
    private bool inputDevicesChanged;

    public TMP_Text statusPlayerOne, statusPlayerTwo;
    
    private Thread searchForInputDevices;
    
    private void Awake()
    {
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
        
        // TODO One or more joysticks found: first player who presses the action button is player 1
        // If only one joystick is present, only enable WASD + SPACE (prevent keyboard player to control joystick player)

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
                    print("Only keyboard found. Use two keyboards to control players");
                    statusPlayerOne.text = "Player 1: Keyboard (1)";
                    statusPlayerTwo.text = "Player 2: Keyboard (2)";
                    break;
                
                case 1:
                    print("One joystick found. Player one uses keyboard, player two joystick");
                    statusPlayerOne.text = "Player 1: Keyboard";
                    statusPlayerTwo.text = "Player 2: Gamepad";
                    break;
                
                case 2:
                    print("Both players use joysticks, assigned by joystick number");
                    statusPlayerOne.text = "Player 1: Gamepad (1)";
                    statusPlayerTwo.text = "Player 2: Gamepad (2)";
                    break;
                
                default:
                    print("More than one joystick found. Same procedure like case 2");
                    statusPlayerOne.text = "Player 1: Gamepad (1)";
                    statusPlayerTwo.text = "Player 2: Gamepad (2)";
                    break;
            }
        }
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
