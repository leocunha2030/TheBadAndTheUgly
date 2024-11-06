using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public string firstLevel; // Nome da cena do primeiro nível a ser carregada ao iniciar o jogo

    // Método para iniciar o jogo
    public void PlayGame()
    {
        // Carrega a cena especificada em "firstLevel"
        SceneManager.LoadScene(firstLevel);
    }

    // Método para sair do jogo
    public void QuitGame()
    {
        // Fecha a aplicação (não funcionará no Editor da Unity)
        Application.Quit();
    }
}
