using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAudio : MonoBehaviour
{
    public AudioSource footstepsSound;
    public NavMeshAgent agent;
    public float minSpeedForFootsteps = 0.00001f;

    void Start()
    {
        if (agent == null)
        {
            agent = GetComponent<NavMeshAgent>();
        }
    }

    void Update()
    {
        // Verifica a velocidade do inimigo
        if (agent.velocity.magnitude > minSpeedForFootsteps)
        {
            if (!footstepsSound.isPlaying)
            {
                footstepsSound.Play();
            }
        }
        else
        {
            if (footstepsSound.isPlaying)
            {
                footstepsSound.Pause();
            }
        }
    }
}
