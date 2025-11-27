using UnityEngine;
using TMPro;

public class PlayerInventory : MonoBehaviour
{
    [Header("LOOT SETTINGS")]
    public int totalLoot = 0;

    [Header("UI SETTINGS")]
    public TMP_Text lootText;   // Drag your TextMeshPro UI here

    private void Start()
    {
        UpdateLootUI();
    }

    // Call this when the player collects an item
    public void AddItem(string itemName, int value)
    {
        totalLoot += value;
        UpdateLootUI();

        Debug.Log("Collected " + itemName + "! Total Loot = " + totalLoot);
    }

    void UpdateLootUI()
    {
        if (lootText != null)
            lootText.text = "Loot: " + totalLoot;
    }
}
