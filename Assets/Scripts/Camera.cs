using UnityEngine;

public class Camera : MonoBehaviour
{
    public Transform cameraTarget; // O alvo que a câmera seguirá

    void LateUpdate()
    {
        // Posiciona e rotaciona a câmera para coincidir com o alvo
        transform.position = cameraTarget.position;
        transform.rotation = cameraTarget.rotation;
    }
}
