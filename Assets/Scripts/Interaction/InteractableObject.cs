using UnityEngine;

public class InteractableObject : MonoBehaviour
{
    public DoorController[] doorsToUnlock;

    public void OnPlayerInteract()
    {
        Debug.Log($"{gameObject.name} interacted with!");

        foreach (var door in doorsToUnlock)
        {
            if (door != null)
                door.RegisterInteraction();
        }
    }
}
