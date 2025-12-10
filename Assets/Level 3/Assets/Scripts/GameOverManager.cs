using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverManager : MonoBehaviour
{
    [Header("UI Objects")]
    public GameObject gameOverUI; // Ang Game Over Panel
    public GameObject gameHUD;    // Ang HUD na itatago

    [Header("Scene Settings")]
    public string mainMenuSceneName = "MainMenu"; // Pangalan ng Main Menu scene mo

    public void GameOver()
    {
        Time.timeScale = 0f; // Tigil ang oras
        gameOverUI.SetActive(true);

        if (gameHUD != null)
        {
            gameHUD.SetActive(false);
        }
    }

    public void RestartGame()
    {
        Time.timeScale = 1f; // Ibalik ang oras sa normal
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    // Ito ang function na ilalagay mo sa Button
    public void MainMenu()
    {
        Time.timeScale = 1f; // IMPORTANTE: Ibalik ang oras bago lumipat ng scene
        SceneManager.LoadScene(mainMenuSceneName);
    }
}