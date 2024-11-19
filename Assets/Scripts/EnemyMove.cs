using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class EnemyMove : MonoBehaviour
{
    public float moveSpeed, distanceToStop; // Velocidade de movimento e distância de parada do inimigo
    public float attackRange = 10f; // Distância mínima para o inimigo atacar
    public NavMeshAgent agent; // Componente NavMeshAgent para navegação automática
    public Transform firePoint; // Ponto de onde o inimigo atira
    public float fireRate; // Taxa de tiros
    private float fireCount; // Contador para controlar o tempo entre tiros
    public float fireDelay; // Atraso entre a animação e o tiro
    public GameObject bullet; // Prefab da bala que o inimigo dispara
    public Animator animator; // Componente Animator para controlar animações do inimigo
    public GameObject MuzzleFlash; // Efeito visual do disparo
    private bool isAlive = true; // Variável para controlar se o inimigo está vivo

    void Start()
    {
        if (MuzzleFlash != null)
        {
            MuzzleFlash.SetActive(false); // Desativa o efeito de MuzzleFlash no início
        }
    }

    void Update()
    {
        if (!isAlive) return; // Se o inimigo estiver morto, não executa o restante do código

        Vector3 target = PlayerMove.instance.transform.position;
        float distanceToPlayer = Vector3.Distance(transform.position, target); // Calcula a distância ao jogador

        // Movimento do inimigo
        if (distanceToPlayer > distanceToStop)
        {
            agent.destination = target; // Segue o jogador
            animator.SetBool("IsMoving", true); // Animação de movimento
        }
        else
        {
            agent.destination = transform.position; // Para de se mover
            animator.SetBool("IsMoving", false); // Animação de parada
        }

        // Controle de ataque
        if (distanceToPlayer <= attackRange && distanceToPlayer <= distanceToStop)
        {
            AttackPlayer();
        }
    }

    void AttackPlayer()
    {
        // Controla o tempo entre tiros
        fireCount -= Time.deltaTime;
        if (fireCount <= 0)
        {
            fireCount = fireRate + fireDelay; // Reinicia o contador de tiros com um atraso
            firePoint.LookAt(PlayerMove.instance.transform.position + new Vector3(0f, 1f, 0f)); // Aponta o ponto de tiro para o jogador

            Vector3 targetDirection = PlayerMove.instance.transform.position - transform.position;
            float angle = Vector3.SignedAngle(targetDirection, transform.forward, Vector3.up);

            if (Mathf.Abs(angle) < 15f)
            {
                Instantiate(bullet, firePoint.position, firePoint.rotation); // Cria a bala
                animator.SetTrigger("Fire"); // Dispara a animação de tiro
                StartCoroutine(WaitAndSetActiveFalse()); // Ativa o efeito de MuzzleFlash
            }
        }
    }

    IEnumerator WaitAndSetActiveFalse()
    {
        if (MuzzleFlash != null && !MuzzleFlash.activeSelf)
        {
            MuzzleFlash.SetActive(true); // Ativa o efeito de MuzzleFlash temporariamente
            yield return new WaitForSeconds(0.03f); // Exibe o efeito por um curto período
            MuzzleFlash.SetActive(false);
        }
    }

    // Método para desativar o inimigo
    public void Die()
    {
        isAlive = false; // Atualiza o status do inimigo
        agent.enabled = false; // Desativa o movimento do inimigo
        animator.SetBool("Dead", true); // Aciona a animação de morte
    }
}
