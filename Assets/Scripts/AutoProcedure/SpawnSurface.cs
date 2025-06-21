using UnityEngine;

[RequireComponent(typeof(Collider))]
public class SpawnSurface : MonoBehaviour
{
    public string allowedInteractableName;
    public float margin = 0.2f;
    public int minSpawnCount = 1;
    public int maxSpawnCount = 3;

    public Vector3 GetRandomSpawnPoint()
    {
        Collider col = GetComponent<Collider>();
        Bounds bounds = col.bounds;

        float minX = bounds.min.x + margin;
        float maxX = bounds.max.x - margin;
        float minZ = bounds.min.z + margin;
        float maxZ = bounds.max.z - margin;

        Vector3 randomPoint = new Vector3(
            Random.Range(minX, maxX),
            bounds.max.y + 1f,
            Random.Range(minZ, maxZ)
        );

        if (Physics.Raycast(randomPoint, Vector3.down, out RaycastHit hit, 2f))
        {
            return hit.point;
        }

        return bounds.center;
    }
}
