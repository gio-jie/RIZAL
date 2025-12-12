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
    public string itemTag = "LootItem";

    [Header("Transform Change When Closed")]
    public bool applyClosedTransform = true;
    public Vector3 closedPosition;   // target position when closed
    public Vector3 closedScale = Vector3.one; // target scale when closed

    private bool isPlayerNearby = false;
    private bool isOpened = false;
    private SpriteRenderer sr;

    private void Start()
    {
        sr = GetComponent<SpriteRenderer>();

        // Apply closed transform if the cabinet starts in closed state
        if (!isOpened)
        {
            sr.sprite = closedSprite;
        }

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
        ApplyClosedTransform();
        if (!isPlayerNearby || isOpened)
            return;

        isOpened = true;
        sr.sprite = openedSprite;
        interactButton.gameObject.SetActive(false);

        EnableNearbyItems();
    }

    public void CloseCabinet()
    {
        isOpened = false;
        sr.sprite = closedSprite;
    }

    private void ApplyClosedTransform()
    {
        if (!applyClosedTransform) return;

        transform.localPosition = closedPosition;
        transform.localScale = closedScale;
    }

    private void EnableNearbyItems()
    {
        GameObject[] items = GameObject.FindGameObjectsWithTag(itemTag);

        foreach (var item in items)
        {
            if (GetComponent<Collider2D>().bounds.Contains(item.transform.position))
            {
                item.SetActive(true);
            }
        }
    }
}
