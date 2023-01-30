using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHandler : MonoBehaviour
{
    private GameObject surrogate;
    private PlayerMovement playerMovementController;
    private FightManager fightManager;
    private CharType characterType;

    public void OnGetCharacterToControl(GameObject characterPrefab, SpawnPoint spawnPosition)
    {
        // Assigned player Character
        characterType = characterPrefab.GetComponent<PlayerIndividualBehavior>().GetPrefabType();
        name = "Surrogate of " + characterType.ToString();
        surrogate = Instantiate(characterPrefab, spawnPosition.transform.position, Quaternion.identity);
        if (surrogate != null)
        {
            print($"Character {name} spawned at {spawnPosition.name}");
        }
        playerMovementController = surrogate.GetComponent<PlayerMovement>();
        // Fight manager instance
    }

    public void OnGetFightToControl(FightManager fightManager)
    {
        this.fightManager = fightManager;
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
                surrogate.gameObject?.SetActive(false);
                break;
            case GameState.Fight:
                break;
            case GameState.FightEnd:
                playerMovementController.transform.position = fightManager.transform.position;
                surrogate.transform.position = fightManager.transform.position;
                surrogate.gameObject?.SetActive(true);
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
                transform.position = fightManager.transform.position;
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
        Vector2 movementDirection = context.ReadValue<Vector2>();
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
                playerMovementController?.SetMovement(movementDirection);
                break;
            case GameState.FightBegin:
                break;
            case GameState.Fight:
                fightManager?.SetMovement(movementDirection, characterType);
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

    public void OnAction(InputAction.CallbackContext context)
    {
        bool contextPerformed = false;
        if (context.performed)
        {
            contextPerformed = true;
        }
        if (context.canceled)
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
                fightManager?.SetAction(contextPerformed, characterType);
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
        GameEventManager.Instance.OnGameStateChanged += OnGameStateChanged;
        GameEventManager.Instance.OnFightStarted += OnFightStarted;
        GameEventManager.Instance.OnFightEnded += OnFightEnded;
    }



    private void OnDisable()
    {
        GameEventManager.Instance.OnGameStateChanged -= OnGameStateChanged;
        GameEventManager.Instance.OnFightStarted += OnFightStarted;
        GameEventManager.Instance.OnFightEnded += OnFightEnded;
    }
}

