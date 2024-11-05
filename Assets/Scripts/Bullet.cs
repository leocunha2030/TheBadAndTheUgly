using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float bulletSpeed, lifeTime;
    public int damage;
    public Rigidbody theRigidbody;
    public bool damageEnemy, damagePlayer;

    // Prefabs dos efeitos
    public GameObject bloodSplatterPrefab;
    public GameObject dustEffectPrefab;

    void Start()
    {
    }

    void Update()
    {
        theRigidbody.linearVelocity = transform.forward * bulletSpeed;
        lifeTime -= Time.deltaTime;

        if (lifeTime <= 0)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Enemy" && damageEnemy)
        {
            // Aplica o dano ao inimigo
            other.gameObject.GetComponent<EnemyHealth>().DamageEnemy(damage);
            // Instancia o efeito de blood splatter
            Instantiate(bloodSplatterPrefab, transform.position, Quaternion.identity);
        }
        else if (other.gameObject.tag == "Head" && damageEnemy)
        {
            other.transform.parent.gameObject.GetComponent<EnemyHealth>().DamageEnemy(damage * 2);
            Instantiate(bloodSplatterPrefab, transform.position, Quaternion.identity);
        }
        else if (other.gameObject.tag == "Player" && damagePlayer)
        {
            // Aplica dano ao jogador
            PlayerHealth.instance.DamagePlayer(damage);
            // Instancia o efeito de blood splatter ao atingir o jogador
            Instantiate(bloodSplatterPrefab, transform.position, Quaternion.identity);
        }
        else
        {
            // Instancia o efeito de poeira para outros objetos
            Instantiate(dustEffectPrefab, transform.position, Quaternion.identity);
        }

        Destroy(gameObject);
    }
}
