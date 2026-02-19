using UnityEngine;

public enum GameState
{
    Menu,
    Playing,
    Win
}


public class GameManager : MonoBehaviour
{
    [SerializeField] private GameState currentState;

    private float timePlayed;
    private float debugTimer;

    private void Start()
    {
        SetState(GameState.Menu);
        ResetRun();
    }

    private void Update()
    {
        if (currentState == GameState.Menu)
        {
            if (PressedStart())
                StartGame();

            return;
        }

        // ===== Playing =====
        timePlayed += Time.deltaTime;

        // แสดงเวลาใน Console ทุก 1 วิ (กัน spam)
        debugTimer += Time.deltaTime;
        if (debugTimer >= 1f)
        {
            debugTimer = 0f;
            Debug.Log("Time: " + Mathf.FloorToInt(timePlayed));
        }
    }

    private bool PressedStart()
    {
        // Mobile
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
            return true;

        // PC
        if (Input.GetMouseButtonDown(0)) return true;
        if (Input.GetKeyDown(KeyCode.Space)) return true;

        return false;
    }

    private void StartGame()
    {
        ResetRun();
        SetState(GameState.Playing);
    }

    private void ResetRun()
    {
        timePlayed = 0f;
        debugTimer = 0f;
    }

    private void SetState(GameState newState)
    {
        currentState = newState;
        Debug.Log("STATE = " + currentState);
    }
}


