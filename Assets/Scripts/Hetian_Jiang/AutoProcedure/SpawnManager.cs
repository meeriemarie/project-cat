using UnityEngine;

public class SpawnerManager : MonoBehaviour
{
    [System.Serializable]
    public class InteractableMapping
    {
        public string interactableName;
        public GameObject prefab;
    }

    public InteractableMapping[] interactables;

    private void Start()
    {
        SpawnSurface[] surfaces = FindObjectsOfType<SpawnSurface>();

        foreach (var surface in surfaces)
        {
            GameObject prefab = GetPrefabByName(surface.allowedInteractableName);
            if (prefab == null || surface.spawnPoint == null) continue;

            GameObject instance = Instantiate(prefab, surface.spawnPoint.position, Quaternion.identity);
            instance.transform.SetParent(surface.transform);

            // Apply random pastel color
            ColorManager.ApplyRandomPastelColor(instance);
        }
    }

    private GameObject GetPrefabByName(string name)
    {
        foreach (var mapping in interactables)
        {
            if (mapping.interactableName == name)
                return mapping.prefab;
        }
        return null;
    }
}
