using System;
using System.Collections.Generic;
using System.Linq;
using Cinemachine;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    private SpawnPoint nextSpawnPosition;
    private PlayerInputManager playerInputManager;

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
        
        // Setup screens
        CheckForSecondScreen();
        
        // Find player input manager
        playerInputManager = FindObjectOfType<PlayerInputManager>();

        // Find all spawn points
        AutoFillCollectibles();

        SetPlayerCharactersByRound();

        currentGameState = GameState.LevelRunning;
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
    

    public event Action<GameState> onGameStateChanged;
    
    public GameState GetGameState()
    {
        return currentGameState;
    }
    

    // TODO implement SetPlayerCharactersByRound() outcome into player spawn
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
    

    public SpawnPoint GetSpawnPosition()
    {
        return nextSpawnPosition;
    }


    public void CheckIfPlayerIsInVoid()
    {
        if (PlayerInput.all.Count > 0)
        {
            foreach (var player in PlayerInput.all)
            {
                if (player.transform.position.y < 10000f)
                {
                    CharType playerCharacter = player.GetComponentInChildren<PlayerIndividualBehavior>().GetPrefabType();
                    SpawnPoint[] spawnPoints = spawnerList.Where(sp => (int)sp.GetSpawnType() == (int)playerCharacter).ToArray();
                    
                    player.transform.position = new Vector2();
                }
            }
        }
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


    private void OnEnable()
    {
        if (PlayerInputManager.instance != null)
        {
            PlayerInputManager.instance.onPlayerJoined += OnPlayerJoined;
            PlayerInputManager.instance.onPlayerLeft += OnPlayerLeft;
        }
    }

    
    private void OnDisable()
    {
        if (PlayerInputManager.instance != null)
        {
            PlayerInputManager.instance.onPlayerJoined -= OnPlayerJoined;
            PlayerInputManager.instance.onPlayerLeft -= OnPlayerLeft;
        }
    }
}
    