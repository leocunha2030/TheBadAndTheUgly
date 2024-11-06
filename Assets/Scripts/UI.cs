using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UI : MonoBehaviour
{
    public static UI instance; // Singleton para acesso global à UI
    public Slider healthSlider; // Barra de vida do jogador
    public TextMeshProUGUI healthText; // Texto que exibe a vida do jogador
    public TextMeshProUGUI killedEnemiesText; // Texto que exibe a contagem de inimigos mortos

    public TextMeshProUGUI currentAmmoText; // Texto para exibir munição atual e máxima
    public TextMeshProUGUI reserveAmmoText; // Texto para exibir munição em reserva

    public Image DamageEffect; // Efeito visual para indicar dano ao jogador
    public float damageAlpha = 0.3f, damageFadeSpeed = 3f; // Transparência e velocidade de desvanecimento do efeito de dano

    public GameObject pauseScreen; // Tela de pausa
    public int killedEnemies; // Contagem de inimigos mortos

    private void Awake()
    {
        instance = this; // Configura uma instância singleton para o UI
    }

    private void Update()
    {
        // Atualiza a exibição da munição e o efeito de dano, se necessário
        UpdateAmmoDisplay();
        if (DamageEffect.color.a != 0)
        {
            DamageEffect.color = new Color(DamageEffect.color.r, DamageEffect.color.g, DamageEffect.color.b,
                Mathf.MoveTowards(DamageEffect.color.a, 0f, damageFadeSpeed * Time.deltaTime));
        }
    }

    public void UpdateAmmoDisplay()
    {
        if (PlayerMove.instance != null)
        {
            int currentAmmo = PlayerMove.instance.currentAmmunition;
            int maxAmmo = PlayerMove.instance.maxLoadedAmmo;
            int reserveAmmo = PlayerMove.instance.reserveAmmo;

            // Exibe munição atual e máxima
            currentAmmoText.text = $"{currentAmmo}/{maxAmmo}";
            // Exibe munição em reserva
            reserveAmmoText.text = $"{reserveAmmo}";
        }
    }

    public void ShowDamage()
    {
        // Ativa o efeito visual de dano
        DamageEffect.color = new Color(DamageEffect.color.r, DamageEffect.color.g, DamageEffect.color.b, damageAlpha);
    }
}
