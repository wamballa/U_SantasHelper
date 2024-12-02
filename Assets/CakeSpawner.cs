using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
//using MoreMountains.FeedbacksForThirdParty;
using System.Linq;
using UnityEngine.PlayerLoop;
using System.Runtime.CompilerServices;
using System.Threading;

public class CakeSpawner : MonoBehaviour
{
    // Debug
    public bool _debugMode = false;

    // Feeder
    public Transform feederTransform;
    private int spawnNumber;
    GameObject cake;

    private Dictionary<int, List<float>> levelSpawnDelays;
    public List<float> currentLevelDelays;
    public int currentDelayIndex = 0;

    Coroutine spawnCoroutine = null;

    // Spawn control
    public bool isSpawnerOn;
    private float timer;
    private bool canSpawn = true;

    void Start()
    {
        _debugMode = GameObject.Find("GameManager").GetComponent<GameManager>().DebugMode;

        InitialiseSpawner();

    }
    private void Update()
    {
        SpawnCake();

        if (Input.GetKeyDown(KeyCode.Space))
        {
            isSpawnerOn = !isSpawnerOn;
        }
    }
    void InitialiseSpawner()
    {
        if (GameAssets.instance == null || GameAssets.instance.cake == null)
        {
            Debug.LogError(nameof(CakeSpawner) + "GameAssets or cake prefab not properly assigned.");
            return; // Stop further execution to prevent runtime errors
        }
        cake = GameAssets.instance.cake;
        currentDelayIndex = 0;
        currentLevelDelays = new List<float>();
        SubscribeToEvents();
        InitializeLevelSpawnDelays();

        int currentLevel = GameManager.instance.CurrentLevel;
        //print("Current level = " + currentLevel);
        if (!levelSpawnDelays.ContainsKey(currentLevel))
        {
            //Debug.Log(nameof(CakeSpawner) + $": No spawn delays configured for level {currentLevel}");
            return; // Prevent further execution
        }
        SetDelaysForLevel(currentLevel);
        StartSpawning();
    }
    private void OnTruckPacked(object sender, EventArgs e)
    {
        PauseSpawning();
    }
    private void StartSpawning()
    {
        isSpawnerOn = true;
    }

    private void ResumeSpawning()
    {
        isSpawnerOn = true;
    }

    private void PauseSpawning()
    {
        isSpawnerOn = false;
    }

    private void SpawnCake()
    {
        if (!isSpawnerOn) { return; }
        if (currentLevelDelays.Count == 0)
        {
            return; // Prevent accessing an empty list
        }
        float delay = currentLevelDelays[currentDelayIndex];
        if (_debugMode) delay = 2f;
        timer += Time.deltaTime;

        if (timer > delay)
        {
            GameObject go = Instantiate(cake, transform.position, Quaternion.identity);
            go.name = "Cake " + spawnNumber;
            spawnNumber++;
            timer = 0f; // Reset the timer after spawning a cake
            currentDelayIndex = (currentDelayIndex + 1) % currentLevelDelays.Count;
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        //Gizmos.DrawWireCube(feederTransform.position, feederTransform.localScale * 1.5f);
    }

    private void InitializeLevelSpawnDelays()
    {
        Debug.Log("> " + nameof(CakeSpawner) + " > InitializeLevelSpawnDelays");

        levelSpawnDelays = new Dictionary<int, List<float>>()
        {
            { 0, new List<float>() { 5f, 5f, 15f, 5f, 15f, 20f, 10f, 10f } },
            { 1, new List<float>() { 5f, 5f, 15f, 5f, 15f, 20f, 10f, 10f } },
            { 2, new List<float>() { 5f, 5f, 15f, 5f, 15f, 20f, 10f, 10f } },
            { 3, new List<float>() { 5f, 5f, 15f, 5f, 15f, 20f, 10f, 10f } },
            { 4, new List<float>() { 5f, 5f, 15f, 5f, 15f, 20f, 10f, 10f } },
            { 5, new List<float>() { 5f, 5f, 15f, 5f, 15f, 20f, 10f, 10f } },
            // Add more levels with their respective delays
        };

    }

    private void SetDelaysForLevel(int level)
    {
        Debug.Log("> " + nameof(CakeSpawner) + " >  SetDelaysForLevel");

        if (levelSpawnDelays.TryGetValue(level, out List<float> delays))
        {
            currentLevelDelays = delays;
            currentDelayIndex = 0;
            Debug.Log("> " + nameof(CakeSpawner) + " > SetDelaysForLevel: Delays set for level " + level);
        }
        else
        {
            Debug.Log(nameof(CakeSpawner) + " > SetDelaysForLevel: No spawn delays set for level " + level);
        }
    }


    private void SubscribeToEvents()
    {
        //DebugHelper.Log(nameof(CakeSpawner), "SPAWNER: SubscribeToEvents");
        if (GameManager.instance == null)
        {
            Debug.LogError("ERROR: GameManager instance not found.");
            return; // Stop further execution
        }
        GameManager.instance.OnLevelComplete += OnLevelComplete;
        GameManager.instance.OnLevelStart += OnLevelStart;
        GameManager.instance.OnCakeDropped += OnCakeDropped;
        GameManager.instance.OnTruckPacked += OnTruckPacked;
        GameManager.instance.OnPauseGamePlay += OnPauseGamePlay;
        GameManager.instance.OnResumeGamePlay += OnResumeGamePlay;
        GameManager.instance.OnGameOver += OnGameOver;
    }

    private void OnGameOver(object sender, EventArgs e)
    {
        isSpawnerOn = false;
    }

    private void OnResumeGamePlay(object sender, EventArgs e)
    {
        isSpawnerOn = true;
        //StopCoroutine(spawnCoroutine);
        //StartSpawning();
    }

    private void OnPauseGamePlay(object sender, EventArgs e)
    {
        isSpawnerOn = false;
    }

    private void OnCakeDropped(object sender, EventArgs e)
    {
        //DebugHelper.Log(nameof(CakeSpawner),"CAKESPAWNER: OnCakeDropped *****");

        //StartCoroutine(PauseAndRestartSpawning());
    }

    private IEnumerator PauseAndRestartSpawning()
    {
        // Pause 
        PauseSpawning();
        yield return new WaitForSeconds(GameManager.instance.CakeDropPauseTimer);

        // Restart spawning
        ResumeSpawning();
    }

    void OnLevelComplete(object sender, EventArgs e)
    {
        isSpawnerOn = false;
    }
    void OnLevelStart(object sender, EventArgs e)
    {
        //Debug.Log(nameof(CakeSpawner)+":  OnLevelStart");

        //int currentLevel = GameManager.instance.CurrentLevel; // Assuming a method to get the current level
        //print("Current level = " + currentLevel);
        //if (!levelSpawnDelays.ContainsKey(currentLevel))
        //{
        //    Debug.Log(nameof(CakeSpawner)+$": No spawn delays configured for level {currentLevel}");
        //    return; // Prevent further execution
        //}
        ////isSpawnerOn = true;
        //InitialiseSpawner();
        //SetDelaysForLevel(currentLevel);
        //currentDelayIndex = 0;
        //StartSpawning();
    }
}
