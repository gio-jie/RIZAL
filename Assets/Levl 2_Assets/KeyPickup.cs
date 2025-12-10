using UnityEngine;

public class KeyPickup : MonoBehaviour
{
    public string keyName; // Dito natin ilalagay pangalan ng susi sa Inspector

    void OnTriggerEnter2D(Collider2D other)
    {
        // Kapag nabunggo ni Rizal
        if (other.GetComponent<RizalMovement>())
        {
            // Tawagin ang LevelManager
            LevelManager manager = FindFirstObjectByType<LevelManager>();
            if (manager != null)
            {
                manager.AddKey(keyName); // Ibigay ang susi
                Destroy(gameObject);     // Burahin ang susi sa map
            }
        }
    }
}