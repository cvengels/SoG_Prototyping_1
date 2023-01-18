using System;
using UnityEngine;
using UnityEngine.InputSystem;

public enum AnimationType
{
    Idle,
    Walk,
    Run,
    Jump,
    Fall,
    Wall,
    Interact
}

public enum CharType
{
    Cat = 0,
    Mouse = 1
}

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    private GameObject nextPrefab;
    private Transform nextPosition;

    [Header("Players")]
    public GameObject prefabCat, prefabMouse;
    
    public GameObject[] spawnpointsCat, spawnpointsMouse;

    [Header("Interactive Objects")]
    public GameObject cheesePrefab;
    public GameObject[] spawnpointsCheese;

    [Header("")]
    [SerializeField] private int gameRounds;

    
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
    }

    
    public void OnPlayerJoined(PlayerInput newPlayer)
    {
        print("Player joined (ID: " + newPlayer.playerIndex + ")");
        switch (newPlayer.playerIndex)
        {
            case 0:
            nextPrefab = prefabCat;
            nextPosition = spawnpointsCat[0].transform;
            break;
            
            case 1:
            nextPrefab = prefabMouse;
            nextPosition = spawnpointsMouse[0].transform;
            break;
        }
    }

    public GameObject GetPrefabFromGM()
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
        return nextPosition.position;
    }

    public GameObject GetCatPrefab()
    {
        return prefabCat;
    }

    public GameObject GetMousePrefab()
    {
        return prefabMouse;
    }

    public GameObject GetCheesePrefab()
    {
        return cheesePrefab;
    }

    public GameObject[] GetSpawnsCheese()
    {
        return spawnpointsCheese;
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