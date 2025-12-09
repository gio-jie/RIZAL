using UnityEngine;

public class LockedDoor : MonoBehaviour
{
    public string requiredKey; // Anong susi ang kailangan dito?

    void OnCollisionEnter2D(Collision2D collision)
    {
        // Kapag binangga ni Rizal ang pinto
        if (collision.gameObject.GetComponent<RizalMovement>())
        {
            LevelManager manager = FindFirstObjectByType<LevelManager>();
            
            // Check kung nasa bulsa na ni Rizal ang susi
            if (manager != null && manager.HasKey(requiredKey))
            {
                gameObject.SetActive(false); // Buksan ang pinto (mawala)
            }
            else
            {
                Debug.Log("Naka-lock! Kailangan ng: " + requiredKey);
            }
        }
    }
}