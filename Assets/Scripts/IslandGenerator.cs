using UnityEngine;

public class IslandGenerator : MonoBehaviour
{
    public GameObject centerIsland;
    public GameObject islandPrefab;
    public float radius = 10f;
    public int numberOfIslands;

    private void Start()
    {
        PlaceIslands();
    }

    void PlaceIslands()
    {
        float angleStep = 360f / numberOfIslands;
        
        for (int i = 0; i < numberOfIslands; i++)
        {
            float angle = i * angleStep;
            Vector3 islandPosition = CalculatePosition(angle, radius);
            Quaternion randomYRotation = Quaternion.Euler(0, Random.Range(0f, 360f), 0);
            Instantiate(islandPrefab, islandPosition, randomYRotation);
        }
    }

    Vector3 CalculatePosition(float angle, float radius)
    {
        float angleInRadius = angle * Mathf.Deg2Rad;
        float x = centerIsland.transform.position.x + radius * Mathf.Cos(angleInRadius);
        float z = centerIsland.transform.position.z + radius * Mathf.Sin(angleInRadius);
        return new Vector3(x, centerIsland.transform.position.y, z);
    }
}