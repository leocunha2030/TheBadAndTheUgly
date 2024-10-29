using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public int currentHealth;
    public Animator animator;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void DamageEnemy(int damage)
    {
        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            animator.SetBool("Dead", true);
            Invoke("DestroyAfterDelay", 3f);
        }
    }

    void DestroyAfterDelay()
    {
        Destroy(gameObject);
    }
}
