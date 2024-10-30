using UnityEngine;

public class AmmoPickup : MonoBehaviour
{
    public int ammoAmount; // Quantidade de munição para recarregar

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            PlayerMove.instance.reserveAmmo += ammoAmount;
            Debug.Log("Munição coletada: " + ammoAmount + ", Munição em reserva: " + PlayerMove.instance.reserveAmmo);
            Destroy(gameObject);
        }
    }
}
