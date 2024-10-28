using UnityEngine;

public class HealthPickup : MonoBehaviour
{
    public int heal;
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            PlayerHealth.instance.HealPlayer(heal);
            Destroy(gameObject);
        }
    }
}
