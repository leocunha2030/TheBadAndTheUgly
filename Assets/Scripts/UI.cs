using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UI : MonoBehaviour
{
    public static UI instance;
    public Slider healthSlider;
    public TextMeshProUGUI healthText;

    public TextMeshProUGUI currentAmmoText; // Texto para exibir munição atual e máxima
    public TextMeshProUGUI reserveAmmoText; // Texto para exibir munição em reserva

    private void Awake()
    {
        instance = this;
    }

    private void Update()
    {
        UpdateAmmoDisplay();
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
}
