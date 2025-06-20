using UnityEngine;

public class InteractableObject : MonoBehaviour
{
    public int scoreValue = 10;

    private bool hasInteracted = false; //Track if already interacted

    public void OnPlayerInteract()
    {
        if (hasInteracted) return;

        hasInteracted = true; // Prevent future interactions

        Debug.Log($"{gameObject.name} interacted with!");
        ScoreManager.Instance.AddScore(scoreValue);
    }
}
