using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections; // Kailangan ito para sa Coroutine

[RequireComponent(typeof(AudioSource))] // Kusa maglalagay ng AudioSource
public class LevelExit : MonoBehaviour
{
    [Header("SCENE SETTINGS")]
    public string cutsceneSceneName = "Cutscene";
    public float waitBeforeCutscene = 3f; // Ilang seconds bago lumipat (Default: 3)

    [Header("VISUALS")]
    public Sprite openDoorSprite;

    [Header("AUDIO")]
    public AudioClip winSound; // Sound effect pagpasok sa exit (e.g., Victory tune)

    private bool isOpen = false;
    private bool levelFinished = false; // Para hindi ma-trigger ng paulit-ulit
    private SpriteRenderer spriteRenderer;
    private AudioSource audioSource;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>();
    }

    public void UnlockExit()
    {
        if (!isOpen)
        {
            isOpen = true;
            Debug.Log("LEVEL COMPLETE! Exit is now OPEN.");

            if (openDoorSprite != null && spriteRenderer != null)
            {
                spriteRenderer.sprite = openDoorSprite;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Kung Player ang pumasok, Bukas ang pinto, at Hindi pa tapos ang level
        if (other.CompareTag("Player") && isOpen && !levelFinished)
        {
            StartCoroutine(FinishLevelSequence(other.gameObject));
        }
        else if (other.CompareTag("Player") && !isOpen)
        {
            Debug.Log("Locked pa! Need mo pa kumpletuhin ang items.");
        }
    }

    // Ito ang sequence ng pagtatapos
    IEnumerator FinishLevelSequence(GameObject playerObject)
    {
        levelFinished = true; // Lock para di ma-trigger ulit

        // 1. I-STOP ANG PLAYER (Para kunwari nag-pause ang game)
        PlayerMovement movement = playerObject.GetComponent<PlayerMovement>();
        Rigidbody2D rb = playerObject.GetComponent<Rigidbody2D>();

        if (movement != null) movement.enabled = false; // Patayin ang controls
        if (rb != null) rb.linearVelocity = Vector2.zero;     // Tigil ang slide/momentum

        // 2. TUGTUGIN ANG VICTORY SOUND
        if (winSound != null)
        {
            audioSource.PlayOneShot(winSound);
        }

        Debug.Log("Level Finished! Waiting for cutscene...");

        // 3. MAGHINTAY (Wait for seconds)
        yield return new WaitForSeconds(waitBeforeCutscene);

        // 4. LIPAT NA SA CUTSCENE
        SceneManager.LoadScene(cutsceneSceneName);
    }
}