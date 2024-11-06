using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class FinalScreen : MonoBehaviour
{
    public string mainMenuScene; // Nome da cena do menu principal
    public TextMeshProUGUI killedEnemiesText; // Texto UI para exibir o número de inimigos mortos
    public static FinalScreen instance;

    private void Awake()
    {
        instance = this; // Configura uma instância singleton para acesso global
    }

    void Start()
    {
        // Inicializa o texto de inimigos mortos, exibindo zero no início
        if (killedEnemiesText != null)
        {
            killedEnemiesText.text = "Killed Enemies: " + 0;
        }
        else
        {
            Debug.LogError("killedEnemiesText não foi atribuído no Inspetor.");
        }
    }

    void Update()
    {
        // Atualiza o texto de inimigos mortos com base na contagem do GameManager
        if (killedEnemiesText != null && GameManager.instance != null)
        {
            killedEnemiesText.text = "Killed Enemies: " + GameManager.instance.killedEnemies;
        }
    }

    public void MainMenu()
    {
        // Carrega a cena do menu principal
        SceneManager.LoadScene(mainMenuScene);
    }

    public void QuitGame()
    {
        // Fecha a aplicação
        Application.Quit();
    }
}
