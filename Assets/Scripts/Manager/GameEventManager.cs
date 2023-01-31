using System;
using UnityEngine;

public class GameEventManager : MonoBehaviour
{

    public static GameEventManager Instance;

    // Events
    public event Action<GameState> onGameStateChanged;
    public event Action<Vector3> onFightStarted;
    public event Action<CharType, Vector3> onFightEnded;
    public event Action mouseWinsFight;

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

    public void GameEvent_OnGameStateChanged(GameState newState)
    {
        onGameStateChanged?.Invoke(newState);
    }

    public void GameEvent_OnFightStarted(Vector3 fightStartPosition)
    {
        onFightStarted?.Invoke(fightStartPosition);
    }

    public void GameEvent_OnFightEnded(CharType characterWhoWon, Vector3 positionOfFightEnd)
    {
        onFightEnded?.Invoke(characterWhoWon, positionOfFightEnd);
    }

    public void GameEvent_MouseWinsFight()
    {
        mouseWinsFight?.Invoke();
    }
}
