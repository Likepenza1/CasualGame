using UnityEngine;
using System.Collections.Generic;

public class BackgroundManager : MonoBehaviour
{
    [Header("Максимало волн")]
    public int maxWaves = 5;
    
    [Header("Ai спавн ссылка")]
    public AiSpawner aiSpawner;
    public List<BGData> bgDataList;
    
    private int currentBGIndex = 0;
    private int lastCheckedWaveIndex = -1;
    private List<GameObject> instantiatedBGs = new List<GameObject>();
    
    public BGContent GetCurrentBGContent()
    {
        if (currentBGIndex >= 0 && currentBGIndex < instantiatedBGs.Count)
            return instantiatedBGs[currentBGIndex].GetComponent<BGContent>();

        return null;
    }

    void Start()
    {
        foreach (var bg in bgDataList)
        {
            if (bg.backgroundPrefab != null)
            {
                GameObject instance = Instantiate(bg.backgroundPrefab, Vector3.zero, Quaternion.identity, transform);
                instance.SetActive(false);
                instantiatedBGs.Add(instance);
            }
        }

        if (instantiatedBGs.Count > 0)
        {
            instantiatedBGs[currentBGIndex].SetActive(true);

            if (aiSpawner != null)
            {
                aiSpawner.currentBGIndex = currentBGIndex;
                aiSpawner.backgroundManager = this;
                aiSpawner.StartWaves(); // <-- вызываем только после активации
            }
        }
    }

    public void SwitchBackground()
    {
        if (currentBGIndex < instantiatedBGs.Count)
            instantiatedBGs[currentBGIndex].SetActive(false);

        currentBGIndex++;

        if (currentBGIndex < instantiatedBGs.Count)
            instantiatedBGs[currentBGIndex].SetActive(true);
        
        if (aiSpawner != null)
            aiSpawner.currentBGIndex = currentBGIndex;
    }
    
    public void LoadBackgroundByID(int id)
    {
        SwitchBackgroundTo(id);
    }

    public void SwitchBackgroundTo(int id)
    {
        SetBGByID(id);
    }

    // === Методы для сохранения/загрузки ===
    public int GetCurrentBGID()
    {
        return currentBGIndex;
    }

    public void SetBGByID(int index)
    {
        if (index >= 0 && index < instantiatedBGs.Count)
        {
            if (currentBGIndex < instantiatedBGs.Count)
                instantiatedBGs[currentBGIndex].SetActive(false);

            currentBGIndex = index;
            instantiatedBGs[currentBGIndex].SetActive(true);
        }
    }
}