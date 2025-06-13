using UnityEngine;

public class InteractableTrigger : MonoBehaviour
{
    private InteractableObject parentInteractable;

    private void Awake()
    {
        parentInteractable = GetComponentInParent<InteractableObject>();
        if (parentInteractable == null)
            Debug.LogError("No InteractableObject found in parent!");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            parentInteractable.OnPlayerInteract();
        }
    }
}
