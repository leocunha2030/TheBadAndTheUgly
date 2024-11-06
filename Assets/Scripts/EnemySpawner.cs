using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyToSpawn; // Prefab do inimigo a ser gerado
    public float timeToSpawn; // Intervalo entre os spawns de inimigos
    private float spawnCounter; // Contador para controlar o tempo entre spawns

    void Start()
    {
        spawnCounter = timeToSpawn; // Inicializa o contador com o valor do intervalo de spawn
    }

    void Update()
    {
        spawnCounter -= Time.deltaTime; // Decrementa o contador

        if (spawnCounter <= 0)
        {
            // Gera um novo inimigo e reinicia o contador
            spawnCounter = timeToSpawn;
            Instantiate(enemyToSpawn, transform.position, transform.rotation);
        }
    }
}
