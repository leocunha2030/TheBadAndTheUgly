using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class PlayerHealth : MonoBehaviour
{
    public static PlayerHealth instance;
    public int maxHealth, currentHealth;

    public float timeUnitlFinalScreen = 1f;
    public string finalScreenScene;

    private void Awake()
    {
        instance = this;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentHealth = maxHealth;
        UI.instance.healthSlider.maxValue = maxHealth;
        UI.instance.healthSlider.value = currentHealth;
        UI.instance.healthText.text = "Health: " + currentHealth + "/" + maxHealth;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void DamagePlayer(int damage)
    {
        currentHealth -= damage;
        UI.instance.ShowDamage();
        if (currentHealth <= 0)
        {
            //gameObject.SetActive(false);

            currentHealth = 0;
            StartCoroutine(WaitingForFinalScreen());
            //GameManager.instance.PlayerDeath();
        }

        UI.instance.healthSlider.value = currentHealth;
        UI.instance.healthText.text = "Health: " + currentHealth + "/" + maxHealth;
    }
    public void HealPlayer(int heal)
    {
        currentHealth += heal;

        if(currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
        UI.instance.healthSlider.value = currentHealth;
        UI.instance.healthText.text = "Health: " + currentHealth + "/" + maxHealth;
    }
    public IEnumerator WaitingForFinalScreen()
    {
        yield return new WaitForSeconds(timeUnitlFinalScreen);
        SceneManager.LoadScene(finalScreenScene);
        Cursor.lockState = CursorLockMode.None;
    }
}
