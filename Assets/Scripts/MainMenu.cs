using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public string firstLevel; // Nome da cena do primeiro nível a ser carregada ao iniciar o jogo
    public string aboutScene; // Nome da cena "About" a ser carregada
    public string mainMenuScene; // Nome da cena do menu principal a ser carregada

    // Método para iniciar o jogo
    public void PlayGame()
    {
        // Carrega a cena especificada em "firstLevel"
        SceneManager.LoadScene(firstLevel);
    }

    // Método para carregar a cena "About"
    public void LoadAbout()
    {
        // Carrega a cena especificada em "aboutScene"
        SceneManager.LoadScene(aboutScene);
    }

    // Método para voltar ao menu principal
    public void LoadMainMenu()
    {
        // Carrega a cena especificada em "mainMenuScene"
        SceneManager.LoadScene(mainMenuScene);
    }

    // Método para sair do jogo
    public void QuitGame()
    {
        // Fecha a aplicação (não funcionará no Editor da Unity)
        Application.Quit();
    }
}
