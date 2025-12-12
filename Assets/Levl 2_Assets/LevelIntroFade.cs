using UnityEngine;
using TMPro;
using System.Collections; // Kailangan ito para sa Animation (Coroutines)

public class LevelIntroFade : MonoBehaviour
{
    [Header("References")]
    public TextMeshProUGUI titleText;
    public CanvasGroup canvasGroup; 

    [Header("Settings")]
    public string levelName = "Level 2: Governor's Office"; // Type mo dito yung title
    public float fadeInDuration = 1.0f;  // Gaano katagal mag-fade in
    public float displayDuration = 2.0f; // Gaano katagal naka-display
    public float fadeOutDuration = 1.0f; // Gaano katagal mawala

    void Start()
    {
        // Set natin ang text base sa settings
        if(titleText != null)
        {
            titleText.text = levelName;
        }

        // Simulan ang animation
        StartCoroutine(PlayIntroSequence());
    }

    IEnumerator PlayIntroSequence()
    {
        // STEP 1: Siguraduhing invisible muna sa simula (Alpha 0)
        canvasGroup.alpha = 0;

        // STEP 2: FADE IN LOOP
        float timer = 0;
        while (timer < fadeInDuration)
        {
            timer += Time.deltaTime;
            // Unti-unting lilitaw (0 to 1)
            canvasGroup.alpha = timer / fadeInDuration;
            yield return null; // Wait for next frame
        }
        canvasGroup.alpha = 1; // Siguraduhing litaw na litaw

        // STEP 3: WAIT (Hinto muna para mabasa ng player)
        yield return new WaitForSeconds(displayDuration);

        // STEP 4: FADE OUT LOOP
        timer = 0;
        while (timer < fadeOutDuration)
        {
            timer += Time.deltaTime;
            // Unti-unting mawawala (1 to 0)
            canvasGroup.alpha = 1 - (timer / fadeOutDuration);
            yield return null;
        }
        canvasGroup.alpha = 0; // Siguraduhing tago na

        // STEP 5: Disable na natin para di kumain ng memory
        gameObject.SetActive(false);
    }
}