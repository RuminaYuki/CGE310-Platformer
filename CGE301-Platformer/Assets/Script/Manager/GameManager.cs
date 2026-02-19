using UnityEngine;

public enum GameState
{
    Menu,
    Playing,
    Win
}

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("State")]
    [SerializeField] private GameState currentState;

    [Header("Runtime")]
    [SerializeField] private float timePlayed;

    [Header("Item Reference (single item)")]
    [SerializeField] private GameObject winItem;

    [Header("Enable Plyaer")]
    [SerializeField] private PlayerController playerController;

    [Header("Debug")]
    [SerializeField] private bool showGUI = true;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        Time.timeScale = 1f;
    }

    private void Start()
    {
        if (playerController != null)
        {
            playerController.enabled = false;
        }

        GoToMenu();
    }

    private void Update()
    {
        if (currentState == GameState.Menu)
        {
            if (PressedStart())
            {
                if (playerController != null)
                {
                    playerController.enabled = true;
                }

                StartGame();
            }

            return;
        }

        if (currentState == GameState.Playing)
        {
            timePlayed += Time.deltaTime;
            return;
        }

        if (currentState == GameState.Win)
        {
            if (PressedStart())
            {
                GoToMenu();
            }
        }
    }

    private bool PressedStart()
    {
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
            return true;

        if (Input.GetMouseButtonDown(0)) return true;
        if (Input.GetKeyDown(KeyCode.Space)) return true;

        return false;
    }

    private void GoToMenu()
    {
        Time.timeScale = 1f;
        SetState(GameState.Menu);
        ResetRun();

        if (playerController != null)
        {
            playerController.enabled = false;
        }

        if (winItem != null)
        {
            winItem.SetActive(true);
        }
    }

    private void StartGame()
    {
        Time.timeScale = 1f;
        ResetRun();
        SetState(GameState.Playing);

        if (winItem != null)
        {
            winItem.SetActive(true);
        }
        else
        {
            Debug.LogWarning("[GameManager] winItem is not assigned.");
        }
    }

    private void Win()
    {
        Time.timeScale = 0f;

        if (playerController != null)
        {
            playerController.enabled = false;
        }

        SetState(GameState.Win);
        Debug.Log("YOU WIN! Time = " + Mathf.FloorToInt(timePlayed));
    }

    private void ResetRun()
    {
        timePlayed = 0f;
    }

    private void SetState(GameState newState)
    {
        currentState = newState;
        Debug.Log("STATE = " + currentState);
    }

    public void NotifyWinItemCollected()
    {
        if (currentState != GameState.Playing) return;
        Win();
    }

    private void OnGUI()
    {
        if (!showGUI) return;

        float w = Screen.width;
        float h = Screen.height;

        GUIStyle center = new GUIStyle(GUI.skin.label)
        {
            fontSize = Mathf.RoundToInt(Mathf.Min(w, h) * 0.05f),
            alignment = TextAnchor.MiddleCenter
        };

        GUIStyle hud = new GUIStyle(GUI.skin.label)
        {
            fontSize = Mathf.RoundToInt(Mathf.Min(w, h) * 0.035f),
            alignment = TextAnchor.UpperLeft
        };

        if (currentState == GameState.Menu)
        {
            GUI.Label(new Rect(0, h * 0.42f, w, 80), "Tap to Play", center);
        }

        if (currentState == GameState.Playing)
        {
            GUI.Label(new Rect(20, 20, w * 0.7f, 40), "Time: " + FormatTime(timePlayed), hud);
            GUI.Label(new Rect(20, 60, w * 0.9f, 40), "Goal: Collect the item!", hud);
        }

        if (currentState == GameState.Win)
        {
            GUI.Label(new Rect(0, h * 0.4f, w, 80), "YOU WIN!", center);
            GUI.Label(new Rect(0, h * 0.5f, w, 60), "Time: " + FormatTime(timePlayed), center);
            GUI.Label(new Rect(0, h * 0.6f, w, 60), "Tap to return to Menu", center);
        }
    }

    private string FormatTime(float t)
    {
        int sec = Mathf.Max(0, Mathf.FloorToInt(t));
        int m = sec / 60;
        int s = sec % 60;
        return $"{m}:{s:00}";
    }
}
