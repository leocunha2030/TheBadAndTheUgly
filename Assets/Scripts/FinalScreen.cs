using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class FinalScreen : MonoBehaviour
{
    public string mainMenuScene;
    public TextMeshProUGUI killedEnemiesText;
    public static FinalScreen instance;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
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
        if (killedEnemiesText != null && GameManager.instance != null)
        {
            killedEnemiesText.text = "Killed Enemies: " + GameManager.instance.killedEnemies;
        }
    }

    public void MainMenu()
    {
        SceneManager.LoadScene(mainMenuScene);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
