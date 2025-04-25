using UnityEngine;
using UnityEngine.UI;

public class SaveLoadUI : MonoBehaviour
{
    public Button saveButton;
    public Button loadButton;
    public Button deleteButton;

    private PlayerController player;

    void Start()
    {
        player = FindObjectOfType<PlayerController>();

        saveButton.onClick.AddListener(SaveGame);
        loadButton.onClick.AddListener(LoadGame);
        deleteButton.onClick.AddListener(DeleteSave);
    }

    void SaveGame()
    {
        if (player != null)
        {
            SaveData data = player.CreateSaveData();
            SaveManager.Save(data);
            Debug.Log("Сохранение выполнено вручную.");
        }
    }

    void LoadGame()
    {
        SaveData loaded = SaveManager.Load();
        if (loaded != null && player != null)
        {
            player.ApplySaveData(loaded);
            Debug.Log("Загрузка выполнена вручную.");
        }
        else
        {
            Debug.LogWarning("Сохранение не найдено.");
        }
    }

    void DeleteSave()
    {
        SaveManager.DeleteAll();
        Debug.Log("Все сохранения удалены.");
    }
}