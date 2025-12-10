using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class MainMenu : MonoBehaviour
{
    [Header("Transition Settings")]
    public CanvasGroup fadePanel; // Drag your TransitionPanel here
    public float fadeSpeed = 0.5f; // Lower = Slower/Smoother

    void Start()
    {
        // 1. CRITICAL FIX: Force time to run normally.
        // If you came from "Game Over", time might still be 0!
        Time.timeScale = 1f;

        // 2. Ensure the panel starts clear so we can see the menu
        if (fadePanel != null)
        {
            fadePanel.alpha = 0f;
            fadePanel.blocksRaycasts = false;
        }
    }

    public void OnPlayPressed()
    {
        // 3. Start the transition
        StartCoroutine(FadeOutAndLoad("Level 1"));
    }

    public void OnQuitPressed()
    {
        Debug.Log("Quit Game!");
        Application.Quit();
    }

    IEnumerator FadeOutAndLoad(string sceneName)
    {
        // 4. Block clicks so user doesn't spam the button
        if (fadePanel != null)
        {
            fadePanel.blocksRaycasts = true;

            // 5. Fade to Black
            // We use 'unscaledDeltaTime' so it works even if the game lags or pauses
            while (fadePanel.alpha < 1f)
            {
                fadePanel.alpha += Time.unscaledDeltaTime * fadeSpeed;
                yield return null;
            }
        }

        // 6. Wait a moment for dramatic effect
        yield return new WaitForSecondsRealtime(0.5f);

        // 7. Load the Level
        SceneManager.LoadScene(sceneName);
    }
}