using UnityEngine;

public class MissionTrigger : MonoBehaviour
{
    [TextArea]
    public string newMissionText; // Texto da nova missão a ser exibida

    public float missionStartDelay = 0f; // Tempo de atraso para iniciar a missão (em segundos)

    private bool missionStarted = false; // Verifica se a missão já foi iniciada

    private void OnTriggerEnter(Collider other)
    {
        // Verifica se o jogador entrou no trigger
        if (other.CompareTag("Player") && !missionStarted)
        {
            missionStarted = true; // Marca a missão como iniciada

            if (missionStartDelay > 0f)
            {
                // Aguarda o tempo especificado antes de iniciar a missão
                StartCoroutine(StartMissionWithDelay());
            }
            else
            {
                // Inicia a missão imediatamente
                StartMission();
            }
        }
    }

    private void StartMission()
    {
        // Atualiza o texto da missão na UI
        UI.instance.UpdateMissionText(newMissionText);

        // Desativa o gatilho para evitar repetição
        gameObject.SetActive(false);
    }

    private System.Collections.IEnumerator StartMissionWithDelay()
    {
        // Aguarda o tempo especificado antes de iniciar a missão
        yield return new WaitForSeconds(missionStartDelay);
        StartMission();
    }
}
