using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class PlayerHealth : MonoBehaviour
{
    public static PlayerHealth instance; // Singleton para acesso global
    public int maxHealth, currentHealth; // Valores de vida máxima e atual do jogador
    public float timeUntilFinalScreen = 1f; // Tempo até a tela final após a morte
    public string finalScreenScene; // Nome da cena de tela final

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        // Inicializa a vida e atualiza a UI
        currentHealth = maxHealth;
        UI.instance.healthSlider.maxValue = maxHealth;
        UI.instance.healthSlider.value = currentHealth;
        UI.instance.healthText.text = "Health: " + currentHealth + "/" + maxHealth;
    }

    public void DamagePlayer(int damage)
    {
        // Reduz a vida do jogador e exibe efeito de dano na UI
        currentHealth -= damage;
        UI.instance.ShowDamage();

        if (currentHealth <= 0)
        {
            // Define a vida como zero e inicia a contagem para a tela final
            currentHealth = 0;
            StartCoroutine(WaitingForFinalScreen());
        }

        // Atualiza a UI com a nova vida
        UI.instance.healthSlider.value = currentHealth;
        UI.instance.healthText.text = "Health: " + currentHealth + "/" + maxHealth;
    }

    public void HealPlayer(int heal)
    {
        // Aumenta a vida do jogador até o máximo permitido
        currentHealth += heal;
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
        UI.instance.healthSlider.value = currentHealth;
        UI.instance.healthText.text = "Health: " + currentHealth + "/" + maxHealth;
    }

    private IEnumerator WaitingForFinalScreen()
    {
        yield return new WaitForSeconds(timeUntilFinalScreen);
        SceneManager.LoadScene(finalScreenScene); // Carrega a cena final após a morte do jogador
        Cursor.lockState = CursorLockMode.None;
    }
}
