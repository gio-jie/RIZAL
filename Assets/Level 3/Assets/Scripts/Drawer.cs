using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.UI;

public class Drawer : MonoBehaviour
{
    public string requiredKeyName = "DrawerKey"; // Name of the key that opens this drawer
    //public Animator drawerAnimator;             // Optional: animation for opening
    public TextMeshProUGUI messageText;        // UI Text for "Locked" message
    public float messageDuration = 1.5f;

    [Header("Open Movement Settings")]
    public Vector3 openPosition = new Vector3(-33f, 2.5f, 0f);
    public float moveSpeed = 1.5f;

    private bool isOpen = false;
    private PlayerInventory playerInventory;
    public Button interactButton;

    private void Start()
    {
        if (messageText != null)
        {
            messageText.gameObject.SetActive(false);
        }

        if (interactButton != null)
            interactButton.gameObject.SetActive(false);
            interactButton.onClick.AddListener(OnInteract);
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (!col.CompareTag("Player")) return;
        playerInventory = col.GetComponent<PlayerInventory>();

        if (interactButton != null)
            interactButton.gameObject.SetActive(true);
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if (!col.CompareTag("Player")) return;

        playerInventory = null;
        if (interactButton != null)
            interactButton.gameObject.SetActive(false);
    }

    public void OnInteract()
    {
        if (playerInventory == null) return;
        if (isOpen) return;

        if (playerInventory.collectedItems.Contains(requiredKeyName))
        {
            StartCoroutine(OpenDrawer());
        }
        else
        {
            StartCoroutine(ShowLockedMessage());
        }
    }

    IEnumerator OpenDrawer()
    {
        isOpen = true;

        Vector3 startPos = transform.position;
        Vector3 targetPos = openPosition;

        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime * moveSpeed;
            transform.position = Vector3.Lerp(startPos, targetPos, t / messageDuration);
            yield return null;
        }

        Debug.Log("Drawer opened!");
    }

    IEnumerator ShowLockedMessage()
    {
        if (messageText != null)
        {
            messageText.text = "Locked, find the key!";
            messageText.gameObject.SetActive(true);

            yield return new WaitForSeconds(messageDuration);

            messageText.gameObject.SetActive(false);
        }
    }
}
