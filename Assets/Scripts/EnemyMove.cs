using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class EnemyMove : MonoBehaviour
{
    public float moveSpeed, distanceToStop;

    public NavMeshAgent agent;
    private Vector3 target;

    public Transform firePoint;
    public float fireRate;
    private float fireCount;
    public float fireDelay; // Delay do tiro para sincronizar a animação
    public GameObject bullet;
    public Animator animator;

    public GameObject MuzzleFlash; // Adicionando a variável do MuzzleFlash

    // Start is called before the first frame update
    void Start()
    {
        if (MuzzleFlash != null)
        {
            MuzzleFlash.SetActive(false); // Garantir que o MuzzleFlash esteja desativado ao iniciar
        }
    }

    // Update is called once per frame
    void Update()
    {
        target = PlayerMove.instance.transform.position;

        agent.destination = target;

        if (Vector3.Distance(transform.position, target) > distanceToStop)
        {
            agent.destination = target;
        }
        else
        {
            agent.destination = transform.position;
        }

        fireCount -= Time.deltaTime;

        if (fireCount <= 0)
        {
            fireCount = fireRate + fireDelay; // Adiciona o delay do tiro
            firePoint.LookAt(PlayerMove.instance.transform.position + new Vector3(0f, 1f, 0f));

            Vector3 targetDirection = PlayerMove.instance.transform.position - transform.position;
            float angle = Vector3.SignedAngle(targetDirection, transform.forward, Vector3.up);

            if (Mathf.Abs(angle) < 15f)
            {
                Instantiate(bullet, firePoint.position, firePoint.rotation);
                animator.SetTrigger("Fire");

                // Ativar o MuzzleFlash quando a bala for instanciada
                StartCoroutine(WaitAndSetActiveFalse());
            }
            else
            {
                agent.destination = target;
            }

            if (agent.remainingDistance < 0.3f)
            {
                animator.SetBool("IsMoving", false);
            }
            else
            {
                animator.SetBool("IsMoving", true);
            }
        }
    }

    IEnumerator WaitAndSetActiveFalse()
    {
        if (MuzzleFlash != null && !MuzzleFlash.activeSelf)
        {
            MuzzleFlash.SetActive(true);
            yield return new WaitForSeconds(0.03f); // Duração do MuzzleFlash
            MuzzleFlash.SetActive(false);
        }
    }
}
