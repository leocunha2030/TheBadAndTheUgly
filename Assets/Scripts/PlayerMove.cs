using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public float moveSpeed;
    public CharacterController characterController;
    public float mouseSensitivity;
    public float gravity = -9.81f;
    public float jumpHeight = 1.5f;
    public float runSpeedMultiplier = 1.5f; // Multiplicador para corrida

    private Vector3 moveInput;
    private Vector3 velocity;
    public Transform cameraTransform;
    public Animator animator;
    public bool canJump = true; // Variável para controlar o pulo

    public GameObject bullet;
    public Transform firePoint;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 verticalMove = transform.forward * Input.GetAxis("Vertical");
        Vector3 horizontalMove = transform.right * Input.GetAxis("Horizontal");

        // Verificar se o jogador está segurando Shift para correr
        float currentSpeed = moveSpeed;
        if (Input.GetKey(KeyCode.LeftShift))
        {
            currentSpeed *= runSpeedMultiplier;
        }

        moveInput = (horizontalMove + verticalMove).normalized * currentSpeed;

        if (characterController.isGrounded)
        {
            canJump = true;
            velocity.y = -2f; // Manter o jogador no chão
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

        // Camera rotation
        Vector2 mouseInput = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y")) * mouseSensitivity;

        transform.Rotate(Vector3.up * mouseInput.x);
        float newVerticalRotation = cameraTransform.localEulerAngles.x - mouseInput.y;
        newVerticalRotation = (newVerticalRotation > 180) ? newVerticalRotation - 360 : newVerticalRotation;
        newVerticalRotation = Mathf.Clamp(newVerticalRotation, -90f, 90f);
        cameraTransform.localEulerAngles = new Vector3(newVerticalRotation, 0f, 0f);

        // Atualizar o Animator para idle, walk, run e aim
        if (animator != null)
        {
            if (moveInput == Vector3.zero)
            {
                animator.SetFloat("Speed", 0); // Idle
            }
            else if (!Input.GetKey(KeyCode.LeftShift))
            {
                animator.SetFloat("Speed", 0.5f); // Walk
            }
            else
            {
                animator.SetFloat("Speed", 1); // Run
            }

            // Definir AimPressed quando o botão direito do mouse for pressionado
            bool isAiming = Input.GetMouseButton(1); // Botão direito do mouse
            animator.SetBool("AimPressed", isAiming);

            // Shooting logic and animation trigger
            if (isAiming && Input.GetMouseButtonDown(0)) // Botão esquerdo do mouse
            {
                animator.SetTrigger("Shooting"); // Usar trigger em vez de bool
                Instantiate(bullet, firePoint.position, firePoint.rotation);
            }
        }
    }
}
