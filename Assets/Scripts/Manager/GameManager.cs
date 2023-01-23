using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    private SpawnPoint nextSpawnPosition;
    private PlayerInputManager playerInputManager;

    [Header("Character Prefabs")] [SerializeField]
    private GameObject prefabCat;

    [SerializeField] private GameObject prefabMouse;
    [SerializeField] private GameObject prefabFight;

    [Header("Interactive Objects")] [SerializeField]
    private GameObject prefabCheese;

    [Header("Spawn Points")] [SerializeField]
    private List<SpawnPoint> spawnerList;

    [Header("Global Statistics")] [SerializeField]
    private int gameRounds;


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


    public void OnPlayerJoined(PlayerInput newPlayer)
    {
        print("Player joined (ID: " + newPlayer.playerIndex + ")");

        GameObject newPlayerCam = null;
        
        switch (newPlayer.playerIndex)
        {
            case 0:
                newPlayerCam = GameObject.FindGameObjectWithTag("Player1Cam");
                SpawnPoint[] catSpawnPoints = spawnerList.Where(sp => sp.GetSpawnType() == SpawnPointType.Cat).ToArray();
                nextSpawnPosition = catSpawnPoints[Random.Range(0, catSpawnPoints.Length - 1)];
                newPlayer.GetComponent<PlayerInputHandler>().OnGetCharacterToControl(GetCharacterPrefab(), nextSpawnPosition);
                break;

            case 1:
                newPlayerCam = GameObject.FindGameObjectWithTag("Player2Cam");
                SpawnPoint[] mouseSpawnPoints = spawnerList.Where(sp => sp.GetSpawnType() == SpawnPointType.Mouse).ToArray();
                nextSpawnPosition = mouseSpawnPoints[Random.Range(0, mouseSpawnPoints.Length - 1)];
                newPlayer.GetComponent<PlayerInputHandler>().OnGetCharacterToControl(GetCharacterPrefab(), nextSpawnPosition);
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

    public GameObject GetCharacterPrefab()
    {
        if (playerInputManager.playerCount == 2)
        {
            return prefabMouse;
        }

        return prefabCat;
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
    