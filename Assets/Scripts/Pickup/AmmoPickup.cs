using UnityEngine;

public class AmmoPickup : MonoBehaviour
{
    public int ammoAmount; // Quantidade de munição para recarregar

    private void OnTriggerEnter(Collider other)
    {
        // Verifica se o objeto que colidiu tem a tag "Player"
        if (other.tag == "Player")
        {
            // Adiciona munição à reserva do jogador acessando o script PlayerMove
            PlayerMove.instance.reserveAmmo += ammoAmount;
            Debug.Log("Munição coletada: " + ammoAmount + ", Munição em reserva: " + PlayerMove.instance.reserveAmmo);
            // Destroi o item de munição após a coleta
            Destroy(gameObject);
        }
    }
}
