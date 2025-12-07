using UnityEngine;

public class TaskObject : MonoBehaviour
{
    private InteractionManager uiManager;
    private LevelManager levelManager; // <--- ADD THIS

    void Awake()
    {
        uiManager = FindFirstObjectByType<InteractionManager>();
        levelManager = FindFirstObjectByType<LevelManager>(); // <--- ADD THIS
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<RizalMovement>())
        {
            uiManager.RegisterObject(this);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.GetComponent<RizalMovement>())
        {
            uiManager.UnregisterObject(this);
        }
    }

    public void OnInteract()
    {
        // Tell the manager we grabbed an item
        if (levelManager != null)
        {
            levelManager.ItemCollected(); // <--- ADD THIS
        }

        Destroy(gameObject);
    }
}