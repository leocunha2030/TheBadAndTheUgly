using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance; // Singleton para acesso global
    public float waitAfterDeath = 3f; // Tempo de espera após a morte do jogador
    public int killedEnemies; // Contagem de inimigos mortos

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Mantém o GameManager entre cenas
        }
        else
        {
            Destroy(gameObject); // Garante que apenas uma instância do GameManager exista
        }
    }

    void Start()
    {
        // Bloqueia o cursor na tela
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        // Verifica se o jogador pressionou ESC para pausar/despausar
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PauseUnpause();
        }
    }

    public void PauseUnpause()
    {
        // Alterna entre pausar e despausar o jogo
        if (UI.instance.pauseScreen.activeInHierarchy)
        {
            UI.instance.pauseScreen.SetActive(false);
            Cursor.lockState = CursorLockMode.Locked;
            Time.timeScale = 1f;
        }
        else
        {
            UI.instance.pauseScreen.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            Time.timeScale = 0f;
        }
    }
}
