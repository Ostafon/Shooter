using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemySpawner : MonoBehaviour
{
    public GameObject[] enemyPrefabs;
    public int numberOfEnemiesToSpawn = 5;
    public static int totalEnemies;

    void Start()
    { 
        totalEnemies = numberOfEnemiesToSpawn + 5;
        SpawnEnemies();
    }

    void Update()
    {
        HandleEnemyDeath();
    }

    void SpawnEnemies()
    {
        Terrain terrain = Terrain.activeTerrain;
        if (terrain == null)
        {
            Debug.LogError("Terrain not found!");
            return;
        }

        float terrainWidth = terrain.terrainData.size.x;
        float terrainLength = terrain.terrainData.size.z;

        for (int i = 0; i < numberOfEnemiesToSpawn; i++)
        {
           
            Vector3 randomPosition = GetRandomPositionOnTerrain(terrain, terrainWidth, terrainLength);
            float y = terrain.SampleHeight(randomPosition) + terrain.GetPosition().y;

            
            GameObject enemyPrefab = enemyPrefabs[Random.Range(0, enemyPrefabs.Length)];
            Instantiate(enemyPrefab, new Vector3(randomPosition.x, y, randomPosition.z), Quaternion.identity);
        }
    }

    Vector3 GetRandomPositionOnTerrain(Terrain terrain, float width, float length)
    {
        float randomX = Random.Range(0, width);
        float randomZ = Random.Range(0, length);
        float y = terrain.SampleHeight(new Vector3(randomX, 0f, randomZ));
        return new Vector3(randomX, y, randomZ);
    }

    private void HandleEnemyDeath()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        if (enemies.Length == 0)
        {
            SceneManager.LoadScene("WinMenu");
        }
    }
}
