using UnityEngine;
using System.Collections;

public class PlayerMove : MonoBehaviour
{
    public float moveSpeed;
    public float mouseSensitivity;
    public float gravity = -9.81f;
    public float jumpHeight = 1.5f;
    public float runSpeedMultiplier = 1.5f;
    public float jumpVelocity;

    public static PlayerMove instance;

    private Vector3 moveInput;
    private Vector3 velocity;
    public Transform cameraTransform;
    public Animator animator;
    public bool canJump = true;

    public GameObject bullet;
    public Transform firePoint;

    public Transform aimTarget;
    public float aimIKTransitionTime = 0.1f;
    private float currentIKWeight = 0f;
    public float aimIKTransitionSpeed = 3f;

    public CharacterController characterController;

    private int fireCounter = 0;
    public float fireRate = 0.5f;
    private float nextFireTime = 0f;
    public int currentAmmunition = 6;
    public int maxLoadedAmmo = 6;
    public int reserveAmmo = 10;

    public GameObject MuzzleFlash;
    public GameObject killCountMenu; // Referência ao menu de contagem de kills
    public AudioSource reloadSound; // Referência ao som de recarga

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        if (MuzzleFlash != null)
        {
            MuzzleFlash.SetActive(false);
        }
    }

    void Update()
    {
        if (!UI.instance.pauseScreen.activeInHierarchy)
        {
            Vector3 verticalMove = transform.forward * Input.GetAxis("Vertical");
            Vector3 horizontalMove = transform.right * Input.GetAxis("Horizontal");

            float currentSpeed = moveSpeed;
            if (Input.GetKey(KeyCode.LeftShift))
            {
                currentSpeed *= runSpeedMultiplier;
            }

            moveInput = (horizontalMove + verticalMove).normalized * currentSpeed;

            if (characterController.isGrounded)
            {
                canJump = true;
                velocity.y = -2f;
                if (canJump && Input.GetButtonDown("Jump"))
                {
                    velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
                    jumpVelocity = velocity.y;
                    canJump = false;
                    animator.SetBool("Jumping", true);
                }
            }
            else
            {
                animator.SetBool("Jumping", false);
            }

            velocity.y += gravity * Time.deltaTime;
            characterController.Move((moveInput + new Vector3(0, velocity.y, 0)) * Time.deltaTime);

            Vector2 mouseInput = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y")) * mouseSensitivity;
            transform.Rotate(Vector3.up * mouseInput.x);
            float newVerticalRotation = cameraTransform.localEulerAngles.x - mouseInput.y;
            newVerticalRotation = (newVerticalRotation > 180) ? newVerticalRotation - 360 : newVerticalRotation;
            newVerticalRotation = Mathf.Clamp(newVerticalRotation, -90f, 90f);
            cameraTransform.localEulerAngles = new Vector3(newVerticalRotation, 0f, 0f);

            if (animator != null)
            {
                if (moveInput == Vector3.zero)
                {
                    animator.SetFloat("Speed", 0);
                }
                else if (!Input.GetKey(KeyCode.LeftShift))
                {
                    animator.SetFloat("Speed", 0.5f);
                }
                else
                {
                    animator.SetFloat("Speed", 1);
                }

                bool isAiming = Input.GetMouseButton(1);
                animator.SetBool("AimPressed", isAiming);

                if (isAiming && Input.GetMouseButtonDown(0) && Time.time >= nextFireTime && currentAmmunition > 0)
                {
                    animator.SetTrigger("Shooting");
                    RaycastHit hit;
                    if (Physics.Raycast(cameraTransform.position, cameraTransform.forward, out hit, 200f))
                    {
                        if (Vector3.Distance(cameraTransform.position, hit.point) > 1f)
                        {
                            firePoint.LookAt(hit.point);
                        }
                        else
                        {
                            firePoint.LookAt(cameraTransform.position + (cameraTransform.forward * 40f));
                        }
                    }

                    Instantiate(bullet, firePoint.position, firePoint.rotation);
                    fireCounter++;
                    currentAmmunition--;
                    nextFireTime = Time.time + fireRate;
                    Debug.Log("Tiros disparados: " + fireCounter);
                    Debug.Log("Munição restante no pente: " + currentAmmunition);
                    Debug.Log("Munição em reserva: " + reserveAmmo);

                    StartCoroutine(WaitAndSetActiveFalse());
                }

                if (Input.GetKeyDown(KeyCode.R) && currentAmmunition < maxLoadedAmmo && reserveAmmo > 0)
                {
                    int ammoNeeded = maxLoadedAmmo - currentAmmunition;
                    int ammoToReload = Mathf.Min(ammoNeeded, reserveAmmo);
                    currentAmmunition += ammoToReload;
                    reserveAmmo -= ammoToReload;
                    Debug.Log("Recarregando... Munição atual: " + currentAmmunition + ", Munição em reserva: " + reserveAmmo);

                    if (reloadSound != null)
                    {
                        reloadSound.Play();
                    }
                }
            }

            if (Input.GetKeyDown(KeyCode.Tab))
            {
                killCountMenu.SetActive(true);
            }

            if (Input.GetKeyUp(KeyCode.Tab))
            {
                killCountMenu.SetActive(false);
            }
        }
    }

    IEnumerator WaitAndSetActiveFalse()
    {
        if (MuzzleFlash != null && !MuzzleFlash.activeSelf)
        {
            MuzzleFlash.SetActive(true);
            yield return new WaitForSeconds(0.03f);
            MuzzleFlash.SetActive(false);
        }
    }

    void OnAnimatorIK(int layerIndex)
    {
        if (animator != null)
        {
            if (Input.GetMouseButton(1))
            {
                currentIKWeight = Mathf.Clamp01(currentIKWeight + Time.deltaTime * aimIKTransitionSpeed);
                animator.SetIKPositionWeight(AvatarIKGoal.RightHand, currentIKWeight);
                animator.SetIKRotationWeight(AvatarIKGoal.RightHand, currentIKWeight);
                animator.SetIKPosition(AvatarIKGoal.RightHand, aimTarget.position);
                animator.SetIKRotation(AvatarIKGoal.RightHand, aimTarget.rotation);
                animator.SetLookAtWeight(currentIKWeight);
                animator.SetLookAtPosition(aimTarget.position);
            }
            else
            {
                currentIKWeight = Mathf.Clamp01(currentIKWeight - Time.deltaTime * aimIKTransitionSpeed);
                animator.SetIKPositionWeight(AvatarIKGoal.RightHand, currentIKWeight);
                animator.SetIKRotationWeight(AvatarIKGoal.RightHand, currentIKWeight);
                animator.SetLookAtWeight(currentIKWeight);
            }
        }
    }
}
