using System;
using UnityEngine;
using System.IO;

public static class SaveManager
{
    private static string SaveFilePath => Path.Combine(Application.persistentDataPath, "save.json");

    public static void Save(SaveData data)
    {
        try
        {
            string json = JsonUtility.ToJson(data, true);
            File.WriteAllText(SaveFilePath, json);
            Debug.Log("Game Saved to: " + SaveFilePath);
        }
        catch (Exception ex)
        {
            Debug.LogError($"Error saving game: {ex.Message}");
        }
    }

    public static SaveData Load()
    {
        try
        {
            if (File.Exists(SaveFilePath))
            {
                string json = File.ReadAllText(SaveFilePath);
                SaveData data = JsonUtility.FromJson<SaveData>(json);
                Debug.Log("Game Loaded");
                return data;
            }
            else
            {
                Debug.LogWarning("No save file found, returning new SaveData");
                return new SaveData(); // Возвращаем пустую структуру по умолчанию
            }
        }
        catch (Exception ex)
        {
            Debug.LogError($"Error loading game: {ex.Message}");
            return new SaveData(); // Возвращаем пустую структуру по умолчанию в случае ошибки
        }
    }

    public static void DeleteSave()
    {
        try
        {
            if (File.Exists(SaveFilePath))
            {
                File.Delete(SaveFilePath);
                Debug.Log("Save file deleted.");
            }
        }
        catch (Exception ex)
        {
            Debug.LogError($"Error deleting save file: {ex.Message}");
        }
    }
    
    public static void DeleteAll()
    {
        if (File.Exists(SaveFilePath))
        {
            File.Delete(SaveFilePath);
            Debug.Log("Save file deleted.");
        }
    }
}