using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public int currentHealth; // Vida atual do inimigo
    public Animator animator; // Referência ao Animator para animações
    private EnemyMove enemyMove; // Referência ao script de movimento

    void Start()
    {
        enemyMove = GetComponent<EnemyMove>(); // Obtém a referência ao script EnemyMove
    }

    public void DamageEnemy(int damage)
    {
        currentHealth -= damage; // Reduz a vida do inimigo

        if (currentHealth <= 0)
        {
            if (enemyMove != null)
            {
                enemyMove.Die(); // Desativa o movimento e ataque do inimigo
            }
            Invoke("DestroyAfterDelay", 3f); // Agende a destruição do objeto
            GameManager.instance.killedEnemies++;
            UI.instance.killedEnemiesText.text = "Kills:........................ " + GameManager.instance.killedEnemies;
        }
    }

    void DestroyAfterDelay()
    {
        Destroy(gameObject); // Destroi o objeto inimigo
    }
}
