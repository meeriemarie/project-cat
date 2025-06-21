using UnityEngine;

public class ExitTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        // Make sure it's the player exiting
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player exited through the door");

            // Trigger game won logic
            if (GameManager.Instance != null && !GameManager.Instance.IsGameOver)
            {
                GameManager.Instance.OnGameWon();
            }
        }
    }
}
