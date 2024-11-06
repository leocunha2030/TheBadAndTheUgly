using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float bulletSpeed, lifeTime; // Velocidade e dura��o da bala
    public int damage; // Quantidade de dano da bala
    public Rigidbody theRigidbody; // Corpo r�gido da bala para manipular f�sica
    public bool damageEnemy, damagePlayer; // Flags para determinar se a bala pode causar dano a inimigos ou ao jogador

    public GameObject bloodSplatterPrefab; // Efeito de sangue ao acertar inimigos
    public GameObject dustEffectPrefab; // Efeito de poeira ao acertar outros objetos

    void Update()
    {
        // Define a velocidade da bala e decrementa o tempo de vida
        theRigidbody.linearVelocity = transform.forward * bulletSpeed;
        lifeTime -= Time.deltaTime;

        // Destroi a bala quando o tempo de vida expira
        if (lifeTime <= 0)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Caso a bala acerte um inimigo
        if (other.gameObject.tag == "Enemy" && damageEnemy)
        {
            // Aplica dano ao inimigo e instancia efeito de sangue
            other.gameObject.GetComponent<EnemyHealth>().DamageEnemy(damage);
            Instantiate(bloodSplatterPrefab, transform.position, Quaternion.identity);
        }
        else if (other.gameObject.tag == "Head" && damageEnemy)
        {
            // Aplica dano em dobro ao atingir a cabe�a do inimigo e instancia efeito de sangue
            other.transform.parent.gameObject.GetComponent<EnemyHealth>().DamageEnemy(damage * 2);
            Instantiate(bloodSplatterPrefab, transform.position, Quaternion.identity);
        }
        else if (other.gameObject.tag == "Player" && damagePlayer)
        {
            // Aplica dano ao jogador e instancia efeito de sangue
            PlayerHealth.instance.DamagePlayer(damage);
            Instantiate(bloodSplatterPrefab, transform.position, Quaternion.identity);
        }
        else
        {
            // Instancia efeito de poeira ao atingir outros objetos
            Instantiate(dustEffectPrefab, transform.position, Quaternion.identity);
        }

        // Destroi a bala ap�s a colis�o
        Destroy(gameObject);
    }
}
