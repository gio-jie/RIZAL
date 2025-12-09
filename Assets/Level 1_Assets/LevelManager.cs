using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class LevelManager : MonoBehaviour
{
    [Header("UI References")]
    public TextMeshProUGUI scoreText;
    public GameObject winPanel;
    
    // --- ORIGINALS (Wag burahin para di masira connection sa Level 1) ---
    public GameObject joystickCanvas; 
    public GameObject interactButton; 
    public GameObject gameOverPanel;
    // -------------------------------------------------------------------

    [Header("Game Settings")]
    public int totalItems = 3;
    private int itemsCollected = 0;

    // --- ITO ANG BAGO: Dito mo ita-type ang text sa Inspector ---
    [TextArea] // Ginagawa nitong malaki ang typing box sa inspector
    public string objectiveText = "Collected Items: "; 
    // ---------------------------------------------------------

    // --- LEVEL 2 KEYS ---
    public List<string> collectedKeys = new List<string>(); 

    void Start()
    {
        Time.timeScale = 1f;
        UpdateScoreUI(); // Tatawagin nito agad ang text pagka-start
        
        if(winPanel != null) winPanel.SetActive(false);
        if(gameOverPanel != null) gameOverPanel.SetActive(false);
    }

    public void ItemCollected()
    {
        itemsCollected++;
        UpdateScoreUI();

        if (itemsCollected >= totalItems)
        {
            WinGame();
        }
    }

    public void AddKey(string keyName)
    {
        if (!collectedKeys.Contains(keyName))
        {
            collectedKeys.Add(keyName);
            Debug.Log("Picked up key: " + keyName);
        }
    }

    public bool HasKey(string keyName)
    {
        return collectedKeys.Contains(keyName);
    }

    // --- ITO ANG NAGBAGO: Pinagsasama niya ang Text + Score ---
    void UpdateScoreUI()
    {
        if (scoreText != null)
        {
            // Format: "Yung Text Mo" + " 1/3"
            scoreText.text = objectiveText + " " + itemsCollected + "/" + totalItems;
        }
    }
    // ----------------------------------------------------------

    void WinGame()
    {
        Debug.Log("Level Complete!");
        if (winPanel != null) winPanel.SetActive(true);
        Time.timeScale = 0f;
        
        // Safety check para sa originals
        if (joystickCanvas != null) joystickCanvas.SetActive(false);
        if (interactButton != null) interactButton.SetActive(false);
    }

    public void GameOver()
    {
        Debug.Log("Game Over!");
        if (gameOverPanel != null) gameOverPanel.SetActive(true);
        Time.timeScale = 0f;
    }

    public void OnRestartPressed() { Time.timeScale = 1f; SceneManager.LoadScene(SceneManager.GetActiveScene().name); }
    public void OnNextLevelPressed() { Time.timeScale = 1f; SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1); }
    public void OnMainMenuPressed() { Time.timeScale = 1f; SceneManager.LoadScene("MainMenu"); }
}