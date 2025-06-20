using UnityEngine;
using System.Collections.Generic;

public class SpawnerManager : MonoBehaviour
{
    public List<GameObject> interactablePrefabs; // name must match allowedInteractableName
    private List<Vector3> usedPositions = new List<Vector3>();

    private void Start()
    {
        SpawnAllInteractables();
    }

    void SpawnAllInteractables()
    {
        SpawnSurface[] surfaces = FindObjectsOfType<SpawnSurface>();

        foreach (SpawnSurface surface in surfaces)
        {
            GameObject prefab = interactablePrefabs.Find(p => p.name == surface.allowedInteractableName);
            if (prefab == null)
            {
                Debug.LogWarning($"No prefab found for surface {surface.name} (expected: {surface.allowedInteractableName})");
                continue;
            }

            int spawnCount = Random.Range(surface.minSpawnCount, surface.maxSpawnCount + 1);

            int attempts = 0;
            while (spawnCount > 0 && attempts < 20)
            {
                Vector3 spawnPos = surface.GetRandomSpawnPoint();

                // Simple overlap prevention (you can improve this)
                bool tooClose = usedPositions.Exists(pos => Vector3.Distance(pos, spawnPos) < 0.2f);
                if (tooClose)
                {
                    attempts++;
                    continue;
                }

                usedPositions.Add(spawnPos);

                GameObject instance = Instantiate(prefab, spawnPos, Quaternion.identity, surface.transform);

                // Automatically add InteractableObject if not present
                if (instance.GetComponent<InteractableObject>() == null)
                {
                    var io = instance.AddComponent<InteractableObject>();
                    io.scoreValue = 10; // default score
                }

                if (instance.GetComponent<Rigidbody>() == null)
                {
                    var io = instance.AddComponent<Rigidbody>();
                }

                spawnCount--;
            }
        }
    }
}
