using UnityEngine;
using TMPro;
using System.Collections; // Kailangan ito para sa Coroutine (IEnumerator)
using System.Collections.Generic;

public class PlayerInventory : MonoBehaviour
{
    [Header("LOOT SETTINGS")]
    public int totalLoot = 0;
    public int targetLootCount = 3;

    [Header("UI SETTINGS")]
    public TMP_Text lootText;

    [Header("NOTIFICATION SETTINGS")]
    public GameObject notificationObject; // Dito mo ida-drag yung Text/Panel na ginawa mo
    public float displayDuration = 4f;    // Ilang segundo bago mawala

    [Header("ITEMS")]
    public List<string> collectedItems = new List<string>();

    [Header("EXIT REFERENCE")]
    public LevelExit levelExit;

    // Flag para hindi paulit-ulit lumabas ang notif kung sumobra sa 3 ang items
    private bool isObjectiveComplete = false;

    private void Start()
    {
        UpdateLootUI();

        // Siguraduhin na nakatago ang notif sa simula
        if (notificationObject != null)
            notificationObject.SetActive(false);
    }

    public void AddItem(string itemName, int value)
    {
        totalLoot += value;
        collectedItems.Add(itemName);
        UpdateLootUI();

        Debug.Log("Collected " + itemName + "! Total Loot = " + totalLoot);

        CheckIfComplete();
    }

    void CheckIfComplete()
    {
        // Check kung naabot na ang target AT kung hindi pa tapos ang mission dati
        if (totalLoot >= targetLootCount && !isObjectiveComplete)
        {
            isObjectiveComplete = true; // Markahan na tapos na para di umulit

            // 1. Buksan ang pinto
            if (levelExit != null)
            {
                levelExit.UnlockExit();
            }

            // 2. Ipakita ang Notification
            StartCoroutine(ShowNotificationRoutine());
        }
    }

    // Ito ang timer para sa notification
    IEnumerator ShowNotificationRoutine()
    {
        if (notificationObject != null)
        {
            notificationObject.SetActive(true); // Ipakita (Show)

            // Maghintay ng 4 seconds
            yield return new WaitForSeconds(displayDuration);

            notificationObject.SetActive(false); // Itago (Hide)
        }
    }

    void UpdateLootUI()
    {
        if (lootText != null)
            lootText.text = "Map, Anatomical Chart, Student's Notebook:  " + totalLoot + "/" + targetLootCount;
    }
}