using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyToSpawn; // Prefab do inimigo a ser gerado
    private bool hasSpawned = false; // Garante que o inimigo é gerado apenas uma vez

    public void SpawnEnemy()
    {
        if (!hasSpawned) // Verifica se já foi gerado
        {
            Instantiate(enemyToSpawn, transform.position, transform.rotation); // Gera o inimigo
            hasSpawned = true; // Marca como gerado
        }
    }
}
