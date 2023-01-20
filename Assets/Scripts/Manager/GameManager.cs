using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    private GameObject nextPrefab;
    private Transform nextPosition;

    [Header("Character Prefabs")] 
    [SerializeField] private GameObject prefabCat;
    [SerializeField] private GameObject prefabMouse;
    [SerializeField] private GameObject prefabFight;
    
    public GameObject[] spawnpointsCat, spawnpointsMouse;

    [Header("Interactive Objects")] 
    [SerializeField] private GameObject prefabCheese;

    [SerializeField] private List<CheeseSpawnPoint> cheeseSpawner;

    [Header("Global Statistics")]
    [SerializeField] private int gameRounds;

    public static UnityEvent<CheeseSpawnPoint> CheeseSp;
    
    private Camera mainCamera, firstPlayer, secondPlayer;
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
            nextPrefab = prefabCat;
            nextPosition = spawnpointsMouse[0].transform;
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
        return nextPosition.position;
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