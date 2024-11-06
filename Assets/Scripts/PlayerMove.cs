using UnityEngine;
using System.Collections;

public class PlayerMove : MonoBehaviour
{
    public float moveSpeed; // Velocidade de movimento do jogador
    public float mouseSensitivity; // Sensibilidade do mouse para rotação
    public float gravity = -9.81f; // Valor da gravidade aplicada ao jogador
    public float jumpHeight = 1.5f; // Altura do pulo do jogador
    public float runSpeedMultiplier = 1.5f; // Multiplicador de velocidade para corrida

    public static PlayerMove instance; // Singleton para acesso global

    private Vector3 moveInput; // Armazena o movimento do jogador
    private Vector3 velocity; // Armazena a velocidade vertical do jogador
    public Transform cameraTransform; // Referência à câmera para rotação
    public Animator animator; // Componente Animator para animações do jogador
    public bool canJump = true; // Flag para verificar se o jogador pode pular

    public GameObject bullet; // Prefab da bala disparada pelo jogador
    public Transform firePoint; // Ponto de onde a bala é disparada

    public Transform aimTarget; // Alvo de mira para onde o jogador deve apontar
    public float aimIKTransitionTime = 0.1f; // Tempo de transição para o IK de mira
    private float currentIKWeight = 0f; // Peso atual do IK para suavizar a transição
    public float aimIKTransitionSpeed = 3f; // Velocidade de transição do IK

    public CharacterController characterController; // Controlador de personagem para gerenciar movimento e física

    private int fireCounter = 0; // Contador de tiros disparados
    public float fireRate = 0.5f; // Taxa de disparo
    private float nextFireTime = 0f; // Tempo até o próximo disparo permitido
    public int currentAmmunition = 6; // Munição atual
    public int maxLoadedAmmo = 6; // Capacidade máxima de munição no pente
    public int reserveAmmo = 10; // Munição em reserva

    public GameObject MuzzleFlash; // Efeito visual do disparo
    public GameObject killCountMenu; // Referência ao menu de contagem de kills
    public AudioSource reloadSound; // Som de recarga
    public AudioSource emptyGunSound; // Som para quando não há munição

    private void Awake()
    {
        instance = this; // Configura o singleton para acesso global
    }

    void Start()
    {
        if (MuzzleFlash != null)
        {
            MuzzleFlash.SetActive(false); // Desativa o MuzzleFlash no início
        }
    }

    void Update()
    {
        // Verifica se a tela de pausa está ativa
        if (!UI.instance.pauseScreen.activeInHierarchy)
        {
            // Movimento do jogador
            Vector3 verticalMove = transform.forward * Input.GetAxis("Vertical");
            Vector3 horizontalMove = transform.right * Input.GetAxis("Horizontal");

            float currentSpeed = moveSpeed;
            if (Input.GetKey(KeyCode.LeftShift)) // Aumenta a velocidade ao correr
            {
                currentSpeed *= runSpeedMultiplier;
            }

            moveInput = (horizontalMove + verticalMove).normalized * currentSpeed;

            if (characterController.isGrounded)
            {
                canJump = true;
                velocity.y = -2f;
                if (canJump && Input.GetButtonDown("Jump")) // Salto do jogador
                {
                    velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
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

            // Controle de rotação do jogador com o mouse
            Vector2 mouseInput = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y")) * mouseSensitivity;
            transform.Rotate(Vector3.up * mouseInput.x);
            float newVerticalRotation = cameraTransform.localEulerAngles.x - mouseInput.y;
            newVerticalRotation = (newVerticalRotation > 180) ? newVerticalRotation - 360 : newVerticalRotation;
            newVerticalRotation = Mathf.Clamp(newVerticalRotation, -90f, 90f);
            cameraTransform.localEulerAngles = new Vector3(newVerticalRotation, 0f, 0f);

            // Animações de movimento e controle de mira
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

                bool isAiming = Input.GetMouseButton(1); // Verifica se o jogador está mirando
                animator.SetBool("AimPressed", isAiming);

                // Controle de disparo
                if (isAiming && Input.GetMouseButtonDown(0) && Time.time >= nextFireTime)
                {
                    if (currentAmmunition > 0) // Verifica se há munição
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

                        Instantiate(bullet, firePoint.position, firePoint.rotation); // Cria a bala
                        fireCounter++;
                        currentAmmunition--;
                        nextFireTime = Time.time + fireRate;

                        StartCoroutine(WaitAndSetActiveFalse()); // Ativa o MuzzleFlash temporariamente
                    }
                    else
                    {
                        if (emptyGunSound != null) // Som de arma vazia
                        {
                            emptyGunSound.Play();
                        }
                    }
                }

                // Recarga de munição
                if (Input.GetKeyDown(KeyCode.R) && currentAmmunition < maxLoadedAmmo && reserveAmmo > 0)
                {
                    int ammoNeeded = maxLoadedAmmo - currentAmmunition;
                    int ammoToReload = Mathf.Min(ammoNeeded, reserveAmmo);
                    currentAmmunition += ammoToReload;
                    reserveAmmo -= ammoToReload;

                    if (reloadSound != null)
                    {
                        reloadSound.Play();
                    }
                }
            }

            // Controle do menu de kills
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
            yield return new WaitForSeconds(0.03f); // Exibe o MuzzleFlash por um curto período
            MuzzleFlash.SetActive(false);
        }
    }

    void OnAnimatorIK(int layerIndex)
    {
        if (animator != null)
        {
            if (Input.GetMouseButton(1)) // Se o jogador está mirando
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
                animator.SetIKPositionWeight(AvatarIKGoal.RightHand, 0);
                animator.SetIKRotationWeight(AvatarIKGoal.RightHand, 0);
                animator.SetLookAtWeight(0);
            }
        }
    }
}
