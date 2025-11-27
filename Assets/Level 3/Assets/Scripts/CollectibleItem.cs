using System.Collections;
using UnityEngine;

public class CollectibleItem : MonoBehaviour
{
    public string itemName = "Loot";
    public int itemValue = 1; // pwede coins, score, loot amount
    public AudioClip pickSound;

    private bool collected = false;

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player") && !collected)
        {
            collected = true;

            // Add to inventory or score
            PlayerInventory inventory = col.GetComponent<PlayerInventory>();
            if (inventory != null)
            {
                inventory.AddItem(itemName, itemValue);
            }

            // Play sound
            if (pickSound != null)
                AudioSource.PlayClipAtPoint(pickSound, transform.position, 0.8f);

            // Optional: small pop animation
            StartCoroutine(PickupAnimation());
        }
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