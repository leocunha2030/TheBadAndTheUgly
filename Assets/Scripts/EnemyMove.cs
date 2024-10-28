using UnityEngine;
using UnityEngine.AI;

public class EnemyMove : MonoBehaviour
{
    public float moveSpeed, distanceToStop;
    
    public NavMeshAgent agent;
    private Vector3 target;

    public Transform firePoint;
    public float fireRate, waitBetweenShots = 2f, timeToShoot = 1f;
    private float fireCount, shotwaitCounter, shootTimeCounter;
    public GameObject bullet;
    public Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        shootTimeCounter = timeToShoot;
        shotwaitCounter = waitBetweenShots;
    }

    // Update is called once per frame
    void Update()
    {
        target = PlayerMove.instance.transform.position;

        agent.destination = target;

        if (Vector3.Distance(transform.position, target) > distanceToStop)
        {
            shootTimeCounter = timeToShoot;
            shotwaitCounter = waitBetweenShots;

            agent.destination = target;
            animator.SetBool("IsMoving", true);
        }
        else
        {
            agent.destination = transform.position;
            animator.SetTrigger("Fire");
        }

        if(shotwaitCounter > 0)
        {
            shotwaitCounter -= Time.deltaTime;

            if(shotwaitCounter <= 0)
            {
                shootTimeCounter = timeToShoot;
            }
        }
        else
        {
            shootTimeCounter -= Time.deltaTime;

            if(shootTimeCounter > 0)
            {
                fireCount -= Time.deltaTime;

                if (fireCount <= 0)
                {
                    fireCount = fireRate;

                    firePoint.LookAt(PlayerMove.instance.transform.position + new Vector3(0f, 1f, 0f));

                    Vector3 targetDirection = PlayerMove.instance.transform.position - transform.position;
                    float angle = Vector3.SignedAngle(targetDirection, transform.forward, Vector3.up);

                    if (Mathf.Abs(angle) < 30f)
                    {
                        Instantiate(bullet, firePoint.position, firePoint.rotation);
                    }
                    else
                    {
                        transform.LookAt(PlayerMove.instance.transform.position);
                        shotwaitCounter = waitBetweenShots;
                    }

                }

            }
            else
            {
                shotwaitCounter = waitBetweenShots;
            }
        }

       
    }
}
