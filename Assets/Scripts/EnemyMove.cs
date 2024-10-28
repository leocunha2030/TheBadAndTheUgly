using UnityEngine;
using UnityEngine.AI;

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
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        target = PlayerMove.instance.transform.position;
        //target.y = transform.position.y;

        agent.destination = target;

        //ansform.LookAt(target);
        //eRigidbody.linearVelocity = transform.forward * moveSpeed;

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

            if (Mathf.Abs(angle) < 45f)
            {
                Instantiate(bullet, firePoint.position, firePoint.rotation);
                animator.SetTrigger("Fire");
            }
            else
            {
                agent.destination = target;
            }

            if (agent.remainingDistance < 0.3f)
            {
                animator.SetBool("IsMoving", true);
            }
        }
    }
}
