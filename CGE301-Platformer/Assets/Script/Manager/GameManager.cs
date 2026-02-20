using UnityEngine;
using System.Reflection;

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

    [Header("UI State Objects")]
    [SerializeField] private Transform uiRoot;
    [SerializeField] private GameObject startUI;
    [SerializeField] private GameObject playingUI;
    [SerializeField] private GameObject winUI;
    [SerializeField] private GameObject playingTimeUI;
    [SerializeField] private GameObject winTimeUI;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        Time.timeScale = 1f;
        CacheUIReferences();
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
            UpdateTimeLabels();
            return;
        }

        if (currentState == GameState.Win)
        {
            UpdateTimeLabels();
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
        UpdateTimeLabels();
    }

    private void SetState(GameState newState)
    {
        currentState = newState;
        UpdateUIByState();
        Debug.Log("STATE = " + currentState);
    }

    public void NotifyWinItemCollected()
    {
        if (currentState != GameState.Playing) return;
        Win();
    }

    private string FormatTime(float t)
    {
        int sec = Mathf.Max(0, Mathf.FloorToInt(t));
        int m = sec / 60;
        int s = sec % 60;
        return $"{m}:{s:00}";
    }

    private void CacheUIReferences()
    {
        if (uiRoot == null)
        {
            GameObject root = GameObject.Find("UI");
            if (root != null)
            {
                uiRoot = root.transform;
            }
        }

        if (uiRoot == null)
        {
            return;
        }

        if (startUI == null)
        {
            Transform t = uiRoot.Find("Start");
            if (t != null) startUI = t.gameObject;
        }

        if (playingUI == null)
        {
            Transform t = uiRoot.Find("Playing");
            if (t != null) playingUI = t.gameObject;
        }

        if (winUI == null)
        {
            Transform t = uiRoot.Find("Win");
            if (t != null) winUI = t.gameObject;
        }

        if (playingTimeUI == null && playingUI != null)
        {
            Transform t = playingUI.transform.Find("Time");
            if (t != null) playingTimeUI = t.gameObject;
        }

        if (winTimeUI == null && winUI != null)
        {
            Transform t = winUI.transform.Find("Time");
            if (t != null) winTimeUI = t.gameObject;
        }
    }

    private void UpdateUIByState()
    {
        if (!HasSceneUI())
        {
            return;
        }

        if (startUI != null) startUI.SetActive(currentState == GameState.Menu);
        if (playingUI != null) playingUI.SetActive(currentState == GameState.Playing);
        if (winUI != null) winUI.SetActive(currentState == GameState.Win);
        UpdateTimeLabels();
    }

    private bool HasSceneUI()
    {
        return startUI != null || playingUI != null || winUI != null;
    }

    private void UpdateTimeLabels()
    {
        string timeValue = "Time: " + FormatTime(timePlayed);
        SetTextOnObject(playingTimeUI, timeValue);
        SetTextOnObject(winTimeUI, timeValue);
    }

    private void SetTextOnObject(GameObject target, string value)
    {
        if (target == null)
        {
            return;
        }

        Component[] components = target.GetComponents<Component>();
        for (int i = 0; i < components.Length; i++)
        {
            Component component = components[i];
            if (component == null)
            {
                continue;
            }

            PropertyInfo textProperty = component.GetType().GetProperty("text");
            if (textProperty == null || !textProperty.CanWrite || textProperty.PropertyType != typeof(string))
            {
                continue;
            }

            textProperty.SetValue(component, value);
            return;
        }
    }
}
