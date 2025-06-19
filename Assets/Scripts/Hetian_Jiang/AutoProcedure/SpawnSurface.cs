using UnityEngine;

public class SpawnSurface : MonoBehaviour
{
    public string allowedInteractableName; // e.g., "PlantPot", "ToiletPaper"
    public Transform spawnPoint; // A child empty GameObject that defines where to spawn the object
}
