using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public float moveSpeed;
    public CharacterController characterController;
    public float mouseSensitivity;
    public float gravity = -9.81f;
    public float jumpHeight = 1.5f;
    public float runSpeedMultiplier = 1.5f;

    private Vector3 moveInput;
    private Vector3 velocity;
    public Transform cameraTransform;
    public Animator animator;
    public bool canJump = true;

    public GameObject bullet;
    public Transform firePoint;

    [Header("Inverse Kinematics Settings")]
    public Transform aimTarget; // O alvo para o qual a mão/arma deve apontar
    public float ikWeight = 1.0f; // Peso do IK (de 0 a 1)

    private float verticalRotation = 0f;
    public float maxVerticalAngle = 60f;

    void Start()
    {
    }

    void Update()
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
                canJump = false;
            }
        }
        else
        {
            canJump = false;
        }

        velocity.y += gravity * Time.deltaTime;

        characterController.Move((moveInput + velocity) * Time.deltaTime);

        // Rotação da câmera e do personagem
        Vector2 mouseInput = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y")) * mouseSensitivity;

        // Rotação horizontal do corpo
        transform.Rotate(Vector3.up * mouseInput.x);

        // Rotação vertical da câmera
        verticalRotation -= mouseInput.y;
        verticalRotation = Mathf.Clamp(verticalRotation, -maxVerticalAngle, maxVerticalAngle);
        cameraTransform.localEulerAngles = new Vector3(verticalRotation, 0f, 0f);

        // Atualizar o Animator para idle, walk, run e aim
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

            // Animação de tiro
            if (isAiming && Input.GetMouseButtonDown(0))
            {
                animator.SetBool("Shooting", true);
                Instantiate(bullet, firePoint.position, firePoint.rotation);
            }
            else
            {
                animator.SetBool("Shooting", false);
            }
        }

        // Atualizar a posição do alvo do IK para a direção da mira
        if (aimTarget != null)
        {
            aimTarget.position = cameraTransform.position + cameraTransform.forward * 10f; // Ponto alvo à frente da câmera
        }
    }

    void OnAnimatorIK(int layerIndex)
    {
        if (animator != null)
        {
            bool isAiming = Input.GetMouseButton(1); // Verifica se o jogador está mirando

            if (isAiming)
            {
                // Definir a posição da mão direita para o alvo da mira
                animator.SetIKPositionWeight(AvatarIKGoal.RightHand, ikWeight);
                animator.SetIKRotationWeight(AvatarIKGoal.RightHand, ikWeight);
                animator.SetIKPosition(AvatarIKGoal.RightHand, aimTarget.position);
                animator.SetIKRotation(AvatarIKGoal.RightHand, aimTarget.rotation);
            }
            else
            {
                // Desativar o IK quando não estiver mirando
                animator.SetIKPositionWeight(AvatarIKGoal.RightHand, 0);
                animator.SetIKRotationWeight(AvatarIKGoal.RightHand, 0);
            }
        }
    }
}
