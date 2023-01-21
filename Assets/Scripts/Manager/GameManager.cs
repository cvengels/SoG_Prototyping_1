using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    private GameObject nextPrefab;
    private Transform nextSpawnPosition;

    [Header("Character Prefabs")] 
    [SerializeField] private GameObject prefabCat;
    [SerializeField] private GameObject prefabMouse;
    [SerializeField] private GameObject prefabFight;

    [Header("Interactive Objects")] 
    [SerializeField] private GameObject prefabCheese;
    
    [Header("Spawn Points")]
    [SerializeField] private List<SpawnPoint> spawnerList;

    [Header("Global Statistics")]
    [SerializeField] private int gameRounds;
    
    public static UnityEvent<Camera> AddPlayerCamera;
    
    
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

        // Find all spawn points
        AutoFillCollectibles();
    }
        
    public void OnPlayerJoined(PlayerInput newPlayer)
    {
        print("Player joined (ID: " + newPlayer.playerIndex + ")");
        
        
        switch (newPlayer.playerIndex)
        {
            case 0:
            nextPrefab = prefabCat;
            SpawnPoint[] catSpawnPoints = spawnerList.Where(sp => sp.GetSpawnType() == SpawnPointType.Cat).ToArray();
            nextSpawnPosition = catSpawnPoints[Random.Range(0, catSpawnPoints.Length -1)].transform;
            Instantiate(nextPrefab, nextSpawnPosition.position, Quaternion.identity);
            break;
            
            case 1:
            nextPrefab = prefabMouse;
            SpawnPoint[] mouseSpawnPoints = spawnerList.Where(sp => sp.GetSpawnType() == SpawnPointType.Mouse).ToArray();
            nextSpawnPosition = mouseSpawnPoints[Random.Range(0, mouseSpawnPoints.Length -1)].transform;
            Instantiate(nextPrefab, nextSpawnPosition.position, Quaternion.identity);
            break;
        }
    }

    public GameObject GetCharacterPrefab()
    {
        return nextPrefab;
    }
    
    public void OnPlayerLeft(PlayerInput playerLeft)
    {
        print("Player left (ID: " + playerLeft.playerIndex + ")");
        Destroy(playerLeft);
    }

    public Vector2 GetSpawnPosition()
    {
        return nextSpawnPosition.position;
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
            print("Spawner list: " + spawners);
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