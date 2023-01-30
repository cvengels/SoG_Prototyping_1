using System;
using UnityEngine;

public class GameEventManager : MonoBehaviour
{
<<<<<<< Updated upstream

    public static GameEventManager Instance;
    
    // Events
    public event Action<GameState> OnGameStateChanged;
    public event Action<Vector3> OnFightStarted;
    public event Action<CharType, Vector3> OnFightEnded;
    
=======
    public static GameEventManager Instance;
>>>>>>> Stashed changes

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

<<<<<<< Updated upstream

    public void GameEvent_OnGameStateChanged(GameState newState)
    {
        OnGameStateChanged?.Invoke(newState);
    }

    public void GameEvent_OnFightStarted(Vector3 fightStartPosition)
    {
        OnFightStarted?.Invoke(fightStartPosition);
    }

    protected virtual void OnOnFightEnded(CharType characterWhoWon, Vector3 positionOfFightEnd)
    {
        OnFightEnded?.Invoke(characterWhoWon, positionOfFightEnd);
=======
    public event Action<GameState> onGameStateChanged;
    public void OnGameStateChanged(GameState newState)
    {
        onGameStateChanged?.Invoke(newState);
>>>>>>> Stashed changes
    }
}
