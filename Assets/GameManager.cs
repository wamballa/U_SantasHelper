using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;
    //private bool isInitialized = false;

    public enum GameState
    {
        New,
        Ongoing,
        TruckPacked,
        GameOver
    }
    public GameState currentState = GameState.New;

    public bool DebugMode;

    public int Lives { get; private set; }

    private int highScore;
    public int Score { get; private set; }
    public int CakesPacked { get; set; }
    public int CurrentLevel { get; set; }
    public int CakeDropPauseTimer { get; set; } = 3;

    // Bools
    public static bool CanPackTruck { get; set; }
    //public bool isTruckPacked;
    //private bool isGameOver;

    [Header("UI")]
    private TMP_Text livesText;
    private TMP_Text packedCounterText;
    private TMP_Text levelText;
    public TMP_Text scoreText;

    // Events
    public event EventHandler OnLevelComplete;
    public event EventHandler OnLevelStart;
    public event EventHandler OnTruckPacked;
    public event EventHandler OnCakeDropped;
    public event EventHandler OnPauseGamePlay;
    public event EventHandler OnResumeGamePlay;
    public event EventHandler OnGameOver;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Debug.Log("> GameManager > Awake: Destroy duplicate instance");
            Destroy(this.gameObject);
            return;
        }

        Debug.Log("> GameManager > Awake: Create singleton instance");
        instance = this;
        DontDestroyOnLoad(gameObject);

        ResetGameManager(); // Ensure a clean slate at the start
    }

    private void ResetGameManager()
    {
        Debug.Log("> GameManager > ResetGameManager: Resetting game variables");
        Score = 0;
        Lives = 1;
        CakesPacked = 0;
        CanPackTruck = true;
        currentState = GameState.New;
        CurrentLevel = 0;
        UpdateUI();
    }

    private void InitializeLevel()
    {
        Debug.Log("> GameManager > InitializeLevel: Setting up level variables");
        currentState = GameState.Ongoing;
        CakesPacked = 0;
        CanPackTruck = true;
        UpdateUI();
        OnLevelStart?.Invoke(this, EventArgs.Empty);
    }


    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log($"> GameManager > OnSceneLoaded: {scene.name}");

        if (scene.name == "Menu")
        {
            ResetGameManager(); // Reset global game variables for new game prep
        }
        else if (scene.name == "GameLevel")
        {
            InitializeLevel(); // Set up for the active level
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("> " + nameof(GameManager) + " > Start ");
        OnLevelStart?.Invoke(this, EventArgs.Empty);
        UpdateUI();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateUI();
        CheckIfLivesLeft();
    }

    // Checks if there are any lives left
    private void CheckIfLivesLeft()
    {
        if (DebugMode) return;

        if (Lives <= 0 && currentState != GameState.GameOver )
        {
            Debug.Log(GetSceneName() + "CheckIfLivesLeft ==============================");
            currentState = GameState.GameOver;
            GameUtilities.SaveHighScore(CakesPacked);
            OnGameOver?.Invoke(this, EventArgs.Empty);
            StartCoroutine(HandleGameOver());
        }
    }

    // Handles the game over process
    private IEnumerator HandleGameOver()
    {
        yield return new WaitForSeconds(3);
        Debug.Log(GetSceneName() + nameof(GameManager) + ": HandleGameOver. Level = 0.");
        CurrentLevel = 0;
        SceneManager.LoadScene("GameOver");
    }

    // Handles the event when a cake is dropped
    public void HandleCakeDroppedEvent()
    {
        OnPauseGamePlay?.Invoke(this, EventArgs.Empty);
    }

    // Handles the event when a mouse eats a cake
    public void HandleMouseEatsCakeEvent()
    {
        OnResumeGamePlay?.Invoke(this, EventArgs.Empty);
        Lives--;
    }

    // Restarts the current level
    //public void RestartLevel()
    //{
    //    CurrentLevel++;
    //    //Debug.Log("================================================");
    //    Debug.Log("> GAMEMANAGER > RestartLevel: " + CurrentLevel);
    //    ResetLevelData();
    //    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex, LoadSceneMode.Single);
    //}

    // Resets the level data to default values
    //private void ResetLevelData()
    //{
    //    CakesPacked = 0;
    //    CanPackTruck = true;
    //}

    // Updates the UI elements with the current game state
    void UpdateUI()
    {
        if (SceneManager.GetActiveScene().name != "Menu" 
            && SceneManager.GetActiveScene().name != "GameOver")
        {
            //GetLivesText().text = Lives.ToString();
            GetTextObject("Lives Value").text = Lives.ToString();
            //GetLevelText().text = CurrentLevel.ToString();
            GetTextObject("Packed Value").text = CakesPacked.ToString();
            //GetTextObject("ScoreText").text = Score.ToString();
        }
    }



    // Adds a packed cake and updates the state accordingly
    public void AddPackedCakes()
    {
        if (CanPackTruck)
        {
            CakesPacked++;
            Score++;

            if (CakesPacked == 8)
            {
                Debug.Log("> GameManager > AddPackedCakes: Truck is now packed @@@@@@@@@");
                CanPackTruck = false;
                OnLevelComplete?.Invoke(this, EventArgs.Empty);

                GameObject[] gameObjects = GameObject.FindGameObjectsWithTag("Cake");
                if (gameObjects.Length > 0)
                {
                    foreach (var gameObject in gameObjects)
                    {
                        Destroy(gameObject);
                    }
                }
            }
        }
    }

    TMP_Text GetTextObject(string name)
    {
        GameObject go = GameObject.Find(name);
        if (go != null)
        {
            return go.GetComponent<TMP_Text>();
        }
        else return null;
    }

    // Gets the level text UI element
    TMP_Text GetLevelText()
    {
        if (levelText == null)
        {
            GameObject go = GameObject.Find("LevelNumberText");
            if (go != null)
            {
                levelText = go.GetComponent<TMP_Text>();
            }
        }
        return levelText;
    }

    // Gets the lives text UI element
    TMP_Text GetLivesText()
    {
        if (livesText == null)
        {
            GameObject go = GameObject.Find("Lives Value");
            if (go != null)
            {
                livesText = go.GetComponent<TMP_Text>();
            }
        }
        return livesText;
    }

    // Gets the packed cakes text UI element
    TMP_Text GetPackedCakesText()
    {
        if (packedCounterText == null)
        {
            GameObject go = GameObject.Find("PackedCakeText");
            if (go != null)
            {
                packedCounterText = go.GetComponent<TMP_Text>();
            }
            else
            {
                Debug.LogError("GAMEMANAGER: No PackedCakeText text found");
            }
        }
        return packedCounterText;
    }

    // Gets the name of the active scene
    string GetSceneName()
    {
        return SceneManager.GetActiveScene().name + ": ";
    }
}
