using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ItemInteract : MonoBehaviour
{
    [Header("UI")]
    public Button interactButton;

    [Header("Move Settings")]
    public Vector3 targetPosition; // where the rag should move when interacted
    public float moveSpeed = 2f;

    private bool isMoved = false;
    private PlayerInventory playerInventory;

    private void Start()
    {
        if (interactButton != null)
        {
            interactButton.gameObject.SetActive(false);
            interactButton.onClick.AddListener(OnInteract);
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (!col.CompareTag("Player")) return;

        if (col.CompareTag("Player"))
        {
            playerInventory = col.GetComponent<PlayerInventory>();
            if (!isMoved && interactButton != null)
            interactButton.gameObject.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if (!col.CompareTag("Player")) return;

        if (col.CompareTag("Player"))
        {
            playerInventory = null;
            if (!isMoved && interactButton != null)
                interactButton.gameObject.SetActive(false);
        }
    }

    public void OnInteract()
    {
        if (playerInventory == null) return;
        if (isMoved) return;

        StartCoroutine(MoveRag());
    }

    private IEnumerator MoveRag()
    {
        isMoved = true;
        interactButton.gameObject.SetActive(false);

        Vector3 startPos = transform.position;
        Vector3 endPos = targetPosition;
        float t = 0f;

        while (t < 1f)
        {
            t += Time.deltaTime * moveSpeed;
            transform.position = Vector3.Lerp(startPos, endPos, t / moveSpeed);
            yield return null;
        }

        Debug.Log("Rag moved!");
    }
}
