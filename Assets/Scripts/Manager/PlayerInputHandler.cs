using System;
using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;
using Random = UnityEngine.Random;

public class PlayerInputHandler : MonoBehaviour
{
    private GameObject surrogate;
    private PlayerMovement playerMovementController;
    private CharType characterType;

    public void OnGetCharacterToControl(GameObject characterPrefab, SpawnPoint spawnPosition)
    {
        // Assigned player Character
        characterType = characterPrefab.GetComponent<PlayerIndividualBehavior>().GetPrefabType();
        name = "Surrogate of " + characterType.ToString();
        surrogate = Instantiate(characterPrefab, spawnPosition.transform.position, Quaternion.identity);
        if (surrogate != null)
        {
            print($"Character {name} spawned at {spawnPosition.name} ({(Vector2)spawnPosition.transform.position})");
        }
        playerMovementController = surrogate.GetComponent<PlayerMovement>();
        // Fight manager instance
    }


    private void OnGameStateChanged(GameState newGameState)
    {
        switch (newGameState)
        {
            case GameState.MainMenu:
                break;
            case GameState.PlayerSelect:
                break;
            case GameState.Options:
                break;
            case GameState.Credits:
                break;
            case GameState.LevelBegin:
                break;
            case GameState.LevelRunning:
                break;
            case GameState.FightBegin:
                if (FightManager.Instance == null)
                {
                    GameObject fightInstance = Instantiate(GameManager.Instance.GetFightPrefab(), surrogate.transform.position,
                        Quaternion.identity);
                    fightInstance.GetComponent<Rigidbody2D>().velocity = (Vector2.right * Random.Range(-1, 0) + Vector2.up) * 50f;

                    GameObject.
                        FindGameObjectWithTag("Player1Cam").
                        GetComponent<CinemachineVirtualCamera>().
                        Follow = fightInstance.transform;
                    GameObject.
                        FindGameObjectWithTag("Player2Cam").
                        GetComponent<CinemachineVirtualCamera>().
                        Follow = fightInstance.transform;
                }
                surrogate.gameObject?.SetActive(false);
                break;
            case GameState.Fight:
                break;
            case GameState.FightEnd:
                playerMovementController.transform.position = FightManager.Instance.transform.position;
                surrogate.transform.position = FightManager.Instance.transform.position;
                surrogate.gameObject?.SetActive(true);

                string playerCamName = "";

                if (GetComponent<PlayerInput>().playerIndex == 0)
                {
                    playerCamName = "Player1Cam";
                }
                else
                {
                    playerCamName = "Player2Cam";
                }

                GameObject.FindGameObjectWithTag(playerCamName).
                    GetComponent<CinemachineVirtualCamera>().
                    Follow = transform;
                
                break;
            case GameState.LevelEnd:
                break;
            case GameState.Pause:
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(newGameState), newGameState, null);
        }
    }

    
    private void Update()
    {
        switch (GameManager.Instance.GetGameState())
        {
            case GameState.MainMenu:
                break;
            case GameState.PlayerSelect:
                break;
            case GameState.Options:
                break;
            case GameState.Credits:
                break;
            case GameState.LevelBegin:
                break;
            case GameState.LevelRunning:
                transform.position = playerMovementController.transform.position;
                break;
            case GameState.FightBegin:
                break;
            case GameState.Fight:
                transform.position = FightManager.Instance.transform.position;
                break;
            case GameState.FightEnd:
                break;
            case GameState.LevelEnd:
                break;
            case GameState.Pause:
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        if (gameObject.scene.IsValid())
        {
            Vector2 movementDirection = context.ReadValue<Vector2>();
            if (playerMovementController != null)
            {
                playerMovementController.SetMovement(movementDirection);
            }

            if (FightManager.Instance != null)
            {
                FightManager.Instance.SetMovement(movementDirection, characterType);
            }
        }
    }

    public void OnAction(InputAction.CallbackContext context)
    {
        if (gameObject.scene.IsValid())
        {
            bool contextPerformed = false;
            if (context.performed)
            {
                contextPerformed = true;
            }
            if (context.started || context.canceled)
            {
                contextPerformed = false;
            }
            switch (GameManager.Instance.GetGameState())
            {
                case GameState.MainMenu:
                    break;
                case GameState.PlayerSelect:
                    break;
                case GameState.Options:
                    break;
                case GameState.Credits:
                    break;
                case GameState.LevelBegin:
                    break;
                case GameState.LevelRunning:
                        playerMovementController?.SetAction(contextPerformed);
                    break;
                case GameState.FightBegin:
                    break;
                case GameState.Fight:
                    FightManager.Instance?.SetAction(contextPerformed, characterType);
                    break;
                case GameState.FightEnd:
                    break;
                case GameState.LevelEnd:
                    break;
                case GameState.Pause:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }

    
    private void OnFightStarted(Vector3 position)
    {
        surrogate.SetActive(false);
    }

    
    private void OnFightEnded(CharType characterWhoWon, Vector3 positionOfFightEnd)
    {

        if (GameManager.Instance.MouseHasWon())
        {
            surrogate.SetActive(true);
            surrogate.transform.position = positionOfFightEnd;

            CharType characterType = surrogate.GetComponent<PlayerIndividualBehavior>().GetPrefabType();
            if (characterType == CharType.Cat)
            {
                playerMovementController.Stun(Vector2.up + Vector2.left);
            }
            else if (characterType == CharType.Mouse)
            {
                playerMovementController.SetMovement(Vector2.up + Vector2.right);
                playerMovementController.SetAction(true, true);
            }
        }
    }
    
    
    private void OnEnable()
    {
        GameEventManager.Instance.onGameStateChanged += OnGameStateChanged;
        GameEventManager.Instance.onFightStarted += OnFightStarted;
        GameEventManager.Instance.onFightEnded += OnFightEnded;
    }



    private void OnDisable()
    {
        GameEventManager.Instance.onGameStateChanged -= OnGameStateChanged;
        GameEventManager.Instance.onFightStarted += OnFightStarted;
        GameEventManager.Instance.onFightEnded += OnFightEnded;
    }
}

