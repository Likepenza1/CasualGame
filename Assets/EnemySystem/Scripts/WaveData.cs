using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WaveData", menuName = "Enemy System/Wave Data", order = 1)]
public class WaveData : ScriptableObject
{
    public string waveName = "Wave";
    public float spawnInterval = 1f;
    public float delayAfterWave = 5f;

    [System.Serializable]
    public class WaveEnemy
    {
        public GameObject prefab;
        public int count;
    }

    public List<WaveEnemy> enemies = new List<WaveEnemy>();
}