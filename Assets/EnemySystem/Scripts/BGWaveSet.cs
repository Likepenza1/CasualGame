using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BGWaveSet", menuName = "Enemy System/BG Wave Set", order = 2)]
public class BGWaveSet : ScriptableObject
{
    public string setName = "BG Set";
    public List<WaveData> waves = new List<WaveData>();
}