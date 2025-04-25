using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BGData
{
    public string backgroundName;
    public GameObject backgroundPrefab;
    public List<WaveData> waves = new List<WaveData>();
}