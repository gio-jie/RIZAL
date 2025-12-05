using UnityEngine;
using UnityEngine.UI;

public class InteractionManager : MonoBehaviour
{
    public Button interactButton;

    // Changed from 'Manuscript' to 'TaskObject'
    private TaskObject currentTarget;

    void Start()
    {
        interactButton.gameObject.SetActive(false);
        interactButton.onClick.AddListener(OnInteractPressed);
    }

    // Generic Register function
    public void RegisterObject(TaskObject item)
    {
        currentTarget = item;
        interactButton.gameObject.SetActive(true); // Show Button
    }

    // Generic Unregister function
    public void UnregisterObject(TaskObject item)
    {
        // Only hide if the item leaving is the one we are currently targeting
        if (currentTarget == item)
        {
            currentTarget = null;
            interactButton.gameObject.SetActive(false); // Hide Button
        }
    }

    public void OnInteractPressed()
    {
        if (currentTarget != null)
        {
            currentTarget.OnInteract(); // Run the object's specific logic

            currentTarget = null;
            interactButton.gameObject.SetActive(false);
        }
    }
}