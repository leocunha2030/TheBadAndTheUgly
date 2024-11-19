using UnityEngine;

public class SpawnTrigger : MonoBehaviour
{
    public EnemySpawner spawner; // Referência ao script do EnemySpawner

    private void OnTriggerEnter(Collider other)
    {
        // Verifica se o objeto que entrou no trigger tem a tag "Player"
        if (other.CompareTag("Player"))
        {
            if (spawner != null) // Garante que o spawner está atribuído
            {
                spawner.SpawnEnemy(); // Chama o método de spawn no EnemySpawner
            }

            // Opcional: desativa o trigger após ser ativado
            gameObject.SetActive(false);
        }
    }
}
