using System.Collections;
using UnityEngine;
using TMPro;

public class WaveSpawner : MonoBehaviour
{
    public enum SpawnerState { BetweenWaves, Spawning, InWave, Shop }

    [Header("Enemy Settings")]
    [SerializeField] private GameObject enemyPrefab;

    [Header("Wave Settings")]
    [SerializeField] private int startCount = 3;
    [SerializeField] private int addPerWave = 2;
    [SerializeField] private float timeBetweenSpawns = 0.5f;
    [SerializeField] private float timeBetweenWaves = 3f;

    [Header("Map Bounds")]
    [SerializeField] private Vector2 mapMin = new Vector2(-8f, -4f);
    [SerializeField] private Vector2 mapMax = new Vector2(9f, 8f);

    [Header("Options")]
    [SerializeField] private bool enableShopBreaks = false;
    [SerializeField] private int wavesPerShop = 5;
    [SerializeField] private TMP_Text waveText;

    private int currentWave = 0;
    private int enemiesToSpawn = 0;
    private int enemiesSpawned = 0;
    private int enemiesAlive = 0;
    private float spawnTimer = 0f;
    private float betweenWaveTimer = 0f;
    private SpawnerState state = SpawnerState.BetweenWaves;

    // track enemy destroyed 
    public void NotifyEnemyDied()
    {
        enemiesAlive = Mathf.Max(0, enemiesAlive - 1);
    }

    void Start()
    {   
        // start wave immediately
        PrepareNextWave();
    }

    void Update()
    {
        switch (state)
        {
            case SpawnerState.Spawning:
                // spawn enemies at fixed intervals until reach enemies to spawn for wave
                spawnTimer -= Time.deltaTime;
                if (enemiesSpawned < enemiesToSpawn && spawnTimer <= 0f)
                {
                    SpawnOne();
                    enemiesSpawned++;
                    spawnTimer = timeBetweenSpawns;
                }
                else if (enemiesSpawned >= enemiesToSpawn)
                {
                    // all enemies from this wave have spawned, wait 
                    state = SpawnerState.InWave;
                }
                break;

            case SpawnerState.InWave:
                if (enemiesAlive == 0)
                {
                    // wave cleared
                    // check if time for a shop break
                    if (enableShopBreaks && wavesPerShop > 0 && (currentWave % wavesPerShop == 0))
                    {
                        OpenShop();
                    }
                    else
                    {
                        // otherwise start next wave
                        state = SpawnerState.BetweenWaves;
                        betweenWaveTimer = timeBetweenWaves;
                    }
                }
                break;

            case SpawnerState.BetweenWaves:
                // rest period before next wave
                if (betweenWaveTimer > 0f)
                {
                    betweenWaveTimer -= Time.deltaTime;
                    if (betweenWaveTimer <= 0f)
                    {
                        PrepareNextWave();
                    }
                }
                break;

            case SpawnerState.Shop:
                // do nothing, Shop defines what to do here
                break;
        }
    }

    // reset wave index, computes how many enemies this wave spawns, update state
    private void PrepareNextWave()
    {
        currentWave++;
        UpdateWaveText();
        enemiesToSpawn = startCount + addPerWave * (currentWave - 1);
        enemiesSpawned = 0;
        spawnTimer = 0f;
        state = SpawnerState.Spawning;
    }

    // spawn enemies one by one
    private void SpawnOne()
    {
        Vector3 spawnPos;

        // random spawns on the edges
        int side = Random.Range(0, 4);
        switch (side)
        {
            case 0: // north
                spawnPos = new Vector3(Random.Range(mapMin.x, mapMax.x), mapMax.y, 0f);
                break;
            case 1: // south
                spawnPos = new Vector3(Random.Range(mapMin.x, mapMax.x), mapMin.y, 0f);
                break;
            case 2: // west
                spawnPos = new Vector3(mapMin.x, Random.Range(mapMin.y, mapMax.y), 0f);
                break;
            default: // east
                spawnPos = new Vector3(mapMax.x, Random.Range(mapMin.y, mapMax.y), 0f);
                break;
        }

        // instantiate enemy
        var enemy = Instantiate(enemyPrefab, spawnPos, Quaternion.identity);

        // report deaths 
        var rep = enemy.GetComponent<EnemyDeathReporter>();
        if (rep == null)
        {
            rep = enemy.AddComponent<EnemyDeathReporter>();
        }
        rep.SetOwner(this);

        enemiesAlive++;
    }
    // open shop ui
    private void OpenShop()
    {
        // update state
        state = SpawnerState.Shop;

        if (enableShopBreaks && ShopUI.Instance != null)
        {
            ShopUI.Instance.Open();
        }
        else
        {
            // if not using shop, endless waves
            state = SpawnerState.BetweenWaves;
            betweenWaveTimer = timeBetweenWaves;
        }
    }

    public void CloseShopAndContinue()
    {
        // resume waves
        state = SpawnerState.BetweenWaves;
        betweenWaveTimer = timeBetweenWaves;
    }

    public void UpdateWaveText()
    {
        // for wave ui text during gameplay
        if (waveText != null)
        {
            waveText.text = "Wave: " + currentWave;
        }
    }
    
    public void ResetForNewRun(bool startImmediately = true)
    {
        // stop any pending spawns
        StopAllCoroutines();

        // reset every thing
        currentWave = 0;
        enemiesToSpawn = 0;
        enemiesSpawned = 0;
        enemiesAlive = 0;
        spawnTimer = 0f;
        betweenWaveTimer = 0f;
        state = SpawnerState.BetweenWaves;

        // update UI
        UpdateWaveText();

        // close shop if it happened to be left open
        if (ShopUI.Instance != null && ShopUI.Instance.IsOpen)
            ShopUI.Instance.ContinueGame();

        // kill any leftover enemies from the previous run
        var leftovers = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (var e in leftovers)
        {
            var rep = e.GetComponent<EnemyDeathReporter>();
            if (rep)
            {
                rep.DisableReporting();
                Destroy(e);
            }
        }

        // also kill any leftover coins from the previous run
        var coins = GameObject.FindGameObjectsWithTag("Coin");
        foreach (var c in coins)
        {
            Destroy(c);
        }
        // optionally start wave 1 immediately
        if (startImmediately)
            PrepareNextWave();
    }

}

