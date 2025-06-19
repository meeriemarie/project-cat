using UnityEngine;

public class DoorController : MonoBehaviour
{
    public int requiredInteractions = 1;
    private bool isUnlocked = false;
    public bool openAtStart = false;

    private void Start()
    {
        if (openAtStart)
        {
            OpenDoor();
            isUnlocked = true;
        }
    }

    public void OpenDoor()
    {
        // For now, just disable the door to �open� it.
        gameObject.SetActive(false);
        Debug.Log($"{gameObject.name} unlocked!");
    }
}
