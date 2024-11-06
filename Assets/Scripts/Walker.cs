using UnityEngine;

public class Walker : MonoBehaviour
{
    public Transform LeftFootTarget; // Alvo para a posição do pé esquerdo
    public Transform RightFootTarget; // Alvo para a posição do pé direito

    public AnimationCurve horizontalCurve; // Curva de movimento horizontal dos pés
    public AnimationCurve verticalCurve; // Curva de movimento vertical dos pés

    private Vector3 LeftTargetOffset, RightTargetOffset; // Posições iniciais dos pés
    private float LeftLegLast = 0, RightLegLast = 0; // Últimas posições dos pés

    void Start()
    {
        // Define as posições iniciais dos pés
        LeftTargetOffset = LeftFootTarget.localPosition;
        RightTargetOffset = RightFootTarget.localPosition;
    }

    void Update()
    {
        // Movimento para o pé esquerdo
        float leftLegForwardMovement = horizontalCurve.Evaluate(Time.time);
        float rightLegForwardMovement = horizontalCurve.Evaluate(Time.time - 1);

        LeftFootTarget.localPosition = LeftTargetOffset
            + this.transform.InverseTransformVector(LeftFootTarget.forward) * leftLegForwardMovement
            + this.transform.InverseTransformVector(LeftFootTarget.up) * verticalCurve.Evaluate(Time.time + 0.5f);

        RightFootTarget.localPosition = RightTargetOffset
            + this.transform.InverseTransformVector(RightFootTarget.forward) * rightLegForwardMovement
            + this.transform.InverseTransformVector(RightFootTarget.up) * verticalCurve.Evaluate(Time.time - 0.5f);

        // Detecção de contato com o solo e avanço do personagem
        RaycastHit hit;
        if (leftLegForwardMovement - LeftLegLast < 0 && Physics.Raycast(LeftFootTarget.position + LeftFootTarget.up, -LeftFootTarget.up, out hit, Mathf.Infinity))
        {
            LeftFootTarget.position = hit.point;
            this.transform.position += this.transform.forward * Mathf.Abs(leftLegForwardMovement - LeftLegLast);
        }

        if (rightLegForwardMovement - RightLegLast < 0 && Physics.Raycast(RightFootTarget.position + RightFootTarget.up, -RightFootTarget.up, out hit, Mathf.Infinity))
        {
            RightFootTarget.position = hit.point;
            this.transform.position += this.transform.forward * Mathf.Abs(rightLegForwardMovement - RightLegLast);
        }

        // Atualiza as últimas posições dos pés
        LeftLegLast = leftLegForwardMovement;
        RightLegLast = rightLegForwardMovement;
    }
}
