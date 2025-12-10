using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CollectibleItem : MonoBehaviour
{
    public string itemName = "Loot";
    public int itemValue = 1; // pwede coins, score, loot amount
    public AudioClip pickSound;

    private bool collected = false;
    private Button interactButton;

    // private void OnTriggerEnter2D(Collider2D col)
    // {
    //     if (col.CompareTag("Player") && !collected)
    //     {
    //         if (interactButton != null && interactButton.gameObject.activeSelf)
    //         {
    //             return;
    //         }
            
    //         collected = true;

    //         // Add to inventory or score
    //         PlayerInventory inventory = col.GetComponent<PlayerInventory>();
    //         if (inventory != null)
    //         {
    //             inventory.AddItem(itemName, itemValue);
    //         }

    //         // Play sound
    //         if (pickSound != null)
    //             AudioSource.PlayClipAtPoint(pickSound, transform.position, 0.8f);

    //         // Optional: small pop animation
    //         StartCoroutine(PickupAnimation());
    //     }
    // }

    private void OnTriggerStay2D(Collider2D col)
    {
        if (col.CompareTag("Player") && !collected)
        {
            // Only collect if interact button is inactive
            interactButton = FindObjectOfType<ItemInteract>()?.interactButton??FindObjectOfType<Drawer>()?.interactButton;
            if (interactButton != null && interactButton.gameObject.activeSelf)
                return;
            
            if (interactButton != null)
            {
                // Delay collection by 2 seconds
                StartCoroutine(DelayedCollect(col, 2f));
            }
            else
            {
                // Collect immediately for items that don't require interaction
                CollectItem(col);
            }
        }
    }

    private IEnumerator DelayedCollect(Collider2D col, float delay)
    {
        collected = true; // prevent multiple collections
        yield return new WaitForSeconds(delay);

        CollectItem(col);
    }

    private void CollectItem(Collider2D player)
    {
        collected = true;

        PlayerInventory inventory = player.GetComponent<PlayerInventory>();
        if (inventory != null)
            inventory.AddItem(itemName, itemValue);

        if (pickSound != null)
            AudioSource.PlayClipAtPoint(pickSound, transform.position, 0.8f);

        StartCoroutine(PickupAnimation());
    }

    IEnumerator PickupAnimation()
    {
        float duration = 0.15f;
        Vector3 startScale = transform.localScale;
        Vector3 endScale = Vector3.zero;

        float t = 0;
        while (t < duration)
        {
            t += Time.deltaTime;
            transform.localScale = Vector3.Lerp(startScale, endScale, t / duration);
            yield return null;
        }

        Destroy(gameObject);
    }
}