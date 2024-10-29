using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public float moveSpeed;
    public float mouseSensitivity;
    public float gravity = -9.81f;
    public float jumpHeight = 1.5f;
    public float runSpeedMultiplier = 1.5f; // Multiplicador para corrida
    public float jumpVelocity; // Velocidade do pulo

    public static PlayerMove instance;

    private Vector3 moveInput;
    private Vector3 velocity;
    public Transform cameraTransform;
    public bool canJump = true; // Variável para controlar o pulo

    public GameObject bullet;
    public Transform firePoint;

    public CharacterController characterController;

    private void Awake()
    {
        instance = this;
    }
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
                jumpVelocity = velocity.y;
                canJump = false;
            }
        }

        velocity.y += gravity * Time.deltaTime;

        // Aplicar movimento
        characterController.Move((moveInput + new Vector3(0, velocity.y, 0)) * Time.deltaTime);

        // Camera rotation
        Vector2 mouseInput = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y")) * mouseSensitivity;

        transform.Rotate(Vector3.up * mouseInput.x);
        float newVerticalRotation = cameraTransform.localEulerAngles.x - mouseInput.y;
        newVerticalRotation = (newVerticalRotation > 180) ? newVerticalRotation - 360 : newVerticalRotation;
        newVerticalRotation = Mathf.Clamp(newVerticalRotation, -90f, 90f);
        cameraTransform.localEulerAngles = new Vector3(newVerticalRotation, 0f, 0f);

        // Shooting
        if (Input.GetMouseButton(1) && Input.GetMouseButtonDown(0)) // Botão direito e esquerdo do mouse
        {
            // Código adicionado para ajustar a direção do firePoint
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
        }
    }
}
