using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    [Header("UI References")]
    public TextMeshProUGUI scoreText;
    public GameObject winPanel;
    private GameObject joystickCanvas; // Optional: To hide joystick on win
    private GameObject interactButton; // Optional: To hide button on win
    public GameObject gameOverPanel;

    [Header("Game State")]
    public int totalItems = 3;
    private int itemsCollected = 0;

    void Start()
    {
        // CRITICAL: Always unfreeze time when a scene loads!
        Time.timeScale = 1f;

        UpdateScoreUI();
        winPanel.SetActive(false);
        gameOverPanel.SetActive(false);
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

    void UpdateScoreUI()
    {
        scoreText.text = "Collected Quinine, Herbal Leaves, Bandages: " + itemsCollected + "/" + totalItems;
    }

    void WinGame()
    {
        Debug.Log("Level Complete!");

        // 1. Show Win Screen
        winPanel.SetActive(true);

        // 2. CRITICAL: Stop the Physics and Game Time
        Time.timeScale = 0f;

        // 3. (Optional) Disable Controls so you can't click things behind the panel
        if (joystickCanvas != null) joystickCanvas.SetActive(false);
        if (interactButton != null) interactButton.SetActive(false);
    }

    public void GameOver()
    {
        Debug.Log("Game Over!");
        gameOverPanel.SetActive(true);
        Time.timeScale = 0f; // Freeze the game
    }

    public void OnRestartPressed()
    {
        Time.timeScale = 1f; // Unfreeze time
        // Reload the current scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void OnNextLevelPressed()
    {
        // Unfreeze before leaving
        Time.timeScale = 1f;

        // Load next scene (Ensure you have added scenes in Build Settings!)
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex + 1);
    }

    public void OnMainMenuPressed()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }
}