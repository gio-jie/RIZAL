using UnityEngine;
using TMPro;
using System.Collections;

public class Drawer : MonoBehaviour
{
    public string requiredKeyName = "DrawerKey"; // Name of the key that opens this drawer
    //public Animator drawerAnimator;             // Optional: animation for opening
    public TextMeshProUGUI messageText;        // UI Text for "Locked" message
    public float messageDuration = 1.5f;

    [Header("Open Movement Settings")]
    public Vector3 openPosition = new Vector3(0f, 3f, 0f);
    public float moveSpeed = 5f;

    private bool isOpen = false;

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (!col.CompareTag("Player")) return;

        PlayerInventory inventory = col.GetComponent<PlayerInventory>();
        if (inventory == null) return;

        if (isOpen) return; // Already opened

        if (inventory.collectedItems.Contains(requiredKeyName))
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
            transform.position = Vector3.Lerp(startPos, targetPos, t);
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
