using UnityEngine;
using UnityEngine.UI;
using TMPro;

[System.Serializable]
public class DialogueEntry
{
    [TextArea(3, 10)]
    public string sentence;
    public bool isRightSide; // True = Rizal, False = Other
}

public class DialogueManager : MonoBehaviour
{
    [Header("UI References")]
    public GameObject dialoguePanel;
    public TextMeshProUGUI dialogueText;
    public TextMeshProUGUI nameText; // <--- NEW: Drag your NameText object here

    public Image imageLeft;
    public Image imageRight;

    [Header("Character Info")]
    public string leftCharacterName = "Paciano"; // <--- NEW: Type name here
    public Sprite spriteLeft;

    public string rightCharacterName = "Rizal";  // <--- NEW: Type name here
    public Sprite spriteRight;

    [Header("Game Links")]
    public RizalMovement playerScript;

    [Header("The Conversation")]
    public DialogueEntry[] conversation;

    private int index = 0;

    void Start()
    {
        // 1. Setup Images once
        imageLeft.sprite = spriteLeft;
        imageRight.sprite = spriteRight;

        // 2. Freeze Player
        if (playerScript != null)
        {
            playerScript.enabled = false;
            // Also stop the Rigidbody to prevent sliding
            playerScript.GetComponent<Rigidbody2D>().linearVelocity = Vector2.zero;
        }

        // 3. Start Dialogue
        dialoguePanel.SetActive(true);
        index = 0;
        UpdateUI();
    }

    public void OnNextButton()
    {
        index++;
        if (index < conversation.Length)
        {
            UpdateUI();
        }
        else
        {
            EndDialogue();
        }
    }

    void UpdateUI()
    {
        // Update the Sentence
        dialogueText.text = conversation[index].sentence;

        // Update the Images AND the Name
        if (conversation[index].isRightSide)
        {
            // Right Side (Rizal) logic
            imageRight.gameObject.SetActive(true);
            imageLeft.gameObject.SetActive(false);

            // Set Name to Rizal
            nameText.text = rightCharacterName; // <--- NEW
            nameText.alignment = TextAlignmentOptions.Right; // Optional: Align name to right
        }
        else
        {
            // Left Side (Paciano) logic
            imageLeft.gameObject.SetActive(true);
            imageRight.gameObject.SetActive(false);

            // Set Name to Paciano
            nameText.text = leftCharacterName; // <--- NEW
            nameText.alignment = TextAlignmentOptions.Left; // Optional: Align name to left
        }
    }

    void EndDialogue()
    {
        dialoguePanel.SetActive(false);

        if (playerScript != null)
            playerScript.enabled = true;
    }
}