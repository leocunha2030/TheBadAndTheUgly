using UnityEngine;
using UnityEngine.SceneManagement; // Necessário para gerenciamento de cenas

public class PriestScript : MonoBehaviour
{
    public string sceneToLoad; // Nome da cena a ser carregada
    public float interactionDistance = 3f; // Distância mínima para interação

    private Transform player; // Referência ao jogador

    void Start()
    {
        // Procura pelo jogador na cena
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject != null)
        {
            player = playerObject.transform;
        }
    }

    void Update()
    {
        // Verifica se o jogador está atribuído e próximo o suficiente
        if (player != null && Vector3.Distance(transform.position, player.position) <= interactionDistance)
        {
            // Detecta se a tecla E foi pressionada
            if (Input.GetKeyDown(KeyCode.E))
            {
                // Carrega a cena especificada
                SceneManager.LoadScene(sceneToLoad);
            }
        }
    }

    // Opcional: desenha uma esfera no Editor para visualizar o alcance da interação
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, interactionDistance);
    }
}
