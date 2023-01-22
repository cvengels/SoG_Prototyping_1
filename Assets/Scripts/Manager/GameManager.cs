using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using UnityEngine;
using UnityEngine.InputSystem;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    private Thread checkDisplays;
    private int displaysFound;
    public event Action<int> DisplaysChanged;
    
    private SpawnPoint nextSpawnPosition;
    private PlayerInputManager playerInputManager;

    [Header("Character Prefabs")]
    [SerializeField] private GameObject prefabCat;
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
        
        // Initial display configuration
        ChangeDisplayConfiguration(Display.displays.Length < 2);
        
        // Start display control thread
        checkDisplays = new Thread(SearchForDisplayChange);

        // Find player input manager
        playerInputManager = FindObjectOfType<PlayerInputManager>();

        // Find all spawn points
        AutoFillCollectibles();
    }

    private void Start()
    {
        checkDisplays.Start();
    }


    public void OnPlayerJoined(PlayerInput newPlayer)
    {
        print("Player joined (ID: " + newPlayer.playerIndex + ")");

        if (playerInputManager.playerCount == 1)
        {
            Camera.main.enabled = false;
            newPlayer.GetComponent<Camera>().targetDisplay = 0;
        }

        if (playerInputManager.playerCount == 2)
        {
            newPlayer.GetComponent<Camera>().targetDisplay = 1;
        }

        switch (newPlayer.playerIndex)
        {
            case 0:
                SpawnPoint[] catSpawnPoints = spawnerList.Where(sp => sp.GetSpawnType() == SpawnPointType.Cat).ToArray();
            nextSpawnPosition = catSpawnPoints[Random.Range(0, catSpawnPoints.Length -1)];
                newPlayer.GetComponent<PlayerInputHandler>().OnGetCharacterToControl(GetCharacterPrefab(), nextSpawnPosition);
            break;
            
            case 1:
                SpawnPoint[] mouseSpawnPoints = spawnerList.Where(sp => sp.GetSpawnType() == SpawnPointType.Mouse).ToArray();
            nextSpawnPosition = mouseSpawnPoints[Random.Range(0, mouseSpawnPoints.Length -1)];
                newPlayer.GetComponent<PlayerInputHandler>().OnGetCharacterToControl(GetCharacterPrefab(), nextSpawnPosition);
            break;
        }
    }

    
    public void OnPlayerLeft(PlayerInput playerLeft)
    {
        print("Player left (ID: " + playerLeft.playerIndex + ")");
        Destroy(playerLeft);
        
        if (playerInputManager.playerCount == 0)
        {
            Camera.main.enabled = true;
        }
        
    }
    
    public GameObject GetCharacterPrefab()
    {
        if (playerInputManager.playerCount == 2)
        {
            return prefabCat;
        }
        
        return prefabMouse;
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
    
    
    private void SearchForDisplayChange()
    {
        Thread.Sleep(1000);
        
        // Search for new Displays
        if (Display.displays.Length != displaysFound)
        {
            displaysFound = Display.displays.Length;
            if (displaysFound == 2)
            {
                Display.displays[1].Activate();
                ChangeDisplayConfiguration(false);
            }
            else
            {
                ChangeDisplayConfiguration(true);
            }
        }
    }

    private void ChangeDisplayConfiguration(bool useSplitscreen)
    {
        playerInputManager.splitScreen = useSplitscreen;
    }
    
}