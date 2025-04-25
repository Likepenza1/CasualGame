using System.Collections.Generic;
using UnityEngine;

public class BGContent : MonoBehaviour
{
    public string bgName = "BG Name";

    [Header("Waves for this BG")]
    public List<WaveData> waves = new List<WaveData>();
}