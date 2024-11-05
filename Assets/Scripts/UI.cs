using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UI : MonoBehaviour
{
    public static UI instance;
    public Slider healthSlider;
    public TextMeshProUGUI healthText;
    public TextMeshProUGUI killedEnemiesText;

    public TextMeshProUGUI currentAmmoText; // Texto para exibir munição atual e máxima
    public TextMeshProUGUI reserveAmmoText; // Texto para exibir munição em reserva

    public Image DamageEffect;
    public float damageAlpha = 0.3f, damageFadeSpeed = 3f;

    public GameObject pauseScreen;
    public int killedEnemies;
    
    private void Awake()
    {
        instance = this;
    }

    private void Update()
    {
        UpdateAmmoDisplay();
        if(DamageEffect.color.a != 0)
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

            // Exibir munição atual e máxima
            currentAmmoText.text = $"{currentAmmo}/{maxAmmo}";

            // Exibir munição em reserva
            reserveAmmoText.text = $"{reserveAmmo}";
        }
    }
    public void ShowDamage()
    {
        DamageEffect.color = new Color(DamageEffect.color.r, DamageEffect.color.g, DamageEffect.color.b, 0.3f);
    }
}
