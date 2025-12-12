using UnityEngine;
using System.Collections;

public class LevelIntro : MonoBehaviour
{
    [Header("UI Settings")]
    public CanvasGroup uiCanvasGroup;
    public float displayTime = 2f;
    public float fadeDuration = 1.5f;

    [Header("Player Settings")]
    public PlayerMovement playerMovement; // Dito ilalagay ang Player object

    void Start()
    {
        uiCanvasGroup.alpha = 1f;

        // 1. I-DISABLE muna ang player movement script pag-start
        if (playerMovement != null)
        {
            playerMovement.enabled = false;
            // Dahil dito, magra-run ang OnDisable() sa script mo at titigil ang controls
        }

        StartCoroutine(DoFadeOut());
    }

    IEnumerator DoFadeOut()
    {
        yield return new WaitForSeconds(displayTime);

        float timer = 0f;
        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            uiCanvasGroup.alpha = Mathf.Lerp(1f, 0f, timer / fadeDuration);
            yield return null;
        }

        uiCanvasGroup.alpha = 0f;
        gameObject.SetActive(false);

        // 2. I-ENABLE na ulit ang player pagtapos ng fade
        if (playerMovement != null)
        {
            playerMovement.enabled = true;
            // Magra-run na ang OnEnable() sa script mo at gagana na ang controls
        }
    }
}