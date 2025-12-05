using UnityEngine;
using UnityEngine.UI; // For Images
using TMPro;          // For Text

// This little class lets us type dialogue nicely in the Inspector
[System.Serializable]
public class DialogueEntry
{
    [TextArea(3, 10)]
    public string sentence;       // What they say
    public bool isRightSide;      // If checked, Despujol speaks. If empty, Rizal speaks.
}

public class DialogueManager : MonoBehaviour
{
    [Header("UI References")]
    public GameObject dialoguePanel;
    public TextMeshProUGUI dialogueText;
    public Image imageLeft;   // RIzal's spot
    public Image imageRight;  // Despujol's spot

    [Header("Character Portraits")]
    public Sprite spriteRizal; // Drag Rizal's face here
    public Sprite spriteDespujol;   // Drag Despujol's face here

    [Header("Game Links")]
    public RizalMovement playerScript; // Link to freeze movement

    [Header("The Conversation")]
    public DialogueEntry[] conversation; // We will fill this in Unity

    private int index = 0;

    void Start()
    {
        // 1. Setup Images
        imageLeft.sprite = spriteRizal;
        imageRight.sprite = spriteDespujol;

        // 2. Freeze the Player
        if (playerScript != null)
            playerScript.enabled = false;

        // 3. Start Dialogue
        dialoguePanel.SetActive(true);
        index = 0;
        UpdateUI();
    }

    public void OnNextButton()
    {
        index++; // Move to next line

        if (index < conversation.Length)
        {
            UpdateUI(); // Show next line
        }
        else
        {
            EndDialogue(); // Finish
        }
    }

    void UpdateUI()
    {
        // 1. Update the Text
        dialogueText.text = conversation[index].sentence;

        // 2. Decide which image to highlight
        if (conversation[index].isRightSide)
        {
            // Right (Despujol) is talking
            imageRight.gameObject.SetActive(true);
            imageLeft.gameObject.SetActive(false); // Hide Despujol
        }
        else
        {
            // Left (Rizal) is talking
            imageLeft.gameObject.SetActive(true); // Show Despujol
            imageRight.gameObject.SetActive(false);
        }
    }

    void EndDialogue()
    {
        // Close the panel
        dialoguePanel.SetActive(false);

        // Unfreeze the Player so the game begins
        if (playerScript != null)
            playerScript.enabled = true;
    }
}