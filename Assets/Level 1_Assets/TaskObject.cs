using UnityEngine;

public class TaskObject : MonoBehaviour
{
    private InteractionManager uiManager;
    private LevelManager levelManager; // <--- ADD THIS

    [Header("Settings")]
    [Tooltip("Iwanang blanko kung Score Item. Lagyan ng pangalan kung Susi.")]
    public string keyName = ""; // Ito ang sikreto natin    

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
        if (levelManager != null)
        {
            // LOGIC: Check natin kung Susi ba 'to o Score Item
            
            if (keyName != "") 
            {
                // CASE 1: May laman ang keyName, so SUSI ito!
                levelManager.AddKey(keyName);
            }
            else
            {
                // CASE 2: Walang laman (Blank), so SCORE ITEM (Gamot/Sulat) ito!
                levelManager.ItemCollected();
            }
        }

        // Burahin ang object sa map
        Destroy(gameObject);
    }
}