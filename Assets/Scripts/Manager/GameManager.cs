using System;
using System.Collections.Generic;
using System.Linq;
using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    private SpawnPoint nextSpawnPosition;

    [Header("Character Prefabs")] 
    [SerializeField] private GameObject prefabCat;
    [SerializeField] private GameObject prefabMouse;
    [SerializeField] private GameObject prefabFight;

    [Header("Interactive Objects")] 
    [SerializeField] private GameObject prefabCheese;

    [Header("Spawn Points")] 
    [SerializeField] private List<SpawnPoint> spawnerList;

    [Header("Global Statistics")]
    [SerializeField] private CharType playerOneCharacter;
    [SerializeField] private CharType playerTwoCharacter;
    [SerializeField] private int gameRounds;
    [SerializeField] private GameState currentGameState;

    [SerializeField] private int mouseLifesLeft;

    [SerializeField] private List<Object> gameScenes;


    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(Instance);
        }
        
#if UNITY_STANDALONE
        foreach (Transform editorHelper in GetComponentsInChildren<Transform>().Where(eh => eh.CompareTag("EditorOnly")))
        {
            editorHelper.gameObject.SetActive(false);
        }
#endif
        
        // Setup screens
        CheckForSecondScreen();

        // Find all spawn points
        AutoFillCollectibles();

        SetPlayerCharactersByRound();
    }


    private void CheckForSecondScreen()
    {
        Debug.Log ("displays connected: " + Display.displays.Length);
        // Display.displays[0] is the primary, default display and is always ON, so start at index 1.
        // Check if additional displays are available and activate each.
    
        for (int i = 1; i < Display.displays.Length; i++)
        {
            Display.displays[i].Activate();
        }
    }
    

    // Set player characters by game round number.
    private void SetPlayerCharactersByRound()
    {
        if (gameRounds % 2 == 0)
        {
            playerOneCharacter = CharType.Cat;
            playerTwoCharacter = CharType.Mouse;
        }
        else
        {
            playerOneCharacter = CharType.Mouse;
            playerTwoCharacter = CharType.Cat;
        }
    }

    public GameState GetGameState()
    {
        return currentGameState;
    }


    public void OnPlayerJoined(PlayerInput newPlayer)
    {
        print("Player joined (ID: " + newPlayer.playerIndex + ")");

        GameObject newPlayerCam = null;
        
        switch (newPlayer.playerIndex)
        {
            case 0:
                newPlayerCam = GameObject.FindGameObjectWithTag("Player1Cam");
                SpawnPoint[] possiblePlayerOneSpawnPoints = spawnerList.Where(sp => sp.GetSpawnType() == (SpawnPointType)playerOneCharacter).ToArray();
                nextSpawnPosition = possiblePlayerOneSpawnPoints[Random.Range(0, possiblePlayerOneSpawnPoints.Length - 1)];
                newPlayer.GetComponent<PlayerInputHandler>().OnGetCharacterToControl(GetCharacterPrefab(playerOneCharacter), nextSpawnPosition);
                break;

            case 1:
                newPlayerCam = GameObject.FindGameObjectWithTag("Player2Cam");
                SpawnPoint[] possiblePlayerTwoSpawnPoints = spawnerList.Where(sp => sp.GetSpawnType() == (SpawnPointType)playerTwoCharacter).ToArray();
                nextSpawnPosition = possiblePlayerTwoSpawnPoints[Random.Range(0, possiblePlayerTwoSpawnPoints.Length - 1)];
                newPlayer.GetComponent<PlayerInputHandler>().OnGetCharacterToControl(GetCharacterPrefab(playerTwoCharacter), nextSpawnPosition);
                break;
        }

        if (newPlayerCam != null)
        {
            newPlayerCam.GetComponent<CinemachineVirtualCamera>().Follow = newPlayer.transform;
            newPlayerCam.GetComponent<CinemachineVirtualCamera>().LookAt = newPlayer.transform;
        }
        else
        {
            print($"Camera could not be attached to Player {newPlayer.playerIndex}");
        }
    }


    public void OnPlayerLeft(PlayerInput playerLeft)
    {

        print("Player left (ID: " + playerLeft.playerIndex + ")");
        Destroy(playerLeft);
    }
    

    public GameObject GetCharacterPrefab(CharType playerCharacter)
    {
        if (playerCharacter == CharType.Cat)
        {
            return prefabCat;
        }
        return prefabMouse;
    }

    public GameObject GetFightPrefab()
    {
        return prefabFight;
    }
    

    public SpawnPoint GetSpawnPosition()
    {
        return nextSpawnPosition;
    }


    [ContextMenu("AutoFill Spawns")]
    public void AutoFillCollectibles()
    {
        spawnerList = FindObjectsOfType<SpawnPoint>().ToList();

        if (spawnerList.Count != 0)
        {
            string spawners = "";
            foreach (var spawner in spawnerList)
            {
                spawners += spawner.name + "   ";
            }

            print($"Spawners found ({spawnerList.Count}): {spawners}");
        }
        else
        {
            print("No spawners found");
        }
    }
    

    public GameObject[] GetObjectsWithLayer(string s_layerMask)
    {
        return FindObjectsOfType<GameObject>().Where(go => go.layer == LayerMask.NameToLayer(s_layerMask)).ToArray();
    }
    public GameObject[] GetObjectsWithLayer(int i_layerMask)
    {
        return FindObjectsOfType<GameObject>().Where(go => go.layer == i_layerMask).ToArray();
    }
    
    [ContextMenu("Fix Platforms")]
    public void RepairPlatforms()
    {
        int platformsFixed = 0;
        GameObject[] platforms = GetObjectsWithLayer("Floor");
        print($"Platforms with layer name \"Floor\" (layer {LayerMask.NameToLayer("Floor")}) found: {platforms.Length}");

    
        foreach (var platform in platforms)
        {
            if (!platform.GetComponent<PlatformEffector2D>() || !platform.GetComponent<PlatformEffector2D>().enabled || !platform.GetComponent<BoxCollider2D>().usedByEffector)
            {
                if (!platform.GetComponent<PlatformEffector2D>())
                {
                    platform.AddComponent<PlatformEffector2D>();
                    print($"Added Platform Effector 2D to {platform.name}");
                }

                if (!platform.GetComponent<PlatformEffector2D>().enabled)
                {
                    platform.GetComponent<PlatformEffector2D>().enabled = true;
                    print($"Enabled Platform Effector 2D on {platform.name}");
                }
                if (!platform.GetComponent<BoxCollider2D>().usedByEffector)
                {
                    platform.GetComponent<BoxCollider2D>().usedByEffector = true;
                    print($"Fixed Effector usage on {platform.name}");
                }

                platformsFixed++;
            }
        }
        print($"Platforms fixed: {platformsFixed}");
    }
    

    public bool MouseHasWon()
    {
        if (mouseLifesLeft > 0)
        {
            mouseLifesLeft--;
            return true;
        }

        return false;
    }
    
    // TODO implement Time (fixed time) with lerp to normal time


    private void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(gameScenes.Where(s => s.name == sceneName).ToString());
    }
    
    
    // Events
    private void PassierscheinA38(GameState newState)
    {
        if (newState == CheckNewGameState(newState))
        {
            Debug.Log($"New game state validated: {newState.ToString()}");
            currentGameState = CheckNewGameState(newState);
            ExecNewGameState();
        }
        else
        {
            Debug.LogError($"Cannot switch from {currentGameState.ToString()} to {newState.ToString()}!");
        }
    }

    private void ExecNewGameState()
    {
        switch (currentGameState)
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
                break;
            case GameState.Fight:
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
    
    
    private GameState CheckNewGameState(GameState newState)
    {
        switch (currentGameState)
        {
            case GameState.MainMenu:
                if (newState == GameState.PlayerSelect || 
                    newState == GameState.Options ||
                    newState == GameState.Credits)
                    return newState;
                break;
            case GameState.PlayerSelect:
                if (newState == GameState.MainMenu ||
                    newState == GameState.LevelBegin)
                    return newState;
                break;
            case GameState.Options:
                if (newState == GameState.MainMenu)
                    return newState;
                break;
            case GameState.Credits:
                if (newState == GameState.MainMenu)
                    return newState;
                break;
            case GameState.LevelBegin:
                if (newState == GameState.LevelRunning)
                    return newState;
                break;
            case GameState.LevelRunning:
                if (newState == GameState.FightBegin ||
                    newState == GameState.Pause ||
                    newState == GameState.LevelEnd)
                    return newState;
                break;
            case GameState.FightBegin:
                if (newState == GameState.Fight)
                    return newState;
                break;
            case GameState.Fight:
                if (newState == GameState.Pause ||
                    newState == GameState.FightEnd)
                    return newState;
                break;
            case GameState.FightEnd:
                if (newState == GameState.LevelRunning ||
                    newState == GameState.LevelEnd)
                    return newState;
                break;
            case GameState.LevelEnd:
                if (newState == GameState.LevelBegin ||
                    newState == GameState.MainMenu)
                    return newState;
                break;
            case GameState.Pause:
                if (newState == GameState.LevelRunning ||
                    newState == GameState.Fight ||
                    newState == GameState.MainMenu)
                    return newState;
                break;
        }
        return GameState.MainMenu;
    }
    
    
    // Event helper methods
    private void InstantiateFightScene()
    {
        
    }


    private void OnEnable()
    {
        PlayerInputManager.instance.onPlayerJoined += OnPlayerJoined;
        PlayerInputManager.instance.onPlayerLeft += OnPlayerLeft;
        
        GameEventManager.Instance.onGameStateChanged += PassierscheinA38;
    }



    private void OnDisable()
    {
        if (PlayerInputManager.instance != null)
        {
            PlayerInputManager.instance.onPlayerJoined -= OnPlayerJoined;
            PlayerInputManager.instance.onPlayerLeft -= OnPlayerLeft;
        }
        
        GameEventManager.Instance.onGameStateChanged -= PassierscheinA38;
    }
}
    