using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SceneTransition : MonoBehaviour
{
    public CanvasGroup fadeGroup;
    public float fadeSpeed = 0.5f;

    void Start()
    {
        fadeGroup.alpha = 1; // Force black instantly
        StartCoroutine(FadeIn());
    }

    IEnumerator FadeIn()
    {
        fadeGroup.blocksRaycasts = true;

        // 1. WAIT for 2 frames. This lets Unity finish the "Heavy Loading" lag spike.
        yield return null;
        yield return null;

        while (fadeGroup.alpha > 0)
        {
            // 2. Use a specific speed. 
            // If it's still choppy, lower the fadeSpeed variable in the Inspector!
            fadeGroup.alpha -= Time.unscaledDeltaTime * fadeSpeed;
            yield return null;
        }

        fadeGroup.blocksRaycasts = false;
    }
}