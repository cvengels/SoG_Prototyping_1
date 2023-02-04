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

    [Header("Teleporters")]
    private List<Teleporter> teleporterList;
    [SerializeField] private List<Teleporter> roomTeleporters;
    [SerializeField] private List<Teleporter> mouseHoles;
    [SerializeField] private float timeShowTeleporterDestinations = 30f;

    [Header("Global Statistics")]
    [SerializeField] private CharType catCharacterType = CharType.Cat;
    [SerializeField] private CharType mouseCharacterType = CharType.Mouse;
    [SerializeField] private int gameRounds;
    [SerializeField] private CharType playerWhoWonRound;
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
        foreach (GameObject editorHelper in GameObject.FindGameObjectsWithTag("EditorOnly").ToList())
        {
            editorHelper.gameObject.GetComponent<SpriteRenderer>().enabled = false;
        }
        /*
        foreach (GameObject spawnPoint in GameObject.FindGameObjectsWithTag("SpawnPoint").ToList())
        {
            spawnPoint.gameObject.GetComponent<SpriteRenderer>().enabled = false;
        }
        */
#endif
        
        // Setup screens
        CheckForSecondScreen();
        
        // Find and sort all teleporters
        AutoFillTeleporters();
        DebugShowTeleporterDestinations(timeShowTeleporterDestinations);
        DebugShowMouseholeDestinations(timeShowTeleporterDestinations);
        
        // Find all spawn points
        AutoFillSpawns();

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
            catCharacterType = CharType.Cat;
            mouseCharacterType = CharType.Mouse;
        }
        else
        {
            catCharacterType = CharType.Mouse;
            mouseCharacterType = CharType.Cat;
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
                SpawnPoint[] possiblePlayerOneSpawnPoints = spawnerList.Where(sp => sp.GetSpawnType() == (SpawnPointType)catCharacterType).ToArray();
                nextSpawnPosition = possiblePlayerOneSpawnPoints[Random.Range(0, possiblePlayerOneSpawnPoints.Length - 1)];
                newPlayer.GetComponent<PlayerInputHandler>().OnGetCharacterToControl(GetCharacterPrefab(catCharacterType), nextSpawnPosition);
                break;

            case 1:
                newPlayerCam = GameObject.FindGameObjectWithTag("Player2Cam");
                SpawnPoint[] possiblePlayerTwoSpawnPoints = spawnerList.Where(sp => sp.GetSpawnType() == (SpawnPointType)mouseCharacterType).ToArray();
                nextSpawnPosition = possiblePlayerTwoSpawnPoints[Random.Range(0, possiblePlayerTwoSpawnPoints.Length - 1)];
                newPlayer.GetComponent<PlayerInputHandler>().OnGetCharacterToControl(GetCharacterPrefab(mouseCharacterType), nextSpawnPosition);
                break;
        }

        if (newPlayerCam != null)
        {
            newPlayerCam.GetComponent<CinemachineVirtualCamera>().Follow = newPlayer.transform;
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
    
    
    [ContextMenu("Room Teleporters and Mouseholes")]
    public void AutoFillTeleporters()
    {
        teleporterList = FindObjectsOfType<Teleporter>().ToList();
        teleporterList.Sort();

        if (teleporterList.Count != 0)
        {
            mouseHoles = new List<Teleporter>();
            roomTeleporters = new List<Teleporter>();
            
            string s_mouseholes = "";
            string s_roomTeleporters = "";
            
            foreach (var teleporter in teleporterList)
            {
                if (teleporter.IsMouseHole())
                {
                    s_mouseholes += teleporter.name + " ";
                    mouseHoles.Add(teleporter);
                }
                else
                {
                    s_roomTeleporters += teleporter.name + " ";
                    roomTeleporters.Add(teleporter);
                }
            }
            
            // TODO fix circle sorter for mouse holes
            /*
            List<GameObject> roomList = GameObject.FindGameObjectsWithTag("Room").ToList();
            Vector2 centerPointOfAllRooms = Vector2.zero;
            foreach (var room in roomList)
            {
                centerPointOfAllRooms += (Vector2)room.transform.position;
            }
            centerPointOfAllRooms /= roomList.Count;
            
            mouseHoles = SortGameObjectsInCircle(centerPointOfAllRooms, mouseHoles);
            */
            
            print($"MouseHoles found ({mouseHoles.Count}): {s_mouseholes}");
            print($"Teleporters found ({roomTeleporters.Count}): {s_roomTeleporters}");

        }
        else
        {
            print("No teleporters found");
        }
    }

    private List<Teleporter> SortGameObjectsInCircle(Vector2 origin, List<Teleporter> originalList)
    {
        if (originalList.Count > 0)
        {
            List<Teleporter> copyOfOriginalList = new List<Teleporter>(originalList);
            List<Teleporter> circleSortedList = new List<Teleporter>();
            float angle = 0;
            int raysToShoot = 720;
            
            for (int i=0; i<raysToShoot; i++) {
                float x = Mathf.Sin (angle);
                float y = Mathf.Cos (angle);
                angle += 2 * Mathf.PI / raysToShoot;
                
                Vector3 dir = new Vector3 (origin.x + x * 1000, origin.y + y * 1000, 0);
                RaycastHit2D hit = default;
                //Debug.DrawLine (origin, dir, Color.gray, timeShowTeleporterDestinations);
                if (Physics2D.Raycast(origin, dir, Mathf.Infinity, LayerMask.GetMask("InteractiveObjects"))) {
                    if (hit != default && hit.transform.CompareTag(("Hole")))
                    {
                        print($"found GameObject {hit.transform.name}, added to circle sorted list");
                        circleSortedList.Add(hit.transform.GetComponent<Teleporter>());
                    }
                }
            }

            return circleSortedList;
        }

        return null;
    }

    private void DebugShowTeleporterDestinations(float showLinesDuration)
    {
        List<GameObject> roomList = GameObject.FindGameObjectsWithTag("Room").ToList();

        if (roomList.Count > 0)
        {
            foreach (var teleporter in roomTeleporters)
            {
                string s_teleporterDestinationName = teleporter.GetTeleporterDestination().ToString();
                GameObject teleporterDestination = roomList.Where(t => t.name == s_teleporterDestinationName).SingleOrDefault();
                    if (teleporterDestination != null)
                    {
                        /*print($"Drawing debug line from {teleporter.name}{(Vector2)teleporter.transform.position} " +
                              $"to room {teleporterDestination.name}{(Vector2)teleporterDestination.transform.position}"); */
                        Debug.DrawLine(teleporter.transform.position, teleporterDestination.transform.position, Color.green, showLinesDuration);
                    }
                    else
                    {
                        Debug.LogError($"Teleporter {teleporter.name} cannot be connected to {s_teleporterDestinationName}");
                    }
            }
        }
        else
        {
            print("No rooms found");
        }
    }

    private void DebugShowMouseholeDestinations(float showLinesDuration)
    {
        if (mouseHoles.Count > 1)
        {
            Teleporter[] mouseHoleArray = mouseHoles.ToArray();
            
            for (int i = 0; i < mouseHoleArray.Length - 1; i++)
            {
                Debug.DrawLine(mouseHoleArray[i].transform.position, mouseHoleArray[i+1].transform.position, Color.cyan, showLinesDuration);
            }
            Debug.DrawLine(mouseHoleArray[mouseHoleArray.Length - 1].transform.position, mouseHoleArray[0].transform.position, Color.cyan, showLinesDuration);
        }
    }


    [ContextMenu("AutoFill Spawns")]
    public void AutoFillSpawns()
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
            return true;
        }

        return false;
    }
    
    // TODO implement Time (fixed time) with lerp to normal time
    
    
    
    // TODO Scene "Fade In" triggers level running and player input enabled (also for room change)
    // TODO Scene "Fade Out" for new scene load enabled and disable player control at beginning of fade
    
    private void PrepareLoadingScene()
    {
        
    }
    
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
                GameEventManager.Instance.GameEvent_OnGameStateChanged(GameState.LevelRunning);
                break;
            case GameState.LevelRunning:
                // find Fight object after battle
                GameObject fightObject = FindObjectOfType<FightManager>().gameObject;
                if (fightObject != null)
                {
                    Destroy(fightObject);
                    PlayerInput[] players = FindObjectsOfType<PlayerInput>();
                    string playerCamName = "";
                    foreach (var player in players)
                    {
                        if (player.playerIndex == 0)
                        {
                            playerCamName = "Player1Cam";
                        }
                        else
                        {
                            playerCamName = "Player2Cam";
                        }
                    GameObject.FindGameObjectWithTag(playerCamName).
                        GetComponent<CinemachineVirtualCamera>().
                        Follow = player.transform;
                    }
                }
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
                    newState == GameState.FightEnd ||
                    newState == GameState.LevelEnd)
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


    private void CatWinsFight()
    {
        CatWinsRound();
    }

    private void CatWinsRound()
    {
        GameEventManager.Instance.GameEvent_OnGameStateChanged(GameState.LevelEnd);
    }


    private void OnEnable()
    {
        PlayerInputManager.instance.onPlayerJoined += OnPlayerJoined;
        PlayerInputManager.instance.onPlayerLeft += OnPlayerLeft;
        
        GameEventManager.Instance.onGameStateChanged += PassierscheinA38;
        GameEventManager.Instance.mouseWinsFight += DecrementMouseLifes;
        GameEventManager.Instance.catWinsFight += CatWinsFight;
    }

    private void DecrementMouseLifes()
    {
        if (!MouseHasWon())
        {
            print("Mouse has no more lifes");
            GameEventManager.Instance.GameEvent_OnGameStateChanged(GameState.LevelEnd);
        }
        else
        {
            mouseLifesLeft--;
            print($"mouse has {mouseLifesLeft} lifes left");
        }
    }


    private void OnDisable()
    {
        if (PlayerInputManager.instance != null)
        {
            PlayerInputManager.instance.onPlayerJoined -= OnPlayerJoined;
            PlayerInputManager.instance.onPlayerLeft -= OnPlayerLeft;
        }
        
        GameEventManager.Instance.onGameStateChanged -= PassierscheinA38;
        GameEventManager.Instance.mouseWinsFight -= DecrementMouseLifes;
        GameEventManager.Instance.catWinsFight -= CatWinsFight;
    }
}
    