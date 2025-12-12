using UnityEngine;
using UnityEngine.UI;

public class CabinetInteract : MonoBehaviour
{
    [Header("Cabinet Sprites")]
    public Sprite closedSprite;
    public Sprite openedSprite;

    [Header("UI")]
    public Button interactButton;

    [Header("Item Detection")]
    public string itemTag = "LootItem"; // tag used for items that spawn inside cabinets
    
    private bool isPlayerNearby = false;
    private bool isOpened = false;
    private SpriteRenderer sr;

    private void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        interactButton.gameObject.SetActive(false);
        interactButton.onClick.AddListener(OpenCabinet);
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player") && !isOpened)
        {
            isPlayerNearby = true;
            interactButton.gameObject.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            isPlayerNearby = false;
            interactButton.gameObject.SetActive(false);
        }
    }

    private void OpenCabinet()
    {
        if (!isPlayerNearby || isOpened)
            return;

        isOpened = true;
        sr.sprite = openedSprite;
        interactButton.gameObject.SetActive(false);

        // Reveal all items that share this cabinet’s trigger
        EnableNearbyItems();
    }

    private void EnableNearbyItems()
    {
        // Find all objects in the scene with the tag
        GameObject[] items = GameObject.FindGameObjectsWithTag(itemTag);

        foreach (var item in items)
        {
            // Only enable items that are overlapping this cabinet’s trigger collider
            if (GetComponent<Collider2D>().bounds.Contains(item.transform.position))
            {
                item.SetActive(true);
            }
        }
    }
}
