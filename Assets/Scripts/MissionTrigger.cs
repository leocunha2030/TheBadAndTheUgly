using UnityEngine;

public class MissionTrigger : MonoBehaviour
{
    [TextArea]
    public string newMissionText; // Texto da nova missão a ser exibida

    public float missionStartDelay = 0f; // Tempo de atraso para iniciar a missão (em segundos)
    public SoundSpawner soundSpawner; // Referência ao script SoundSpawner

    public MissionTrigger[] otherMissionTriggers; // Lista de outras missões para parar o som

    private bool missionStarted = false; // Verifica se a missão já foi iniciada

    private void OnTriggerEnter(Collider other)
    {
        // Verifica se o jogador entrou no trigger
        if (other.CompareTag("Player") && !missionStarted)
        {
            missionStarted = true; // Marca a missão como iniciada

            // Para sons de outras missões
            StopOtherMissionSounds();

            if (missionStartDelay > 0f)
            {
                StartCoroutine(StartMissionWithDelay());
            }
            else
            {
                StartMission();
            }
        }
    }

    private void StartMission()
    {
        // Atualiza o texto da missão na UI
        UI.instance.UpdateMissionText(newMissionText);

        // Toca o som, se o SoundSpawner estiver configurado
        if (soundSpawner != null)
        {
            soundSpawner.TriggerSound();
        }

        // Desativa o gatilho para evitar repetição
        gameObject.SetActive(false);
    }

    private void StopOtherMissionSounds()
    {
        foreach (MissionTrigger mission in otherMissionTriggers)
        {
            if (mission.soundSpawner != null)
            {
                mission.soundSpawner.StopSound();
            }
        }
    }

    private System.Collections.IEnumerator StartMissionWithDelay()
    {
        yield return new WaitForSeconds(missionStartDelay);
        StartMission();
    }
}
