using UnityEngine;
using UnityEngine.SceneManagement;

public class Pause : MonoBehaviour
{
    public string mainMenu; // Nome da cena do menu principal

    // Método para retomar o jogo
    public void Resume()
    {
        // Chama o método de pausa do GameManager para alternar entre pausar/despausar
        GameManager.instance.PauseUnpause();
    }

    // Método para voltar ao menu principal
    public void MainMenu()
    {
        // Carrega a cena do menu principal
        SceneManager.LoadScene(mainMenu);
    }

    // Método para sair do jogo
    public void QuitGame()
    {
        // Fecha a aplicação
        Application.Quit();
    }
}
