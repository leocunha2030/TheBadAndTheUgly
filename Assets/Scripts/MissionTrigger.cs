using UnityEngine;

public class MissionTrigger : MonoBehaviour
{
    [TextArea]
    public string newMissionText; // Texto da nova missão a ser exibida

    private void OnTriggerEnter(Collider other)
    {
        // Verifica se o jogador entrou no trigger
        if (other.CompareTag("Player"))
        {
            // Atualiza o texto da missão na UI
            UI.instance.UpdateMissionText(newMissionText);

            // Desativa o gatilho para evitar repetição
            gameObject.SetActive(false);
        }
    }
}
