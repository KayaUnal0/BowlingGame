using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject obstaclePrefab;
    [SerializeField] private Transform laneReference;

    [Header("Spawn Area")]
    [SerializeField] private float minX = -3f;
    [SerializeField] private float maxX = 3f;
    [SerializeField] private float spawnY = 0.5f;

    [Header("Fixed Z Positions")]
    [SerializeField] private float[] zPositions;

    [Header("Random Scale")]
    [SerializeField] private float minScaleX = 3f;
    [SerializeField] private float maxScaleX = 6f;

    [SerializeField] private float minScaleZ = 3f;
    [SerializeField] private float maxScaleZ = 6f;

    [SerializeField] private float scaleY = 1f;

    [Header("Random Settings")]
    [SerializeField] private bool spawnOnStart = true;

    private void Start()
    {
        if (spawnOnStart)
        {
            SpawnObstacles();
        }
    }

    public void SpawnObstacles()
    {
        if (obstaclePrefab == null)
        {
            Debug.LogError("Obstacle prefab is missing.");
            return;
        }

        if (laneReference == null)
        {
            Debug.LogError("Lane reference is missing.");
            return;
        }

        foreach (float z in zPositions)
        {
            float randomX = Random.Range(minX, maxX);

            Vector3 localPosition = new Vector3(randomX, spawnY, z);
            Vector3 worldPosition = laneReference.TransformPoint(localPosition);

            GameObject obstacle = Instantiate(
                obstaclePrefab,
                worldPosition,
                laneReference.rotation
            );

            float randomScaleX = Random.Range(minScaleX, maxScaleX);
            float randomScaleZ = Random.Range(minScaleZ, maxScaleZ);

            obstacle.transform.localScale = new Vector3(
                randomScaleX,
                scaleY,
                randomScaleZ
            );
        }
    }
}