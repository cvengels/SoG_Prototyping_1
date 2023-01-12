using System.Threading;
using TMPro;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    private string[] joysticks;
    private int joysticksCount, joysticksCountOld;
    private bool searchForNewInputDevices;
    private bool inputDevicesChanged;
    private string playerName;

    private class PlayerControls
    {
        private string moveUp, moveDown, moveLeft, moveRight, actionButton;
        private string horizontalMovement, verticalMovement;
        private string playerName;

        public PlayerControls(string playerName)
        {
            this.playerName = playerName;
            print("Player Controller" + playerName + " created");
        }
        
        public void SetMovement(string horizontalMovement, string verticalMovement)
        {
            this.horizontalMovement = horizontalMovement;
            this.verticalMovement = verticalMovement;

        }

        public void SetMovement(string moveUp, string moveDown, string moveLeft, string moveRight)
        {
            this.moveUp = moveUp;
            this.moveDown = moveDown;
            this.moveLeft = moveLeft;
            this.moveRight = moveRight;
        }

        public string GetName()
        {
            return playerName;
        }

        public void SetAction(string actionButton)
        {
            this.actionButton = actionButton;
        }

        public string GetUp()
        {
            return moveUp;
        }

        public string GetDown()
        {
            return moveDown;
        }

        public string GetLeft()
        {
            return moveLeft;
        }

        public string GetRight()
        {
            return moveRight;
        }

        public string GetAction()
        {
            return actionButton;
        }
    }

    public TMP_Text statusPlayerOne, statusPlayerTwo;
    
    private Thread searchForInputDevices;

    private PlayerControls playerOne, playerTwo;
    
    private void Awake()
    {
        playerOne = new PlayerControls("Player 1");
        playerTwo = new PlayerControls("Player 2");
    
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
                    print("Only keyboard found. Use two keyboards to control players");
                    statusPlayerOne.text = "Player 1: Keyboard (1)";
                    playerOne.SetMovement("W", "S", "A", "D");
                    playerOne.SetAction("E");
                    statusPlayerTwo.text = "Player 2: Keyboard (2)";
                    playerTwo.SetMovement("UpArrow", "DownArrow", "LeftArrow", "RightArrow");
                    playerTwo.SetAction("Space");
                    break;
                
                case 1:
                    print("One joystick found. Player one uses keyboard, player two joystick");
                    statusPlayerOne.text = "Player 1: Keyboard";
                    playerOne.SetMovement("W", "S", "A", "D");
                    playerOne.SetAction("E");
                    statusPlayerTwo.text = "Player 2: Gamepad";
                    playerTwo.SetMovement("Horizontal", "Vertical");
                    playerTwo.SetAction("Joystick1Button0");
                    break;
                
                case 2:
                    print("Both players use joysticks, assigned by joystick number");
                    statusPlayerOne.text = "Player 1: Gamepad (1)";
                    playerOne.SetMovement("Horizontal", "Vertical");
                    playerOne.SetAction("Joystick1Button0");
                    statusPlayerTwo.text = "Player 2: Gamepad (2)";
                    playerTwo.SetMovement("Horizontal", "Vertical");
                    playerTwo.SetAction("Joystick2Button0");
                    break;
                
                default:
                    print("More than one joystick found. Same procedure like case 2");
                    statusPlayerOne.text = "Player 1: Gamepad (1)";
                    playerOne.SetMovement("Horizontal", "Vertical");
                    playerOne.SetAction("Joystick1Button0");
                    statusPlayerTwo.text = "Player 2: Gamepad (2)";
                    playerTwo.SetMovement("Horizontal", "Vertical");
                    playerTwo.SetAction("Joystick2Button0");
                    break;
            }
        }
    }
    
    public void SetPlayer(string playerName)
    {
        this.playerName = playerName;
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
