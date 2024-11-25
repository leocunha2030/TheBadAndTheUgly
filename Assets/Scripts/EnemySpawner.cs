using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyToSpawn; // Prefab do inimigo a ser gerado
    public float spawnDelay = 0f; // Tempo de atraso para spawnar o inimigo (em segundos)
    private bool hasSpawned = false; // Garante que o inimigo é gerado apenas uma vez

    public void TriggerSpawn()
    {
        if (!hasSpawned) // Verifica se já foi gerado
        {
            hasSpawned = true; // Marca como gerado

            if (spawnDelay > 0f)
            {
                // Aguarda o tempo especificado antes de spawnar o inimigo
                StartCoroutine(SpawnWithDelay());
            }
            else
            {
                // Spawna o inimigo imediatamente
                SpawnEnemy();
            }
        }
    }

    private void SpawnEnemy()
    {
        Instantiate(enemyToSpawn, transform.position, transform.rotation); // Gera o inimigo
    }

    private System.Collections.IEnumerator SpawnWithDelay()
    {
        yield return new WaitForSeconds(spawnDelay); // Aguarda o tempo especificado
        SpawnEnemy();
    }
}
