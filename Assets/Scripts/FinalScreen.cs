using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
public class FinalScreen : MonoBehaviour
{
    public string mainMenuScene;

    public TextMeshProUGUI killedEnemiesText;
    public static FinalScreen instance;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        FinalScreen.instance.killedEnemiesText.text = "Killed Enemies: " + 0;
    }

    // Update is called once per frame
    void Update()
    {
        FinalScreen.instance.killedEnemiesText.text = "Killed Enemies: " + GameManager.instance.killedEnemies; ;
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
