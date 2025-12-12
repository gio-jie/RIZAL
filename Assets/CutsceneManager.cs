using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections;

public class CutsceneManager : MonoBehaviour
{
    [Header("SETTINGS")]
    public GameObject[] storyPanels;
    public string nextLevelName = "Level1";

    [Header("ANIMATION")]
    public float typingSpeed = 0.05f;
    public float fadeDuration = 1f;

    [Header("AUDIO")]
    public AudioClip backgroundMusic; // Dito mo ilalagay ang BGM
    public AudioClip typingSound;     // Dito mo ilalagay ang "click" sound

    [Range(0f, 1f)] public float musicVolume = 0.5f; // Slider para sa lakas ng BGM
    [Range(0f, 1f)] public float sfxVolume = 1f;     // Slider para sa lakas ng Typing SFX

    private int currentPanelIndex = 0;
    private bool isTyping = false;
    private string currentFullText = "";
    private TMP_Text activeTextComponent;

    // Gumawa tayo ng dalawang audio source: isa sa music, isa sa sfx
    private AudioSource musicSource;
    private AudioSource sfxSource;

    void Start()
    {
        // 1. SETUP AUDIO SOURCES (Automatic na gagawin ng script)
        musicSource = gameObject.AddComponent<AudioSource>();
        sfxSource = gameObject.AddComponent<AudioSource>();

        // Setup Music
        if (backgroundMusic != null)
        {
            musicSource.clip = backgroundMusic;
            musicSource.loop = true; // Para umulit-ulit
            musicSource.volume = musicVolume;
            musicSource.Play();
        }

        // Setup SFX
        sfxSource.volume = sfxVolume;

        // 2. HIDE PANELS
        foreach (var panel in storyPanels)
        {
            panel.SetActive(false);
            if (panel.GetComponent<CanvasGroup>() == null)
            {
                panel.AddComponent<CanvasGroup>();
            }
        }

        StartCoroutine(PlayScene(currentPanelIndex));
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Return))
        {
            if (isTyping)
            {
                StopAllCoroutines();
                activeTextComponent.text = currentFullText;
                activeTextComponent.maxVisibleCharacters = currentFullText.Length;
                isTyping = false;
            }
            else
            {
                StartCoroutine(TransitionToNext());
            }
        }
    }

    IEnumerator PlayScene(int index)
    {
        GameObject panel = storyPanels[index];
        panel.SetActive(true);

        CanvasGroup canvasGroup = panel.GetComponent<CanvasGroup>();
        canvasGroup.alpha = 0f;

        // FADE IN
        float timer = 0f;
        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(0f, 1f, timer / fadeDuration);
            yield return null;
        }
        canvasGroup.alpha = 1f;

        // TYPEWRITER EFFECT
        activeTextComponent = panel.GetComponentInChildren<TMP_Text>();

        if (activeTextComponent != null)
        {
            currentFullText = activeTextComponent.text;
            activeTextComponent.text = "";
            activeTextComponent.maxVisibleCharacters = 0;
            activeTextComponent.text = currentFullText;

            isTyping = true;

            foreach (char letter in currentFullText.ToCharArray())
            {
                activeTextComponent.maxVisibleCharacters++;

                // Play Typing Sound
                // Nilagyan ko ng check: Tutunog lang kung HINDI space ang tina-type
                if (typingSound != null && !char.IsWhiteSpace(letter))
                {
                    // Randomize pitch ng onti para mas natural (hindi robotic)
                    sfxSource.pitch = Random.Range(0.9f, 1.1f);
                    sfxSource.PlayOneShot(typingSound);
                }

                yield return new WaitForSeconds(typingSpeed);
            }

            isTyping = false;
        }
    }

    IEnumerator TransitionToNext()
    {
        // FADE OUT
        GameObject currentPanel = storyPanels[currentPanelIndex];
        CanvasGroup canvasGroup = currentPanel.GetComponent<CanvasGroup>();

        float timer = 0f;
        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(1f, 0f, timer / fadeDuration);
            yield return null;
        }
        canvasGroup.alpha = 0f;
        currentPanel.SetActive(false);

        currentPanelIndex++;
        if (currentPanelIndex < storyPanels.Length)
        {
            StartCoroutine(PlayScene(currentPanelIndex));
        }
        else
        {
            Debug.Log("End of Cutscene");
            SceneManager.LoadScene(nextLevelName);
        }
    }
}