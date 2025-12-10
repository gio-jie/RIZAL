using UnityEngine;

public class Interactable : MonoBehaviour
{
    public bool IsInteracted { get; protected set; } = false;

    public virtual void Interact()
    {
        IsInteracted = true;
    }
}
