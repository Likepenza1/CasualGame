using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class LobbyUIManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI goldText;
    
    [Header("Buttons")]
    [SerializeField] private Button shopButton;
    [SerializeField] private Button skillTreeButton;
    [SerializeField] private Button startBattleButton;
    [SerializeField] private Button inventoryButton;
    [SerializeField] private Button mercenariesButton;

    void Start()
    {
        UpdateGoldText();

        shopButton.onClick.AddListener(OnShopClicked);
        skillTreeButton.onClick.AddListener(OnSkillTreeClicked);
        startBattleButton.onClick.AddListener(OnStartBattleClicked);
        inventoryButton.onClick.AddListener(OnInventoryClicked);
        mercenariesButton.onClick.AddListener(OnMercenariesClicked);
    }

    void UpdateGoldText()
    {
        goldText.text = $"Gold: {PlayerPrefs.GetInt("Gold", 0)}";
    }

    void OnShopClicked() => Debug.Log("Открыт магазин");
    void OnSkillTreeClicked() => Debug.Log("Открыто дерево навыков");
    void OnStartBattleClicked() => SceneManager.LoadScene("BattleScene"); // или твоя сцена с волнами
    void OnInventoryClicked() => Debug.Log("Открыт инвентарь");
    void OnMercenariesClicked() => Debug.Log("Открыты наёмники");
}