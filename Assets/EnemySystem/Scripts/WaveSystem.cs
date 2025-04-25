using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class WaveSystem : MonoBehaviour
{
    [Header("Wave Settings")]
    public GameObject enemyPrefab;
    public GameObject bossPrefab;
    public Transform playerTarget;

    [Header("Timing")]
    public float timeBetweenWaves = 5f;
    public float timeBetweenSpawns = 5f;

    [Header("Spawn Area")]
    public Vector3 spawnCenter = Vector3.zero;
    public Vector3 spawnSize = new Vector3(10f, 0f, 10f);

    [Header("UI")]
    public TextMeshProUGUI currentWaveText;
    public TextMeshProUGUI nextWaveText;

    private int currentWave = 0;
    private bool isSpawning = false;
    private List<GameObject> activeEnemies = new List<GameObject>();

    void Start()
    {
        StartNextWave();
    }

    void Update()
    {
        // Проверяем, закончилась ли волна (все враги уничтожены)
        if (!isSpawning && activeEnemies.Count == 0)
        {
            StartCoroutine(WaitAndStartNextWave());
        }

        // Обновление UI
        if (currentWaveText != null)
            currentWaveText.text = $"Wave: {currentWave}";
        if (nextWaveText != null)
            nextWaveText.text = isSpawning ? "Spawning..." : "Waiting for next wave...";
    }

    IEnumerator WaitAndStartNextWave()
    {
        yield return new WaitForSeconds(timeBetweenWaves);
        StartNextWave();
    }

    void StartNextWave()
    {
        currentWave++;
        StartCoroutine(SpawnWave(currentWave));
    }

    IEnumerator SpawnWave(int waveNumber)
    {
        isSpawning = true;
        int enemiesToSpawn;

        if (waveNumber % 6 == 0)
        {
            enemiesToSpawn = 1;
            yield return SpawnEnemyWithDelay(bossPrefab);
        }
        else
        {
            int waveIndex = (waveNumber - 1) % 6;
            enemiesToSpawn = 5 + waveIndex;

            for (int i = 0; i < enemiesToSpawn; i++)
            {
                yield return SpawnEnemyWithDelay(enemyPrefab);
            }
        }

        isSpawning = false;
    }

    IEnumerator SpawnEnemyWithDelay(GameObject prefab)
    {
        SpawnEnemy(prefab);
        yield return new WaitForSeconds(timeBetweenSpawns);
    }

    void SpawnEnemy(GameObject prefab)
    {
        Vector3 randomPos = spawnCenter + new Vector3(
            Random.Range(-spawnSize.x / 2, spawnSize.x / 2),
            spawnCenter.y,
            Random.Range(-spawnSize.z / 2, spawnSize.z / 2)
        );

        GameObject enemy = Instantiate(prefab, randomPos, Quaternion.identity);
        activeEnemies.Add(enemy);

        // Подписка на уничтожение
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(spawnCenter, spawnSize);
    }
}
