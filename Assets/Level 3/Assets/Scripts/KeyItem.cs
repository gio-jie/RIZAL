using UnityEngine;

public class KeyItem : MonoBehaviour
{
    public string keyName = "DrawerKey"; // Unique key name
    public AudioClip pickSound;

    private bool collected = false;

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player") && !collected)
        {
            collected = true;

            PlayerInventory inventory = col.GetComponent<PlayerInventory>();
            if (inventory != null)
            {
                inventory.AddItem(keyName, 0); // Add key to inventory
            }

            // Play sound if any
            if (pickSound != null)
                AudioSource.PlayClipAtPoint(pickSound, transform.position, 0.8f);

            Destroy(gameObject); // Remove key from scene
        }
    }
}
