using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class PlayerInventory : MonoBehaviour
{
    [Header("LOOT SETTINGS")]
    public int totalLoot = 0;

    [Header("UI SETTINGS")]
    public TMP_Text lootText;   // Drag your TextMeshPro UI here

    [Header("ITEMS")]
    public List<string> collectedItems = new List<string>();

    private void Start()
    {
        UpdateLootUI();
    }

    // Call this when the player collects an item
    public void AddItem(string itemName, int value)
    {
        totalLoot += value;
        collectedItems.Add(itemName); // <- store the item name
        UpdateLootUI();

        Debug.Log("Collected " + itemName + "! Total Loot = " + totalLoot);
    }

    void UpdateLootUI()
    {
        if (lootText != null)
            lootText.text = "Map, Anatomical Chart, Student's Nootebooks: " + totalLoot + "/3";
    }
}
