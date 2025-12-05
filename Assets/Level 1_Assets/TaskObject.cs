using UnityEngine;

public class TaskObject : MonoBehaviour
{
    private InteractionManager uiManager;

    void Start()
    {
        // Find the manager automatically
        uiManager = FindFirstObjectByType<InteractionManager>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // 1. Check if the physics engine detects ANYTHING
        Debug.Log("Something touched me: " + other.name);

        // 2. Check if it detects the Player Script specifically
        if (other.GetComponent<RizalMovement>())
        {
            Debug.Log("It is Rizal! Showing Button.");
            uiManager.RegisterObject(this);
        }
        else
        {
            Debug.Log("It is NOT Rizal. It is: " + other.name);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.GetComponent<RizalMovement>())
        {
            uiManager.UnregisterObject(this); // Tell manager: "I am out of range"
        }
    }

    // This function runs when you actually tap the button
    public void OnInteract()
    {
        Debug.Log("Task Object Collected!");

        // --- Future Idea: You could add an ID here ---
        // e.g., if (objectName == "Key") OpenDoor();

        Destroy(gameObject);
    }
}