using System;
using System.Threading;
using UnityEngine;
using UnityEngine.InputSystem;
using Random = UnityEngine.Random;

public class FightManager : MonoBehaviour
{
    public static FightManager Instance;

    [SerializeField] private float timeForMouseToInteract = 10f;
    [SerializeField] private int buttonPressesNeededTotal = 100;
    [SerializeField] private int buttonPressesDone;
    [SerializeField] private bool fightIsDecided;

    private Animator fightAnimator;

    private FightStatusGUI fightIndicator;
    
    
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Debug.LogWarning("Fight Manager already spawned!");
            Destroy(this);
        }
        else
        {
            Debug.Log("Fight Manager spawned");
            Instance = this;
        }

        if (buttonPressesNeededTotal % 2 != 0)
        {
            buttonPressesNeededTotal++;
        }

        buttonPressesDone = buttonPressesNeededTotal / 2;

        fightIsDecided = false;
        fightAnimator = GetComponent<Animator>();
        fightIndicator = GetComponentInChildren<FightStatusGUI>();
    }

    private void OnCollisionEnter2D()
    {
        Vector2 playerVelocity = GetComponent<Rigidbody2D>().velocity;
        
        playerVelocity = new Vector2(
            Random.Range(-10f, 10f),
            playerVelocity.y
            );

        if (playerVelocity.x < 0.5f)
        {
            playerVelocity = (Vector2.right * Random.Range(-1, 0) + Vector2.up) * 50f ;
        }
    }

    
    // Animation functions
    private void OnFightInitFinished()
    {
        GameEventManager.Instance.GameEvent_OnGameStateChanged(GameState.Fight);
        fightAnimator.Play("Fight_Main");
    }

    private void OnIsFightEnding()
    {
        if (fightIsDecided)
        {
            fightAnimator.Play("Fight_End");
        }
    }

    private void OnFightDone()
    {
        GameEventManager.Instance.GameEvent_OnGameStateChanged(GameState.LevelRunning);
    }

    
    

    public void SetMovement(Vector2 movementDirection, CharType characterType)
    {
        // Nothing here
    }
    
    public void SetAction(bool contextPerformed, CharType characterType)
    {
        if (contextPerformed && GameManager.Instance.GetGameState() == GameState.Fight)
        {
            print($"{characterType.ToString()} punches really hard!");
            if (characterType == CharType.Mouse)
            {
                buttonPressesDone++;
            }
            else if (characterType == CharType.Cat)
            {
                buttonPressesDone--;
            }
            
            fightIndicator.AdjustIndicator(characterType, buttonPressesNeededTotal, buttonPressesDone);
            
            print($"Button Press status: {buttonPressesDone}");

            if (buttonPressesDone <= 0 || buttonPressesDone >= buttonPressesNeededTotal)
            {
                print("Fight is over, checking who won ...");
                GameEventManager.Instance.GameEvent_OnGameStateChanged(GameState.FightEnd);

                if (buttonPressesDone >= buttonPressesNeededTotal)
                {
                    GameEventManager.Instance.GameEvent_MouseWinsFight();
                }
                fightIsDecided = true;
            }
        }
    }
}
