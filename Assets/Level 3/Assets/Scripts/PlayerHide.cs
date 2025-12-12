using UnityEngine;
using UnityEngine.UI;

public class PlayerHide : MonoBehaviour
{
    [Header("Player")]
    public GameObject player;
    public Transform hidePoint;
    public Transform exitPoint;

    [Header("UI")]
    public Button hideButton;
    public Button exitButton;

    [Header("Enemy Detection")]
    public RandomGuardPatrol enemyAI;
    private bool playerInTrigger = false;


    private bool isHiding = false;

    private void Start()
    {
        hideButton.gameObject.SetActive(false);
        exitButton.gameObject.SetActive(false);

        hideButton.onClick.AddListener(HidePlayer);
        exitButton.onClick.AddListener(ExitHide);
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (!col.CompareTag("Player")) return;

        playerInTrigger = true;

        if (!isHiding && col.CompareTag("Player"))
        {
            hideButton.gameObject.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if (!col.CompareTag("Player")) return;

        playerInTrigger = false;

        if (!isHiding && col.CompareTag("Player"))
        {
            hideButton.gameObject.SetActive(false);
        }
    }

    void HidePlayer()
    {
        isHiding = true;
        hideButton.gameObject.SetActive(false);
        exitButton.gameObject.SetActive(true);

        // Move player inside hiding spot
        player.transform.position = hidePoint.position;
        player.SetActive(false);   // Optional: make player invisible

        // Enemy cannot detect player now
        if (enemyAI != null)
            enemyAI.canDetectPlayer = false;
    }

    void ExitHide()
    {
        isHiding = false;
        exitButton.gameObject.SetActive(false);

        // Move player outside hiding spot
        player.transform.position = exitPoint.position;
        player.SetActive(true);

        // Enable enemy detection again
        if (enemyAI != null)
            enemyAI.canDetectPlayer = true;

        hideButton.gameObject.SetActive(false);
    }
}
