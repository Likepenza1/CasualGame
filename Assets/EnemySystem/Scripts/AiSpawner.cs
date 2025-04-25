using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Random = UnityEngine.Random;



public class AiSpawner : MonoBehaviour
{
    [Header("Spawn Settings")]
    public Vector3 spawnAreaSize = new Vector3(10f, 0f, 10f);
    public int maxEnemies = 100;
    public bool autoStart = true;

    [Header("UI References")]
    public TextMeshProUGUI currentWaveText;
    public TextMeshProUGUI nextWaveText;

    [Header("UI gameobjects")]
    public GameObject UIWavesStart;
    public TextMeshProUGUI waveNumberText;
    public float waveStartUIDuration = 2f;

    [Header("Debug")]
    public int currentWaveIndex = -1;
    public int enemiesAlive = 0;
    public bool isSpawning = false;
    public int currentBGIndex = 0;
    
    public BackgroundManager backgroundManager;
    private Coroutine spawnCoroutine;
    private Coroutine uiDisplayCoroutine;
    private int _currentWaveIndex;

    private List<WaveData> waves => 
        (backgroundManager != null &&
         backgroundManager.bgDataList != null &&
         currentBGIndex < backgroundManager.bgDataList.Count)
            ? backgroundManager.bgDataList[currentBGIndex].waves
            : new List<WaveData>();

    void Start()
    {
        if (backgroundManager == null)
            backgroundManager = FindObjectOfType<BackgroundManager>();
        
        if (UIWavesStart != null)
            UIWavesStart.SetActive(false);

        UpdateWaveUI();
    }

    public void StartWaves()
    {
        if (waves.Count == 0)
        {
            Debug.LogWarning("No waves configured for current BG!");
            return;
        }

        currentWaveIndex = 0;
        UpdateWaveUI();
        StartCoroutine(WaveRoutine());
    }

    IEnumerator WaveRoutine()
    {
        while (currentWaveIndex < waves.Count)
        {
            WaveData currentWave = waves[currentWaveIndex];
            Debug.Log($"Starting wave: {currentWave.waveName}");

            ShowWaveStartUI(currentWaveIndex + 1);
            UpdateWaveUI();

            spawnCoroutine = StartCoroutine(SpawnWave(currentWave));
            yield return spawnCoroutine;

            yield return new WaitUntil(() => enemiesAlive <= 0);
            Debug.Log($"Wave {currentWave.waveName} completed!");

            yield return new WaitForSeconds(currentWave.delayAfterWave);

            currentWaveIndex++;
        }

        currentWaveIndex = -1;
        UpdateWaveUI();
        Debug.Log("All waves completed!");

        // Переключение BG
        if (backgroundManager != null)
        {
            backgroundManager.SwitchBackground();
            currentBGIndex = backgroundManager.GetCurrentBGID();
        }

        yield return new WaitForSeconds(2f);

        StartWaves(); // рестарт волн с новыми монстрами
    }

    void ShowWaveStartUI(int waveNumber)
    {
        if (uiDisplayCoroutine != null)
            StopCoroutine(uiDisplayCoroutine);

        uiDisplayCoroutine = StartCoroutine(DisplayWaveStartUICoroutine(waveNumber));
    }

    IEnumerator DisplayWaveStartUICoroutine(int waveNumber)
    {
        if (UIWavesStart != null)
        {
            if (waveNumberText != null)
                waveNumberText.text = $"WAVE {waveNumber}";

            UIWavesStart.SetActive(true);
            yield return new WaitForSeconds(waveStartUIDuration);
            UIWavesStart.SetActive(false);
        }
    }

    IEnumerator SpawnWave(WaveData wave)
    {
        isSpawning = true;

        foreach (var waveEnemy in wave.enemies)
        {
            for (int i = 0; i < waveEnemy.count; i++)
            {
                if (enemiesAlive >= maxEnemies)
                    yield return new WaitUntil(() => enemiesAlive < maxEnemies);

                SpawnEnemy(waveEnemy.prefab);
                yield return new WaitForSeconds(wave.spawnInterval);
            }
        }

        isSpawning = false;
    }

    void SpawnEnemy(GameObject enemyPrefab)
    {
        Vector3 spawnPosition = transform.position + new Vector3(
            Random.Range(-spawnAreaSize.x / 2f, spawnAreaSize.x / 2f),
            Random.Range(-spawnAreaSize.y / 2f, spawnAreaSize.y / 2f),
            Random.Range(-spawnAreaSize.z / 2f, spawnAreaSize.z / 2f));

        GameObject enemy = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity, transform);
        enemiesAlive++;
    }

    public void EnemyDestroyed()
    {
        enemiesAlive--;
        enemiesAlive = Mathf.Clamp(enemiesAlive, 0, maxEnemies);
    }

    public void SkipToNextWave()
    {
        if (spawnCoroutine != null)
            StopCoroutine(spawnCoroutine);

        currentWaveIndex++;

        if (currentWaveIndex < waves.Count)
        {
            UpdateWaveUI();
            StartCoroutine(WaveRoutine());
        }
    }

    void UpdateWaveUI()
    {
        if (currentWaveText != null)
            currentWaveText.text = currentWaveIndex >= 0 ? $"{currentWaveIndex + 1}/{waves.Count}" : "";

        if (nextWaveText != null)
            nextWaveText.text = currentWaveIndex < waves.Count - 1 ? $"{currentWaveIndex + 2}/{waves.Count}" : "";
    }

    void OnDrawGizmos()
    {
        Gizmos.color = new Color(1f, 0f, 0f, 0.3f);
        Gizmos.DrawCube(transform.position, spawnAreaSize);
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, spawnAreaSize);
    }
}
