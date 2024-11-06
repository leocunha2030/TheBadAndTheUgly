using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public int currentHealth; // Vida atual do inimigo
    public Animator animator; // Referência ao Animator para animações

    public void DamageEnemy(int damage)
    {
        // Reduz a vida do inimigo
        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            // Aciona animação de morte e agenda destruição do objeto
            animator.SetBool("Dead", true);
            Invoke("DestroyAfterDelay", 3f);
            GameManager.instance.killedEnemies++;
            UI.instance.killedEnemiesText.text = "Kills:........................ " + GameManager.instance.killedEnemies;
        }
    }

    void DestroyAfterDelay()
    {
        // Destroi o objeto inimigo
        Destroy(gameObject);
    }
}
